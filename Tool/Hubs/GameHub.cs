using Data;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Model.GameModels;
using Microsoft.AspNetCore.Authorization;
using Service.APIClientServices;
using Model.RequestModels;
using Microsoft.Extensions.Caching.Memory;

namespace Utility.Hubs;

[Authorize]
public class GameHub : Hub
{
    private readonly GameDbContext _context;
    private readonly IUserApiClient _userApiClient;
    private readonly IResultApiClient _resultApiClient;
    private readonly IMemoryCache _cache;

    public GameHub(GameDbContext context, IUserApiClient userApiClient, IResultApiClient resultApiClient, IMemoryCache cache)
    {
        _context = context;
        _userApiClient = userApiClient;
        _resultApiClient = resultApiClient;
        _cache = cache;
    }
    public override async Task OnConnectedAsync()
    {
        // Retrieve user.
        var user = _context.GameUsers
            .Include(u => u.Rooms)
            .SingleOrDefault(u => u.UserName == Context.User!.Identity!.Name!);

        // If user does not exist in database, must add.
        if (user == null)
        {
            user = new GameUser()
            {
                UserName = Context.User!.Identity!.Name!
            };
            _context.GameUsers.Add(user);
            await _context.SaveChangesAsync();

        }
        else
        {
            var tasks = user.Rooms.Select(room => Groups.AddToGroupAsync(Context.ConnectionId, room.RoomName));
            await Task.WhenAll(tasks);
        }

        await base.OnConnectedAsync();
    }
    // Reset Room Data
    public async Task ResetRoom(string roomName)
    {
        _cache.Remove(roomName);
        await Clients.Group(roomName).SendAsync("reseted");
    }
    // Save Game Data
    public async Task SaveGame(string roomName, object game)
    {
        Console.WriteLine(roomName + "has " + game.ToString());
        _cache.Set(roomName, game);
        await Clients.Group(roomName).SendAsync("saved");
    }
    // Reset Game Data
    public async Task Reconnect(string roomName)
    {
        _cache.TryGetValue(roomName, out object game);
        await Clients.Group(roomName).SendAsync("reload", game);
    }
    public async Task RoomCreate()
    {
        string roomName = Context.User!.Identity!.Name!;
        var room = await _context.Rooms.FindAsync(roomName);

        if (room == null)
        {
            room = new PlayRoom { RoomName = roomName };
            await _context.Rooms.AddAsync(room);
            await _context.SaveChangesAsync();
        }
        var user = new GameUser() { UserName = Context.User.Identity!.Name! };
        _context.GameUsers.Attach(user);
        room.GameUsers.Add(user);
        await _context.SaveChangesAsync();

        await Groups.AddToGroupAsync(Context.ConnectionId, roomName);

        await Clients.All.SendAsync("addRoom", roomName);
    }
    public async Task RoomJoin(string roomName)
    {
        var room = await _context.Rooms.FindAsync(roomName);
            
        if (room == null)
        {
            room = new PlayRoom { RoomName = roomName };
            await _context.Rooms.AddAsync(room);
            await _context.SaveChangesAsync();
        }
        var user = new GameUser() { UserName = Context.User!.Identity!.Name! };
        _context.GameUsers.Attach(user);
        room.GameUsers.Add(user);
        await _context.SaveChangesAsync();

        await Groups.AddToGroupAsync(Context.ConnectionId, roomName);

        await Clients.All.SendAsync("addRoom", roomName);
        await Clients.OthersInGroup(roomName).SendAsync("newPlayerJoin", user.UserName);
        await Clients.OthersInGroup(roomName).SendAsync("enableBoard");
    }
    public async Task RoomLeave(string roomName)
    {
        var room = await _context.Rooms.Include(room => room.GameUsers).FirstOrDefaultAsync(room => room.RoomName == roomName);
        if (room != null)
        {
            var user = await _context.GameUsers.FindAsync(Context.User!.Identity!.Name!);
            if (user ==  null)
            {
                user = new GameUser() { UserName = Context.User.Identity!.Name! };
            }
            _context.GameUsers.Attach(user);

            room.GameUsers.Remove(user);
            if (room.GameUsers.Count == 0 || room.GameUsers.All(u => u.UserName != roomName))
                _context.Rooms.Remove(room);
            await _context.SaveChangesAsync();
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomName);
            await Clients.All.SendAsync("leaveRoom", roomName);
        }
    }
    public async Task PlaceStone(string roomName, int row, int col)
    {
        var userName = Context.User!.Identity!.Name;
        var room  = await _context.Rooms.Include(room => room.GameUsers).FirstOrDefaultAsync(r => r.RoomName == roomName);
        if (room == null)
            return;
        var players = room.GameUsers.ToList();
        var index = players.FindIndex(p => p.UserName == userName);
        var colorList = new List<string>
        {
            "success bi bi-circle",
            "danger bi bi-x-lg",
            "primary bi bi-star"
        };
        var color = (index != -1 && index < colorList.Count) ? colorList[index] : "outline-dark bi bi-hand-index-thumb text-white";
        await Clients.Group(roomName).SendAsync("updateBoard", userName, row, col, color);
        await Clients.OthersInGroup(roomName).SendAsync("enableBoard");
    }

    public async Task GameEnd(string roomName, string winnerUserName, DateTime startedDate)
    {
        var now = DateTime.Now;
        if (string.IsNullOrEmpty(winnerUserName))
        {
            await Clients.Group(roomName).SendAsync("gameEndError", "Winner name is null");
            return;
        }
        var winner = await _userApiClient.GetByUserName(winnerUserName);
        if (winner == null)
        {
            await Clients.Group(roomName).SendAsync("gameEndError", "Cannot Found Winner");
            return;
        }
        if (!winner.Succeeded)
        {
            await Clients.Group(roomName).SendAsync("gameEndError", "Found Winner Failure");
            return;
        }

        var room = await _context.Rooms.Include(r => r.GameUsers).FirstOrDefaultAsync(r => r.RoomName.Equals(roomName));
        if (room == null)
        {
            await Clients.Group(roomName).SendAsync("gameEndError", "Cannot Found Room");
            return;
        }
        var loserUserName = room.GameUsers.Where(u => !u.UserName.Equals(winnerUserName)).Select(u => u.UserName).FirstOrDefault();
        if (loserUserName == null)
        {
            await Clients.Group(roomName).SendAsync("gameEndError", "Cannot Found Loser");
            return;
        }
        ResultRequest request = new()
        {
            WinnerUserName = winnerUserName,
            LoserUserName = loserUserName,
            StartedTime = startedDate,
            EndedTime = now
        };
        var result = await _resultApiClient.Create(request);
        if (!result.Succeeded)
        {
            await Clients.Group(roomName).SendAsync("gameEndError", $"Create result failure because {result.Message}");
            return;
        }

        var winnerInGameName = winner.ResultObject.InGameName;
        await Clients.Group(roomName).SendAsync("gameEnd", winnerUserName, winnerInGameName);
    }
    public async Task GameRematch(string roomName)
    {
        await Clients.Group(roomName).SendAsync("rematch");
        await Clients.OthersInGroup(roomName).SendAsync("enableBoard");
    }
}