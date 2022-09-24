using Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Model.GameModels;
using System.Text;

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
            var room = await _context.Rooms.FindAsync(roomName);
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

    }
}
