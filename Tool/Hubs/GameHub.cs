using Data;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Model.GameModels;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Model.DbModels;
using Service.APIClientServices;

namespace Utility.Hubs
{
    [Authorize]
    public class GameHub : Hub
    {
        private readonly GameDbContext _context;

        public GameHub(GameDbContext context)
        {
            _context = context;
        }
        public override async Task OnConnectedAsync()
        {
            // Retrieve user.
            var user = _context.GameUsers
                .Include(u => u.Rooms)
                .SingleOrDefault(u => u.UserName == Context.User.Identity!.Name!);

            // If user does not exist in database, must add.
            if (user == null)
            {
                user = new GameUser()
                {
                    UserName = Context.User.Identity!.Name!
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
        public async Task RoomCreate()
        {
            string roomName = Context.User.Identity!.Name!;
            var room = await _context.Rooms.FindAsync(roomName);

            if (room == null)
            {
                room = new PlayRoom { RoomName = roomName };
                await _context.Rooms.AddAsync(room);
                await _context.SaveChangesAsync();
            }
            if (room != null)
            {
                var user = new GameUser() { UserName = Context.User.Identity!.Name! };
                _context.GameUsers.Attach(user);
                room.GameUsers.Add(user);
                await _context.SaveChangesAsync();

                await Groups.AddToGroupAsync(Context.ConnectionId, roomName);

                await Clients.All.SendAsync("addRoom", roomName);
            }
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
            if (room != null)
            {
                var user = new GameUser() { UserName = Context.User.Identity!.Name! };
                _context.GameUsers.Attach(user);
                room.GameUsers.Add(user);
                await _context.SaveChangesAsync();

                await Groups.AddToGroupAsync(Context.ConnectionId, roomName);

                await Clients.All.SendAsync("addRoom", roomName);
            }
        }
        public async Task RoomLeave(string roomName)
        {
            var room = await _context.Rooms.Include(room => room.GameUsers).FirstOrDefaultAsync(room => room.RoomName == roomName);
            if (room != null)
            {
                var user = await _context.GameUsers.FindAsync(Context.User.Identity!.Name!);
                if (user ==  null)
                {
                    user = new GameUser() { UserName = Context.User.Identity!.Name! };
                }
                _context.GameUsers.Attach(user);

                room.GameUsers.Remove(user);
                if (room.GameUsers.Count == 0)
                    _context.Rooms.Remove(room);
                await _context.SaveChangesAsync();
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomName);
                await Clients.All.SendAsync("leaveRoom", roomName);
            }
        }
        public async Task PlaceStone(string roomName, int row, int col)
        {
            var userName = Context.User.Identity?.Name;
            await Clients.OthersInGroup(roomName).SendAsync("updateBoard", userName, row, col);
        }
    }
}
