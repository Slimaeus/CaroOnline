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

        public ActionResult Play(Board? board)
        {
            board ??= new Board { RowCount = 5, ColumnCount = 5 };
            return View(board);
        }

        public ActionResult Create()
        {
            return View();
        }

    }
}
