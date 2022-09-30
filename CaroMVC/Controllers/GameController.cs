using Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Model.GameModels;
using Model.ActionModels;
using Service.APIClientServices;
using Model.RequestModels;
using AutoMapper;
using Model.ResponseModels;

namespace CaroMVC.Controllers;

[Authorize]
public class GameController : Controller
{
    private readonly GameDbContext _context;
    private readonly IResultApiClient _resultApiClient;
    private readonly IMapper _mapper;

    public GameController(
        GameDbContext context, 
        IResultApiClient resultApiClient,
        IMapper mapper)
    {
        _context = context;
        _resultApiClient = resultApiClient;
        _mapper = mapper;
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

    public async Task<IActionResult> History()
    {
        var userName = User.Identity!.Name;
        if (userName == null)
        {
            ViewData["Error"] = "Unauthorized";
            return View();
        }
        var response = await _resultApiClient.GetHistoryByUserName(userName, new PagingRequest());
        if (!response.Succeeded)
        {
            ViewData["Error"] = "Get Result Failure!";
            return View();
        }
        var gameResults = response.ResultObject;
        var model = gameResults.Select(gr => new HistoryModel { Input = gr });
        return View(model);
    }

}