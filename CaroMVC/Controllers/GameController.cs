using Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Model.GameModels;
using System.Text;
using Model.ActionModels;

namespace CaroMVC.Controllers
{
    [Authorize]
    public class GameController : Controller
    {
        private readonly GameDbContext _context;

        public GameController(GameDbContext context)
        {
            _context = context;
        }
        
        public ActionResult Index()
        {
            var roomList = _context.Rooms.Include(room => room.GameUsers).ToList();
            return View(roomList);
        }

        public async Task<IActionResult> Play(string? roomName)
        {
            var room = await _context.Rooms.Include(room => room.GameUsers).FirstOrDefaultAsync(room => room.RoomName == roomName);
            if (room == null)
                return RedirectToAction("Index", "Game");
            if (room.GameUsers.All(user => user.UserName != User.Identity!.Name))
                return RedirectToAction("Index", "Game");
            var board = new Board { RowCount = 30, ColumnCount = 30};
            var model = new PlayModel { Room = room, Board = board };
            return View(model);
        }

        public ActionResult Create()
        {
            return View();
        }

    }
}
