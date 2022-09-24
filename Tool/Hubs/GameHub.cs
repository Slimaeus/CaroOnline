using Data;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Model.GameModels;
using System.Security.Claims;

namespace Utility.Hubs
{
    public class GameHub : Hub
    {
        private readonly GameDbContext context;

        public GameHub(GameDbContext context)
        {
            this.context = context;
        }
        public override async Task OnConnectedAsync()
        {
            // Retrieve user.
            var user = context.GameUsers
                .Include(u => u.Rooms)
                .SingleOrDefault(u => u.UserName == Context.User.Identity!.Name!);

            // If user does not exist in database, must add.
            if (user == null)
            {
                user = new GameUser()
                {
                    UserName = Context.User.Identity!.Name!
                };
                context.GameUsers.Add(user);
                await context.SaveChangesAsync();

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
            var room = await context.Rooms.FindAsync(roomName);

            if (room == null)
            {
                room = new PlayRoom { RoomName = roomName };
                await context.Rooms.AddAsync(room);
                await context.SaveChangesAsync();
            }
            if (room != null)
            {
                var user = new GameUser() { UserName = Context.User.Identity!.Name! };
                context.GameUsers.Attach(user);
                room.GameUsers.Add(user);
                await context.SaveChangesAsync();

                await Groups.AddToGroupAsync(Context.ConnectionId, roomName);

                await Clients.All.SendAsync("addRoom", roomName);
            }
        }
        public async Task RoomJoin(string roomName)
        {
            var room = await context.Rooms.FindAsync(roomName);
            
            if (room == null)
            {
                room = new PlayRoom { RoomName = roomName };
                await context.Rooms.AddAsync(room);
                await context.SaveChangesAsync();
            }
            if (room != null)
            {
                var user = new GameUser() { UserName = Context.User.Identity!.Name! };
                context.GameUsers.Attach(user);
                room.GameUsers.Add(user);
                await context.SaveChangesAsync();

                await Groups.AddToGroupAsync(Context.ConnectionId, roomName);

                await Clients.All.SendAsync("addRoom", roomName);
            }
        }
        public async Task RoomLeave(string roomName)
        {
            var room = await context.Rooms.Include(room => room.GameUsers).FirstOrDefaultAsync(room => room.RoomName == roomName);
            if (room != null)
            {
                var user = await context.GameUsers.FindAsync(Context.User.Identity!.Name!);
                if (user ==  null)
                {
                    user = new GameUser() { UserName = Context.User.Identity!.Name! };
                }
                context.GameUsers.Attach(user);

                room.GameUsers.Remove(user);
                if (room.GameUsers.Count == 0)
                    context.Rooms.Remove(room);
                await context.SaveChangesAsync();
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomName);
                await Clients.All.SendAsync("leaveRoom", roomName);
            }
        }
        public async Task RoomChat(string roomName, string message)
        {
            await Clients.Group(roomName).SendAsync(message);
        }
        public async Task PlaceStone(int x, int y)
        {
            ClaimsPrincipal user = Context.User;
            string userId = "Guest";
            if (user != null)
                userId = user.Identity!.Name!;
            await Clients.All.SendAsync("updateBoard", x, y, userId);
        }
    }
}
