using Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Model.GameModels;
using Model.ActionModels;
using Service.APIClientServices;
using Model.RequestModels;

namespace CaroMVC.Controllers;

public class GameController : Controller
{
    private readonly GameDbContext _context;
    private readonly IResultApiClient _resultApiClient;
    private readonly IUserApiClient _userApiClient;

    public GameController(
        GameDbContext context, 
        IResultApiClient resultApiClient,
        IUserApiClient userApiClient)
    {
        _context = context;
        _resultApiClient = resultApiClient;
        _userApiClient = userApiClient;
    }
        
    public ActionResult Index(string? error)
    {
        if (error != null)
        {
            ViewData["Error"] = error;
        }
        var roomList = _context.Rooms.Include(room => room.GameUsers).ToList();
        return View(roomList);
    }

    public async Task<IActionResult> Play(string? roomName)
    {
        var room = await _context.Rooms.Include(room => room.GameUsers).FirstOrDefaultAsync(room => room.RoomName == roomName);
        if (room == null)
        {
            return RedirectToAction("Index", "Game", new { error = "Room does not exist!" });
        }
        if (room.GameUsers.All(user => user.UserName != User.Identity!.Name))
        {
            return RedirectToAction("Index", "Game", new { error = "You cannot join this game!" });

        }
        var board = new Board { RowCount = 30, ColumnCount = 30};
        var model = new PlayModel { Room = room, Board = board };
        return View(model);
    }

    public async Task<IActionResult> History(int? pageIndex, int? pageSize)
    {
        var userName = User.Identity!.Name;
        if (userName == null)
        {
            ViewData["Error"] = "Unauthorized";
            return View();
        }
        var request = new PagingRequest();
        if (pageIndex != null)
            request.PageIndex = (int)pageIndex;
        if (pageSize != null)
            request.PageSize = (int)pageSize;
        var response = await _resultApiClient.GetHistoryByUserName(userName, request);
        if (!response.Succeeded)
        {
            ViewData["Error"] = "Get Result Failure!";
            return View();
        }
        var gameResults = response.ResultObject;
        var model = new HistoryModel { Input = gameResults };
        return View(model);
    }
    [AllowAnonymous]
    public async Task<IActionResult> Ranking(int? pageIndex, int? pageSize)
    {
        var request = new PagingRequest();
        if (pageIndex != null)
            request.PageIndex = (int)pageIndex;
        if (pageSize != null)
            request.PageSize = (int)pageSize;
        var response = await _userApiClient.GetRanking(request);
        if (!response.Succeeded)
        {
            ViewData["Error"] = "Get Result Failure!";
            return View();
        }
        var rankings = response.ResultObject;
        var model = new RankingModel
        {
            AllPlayerRanks = rankings
        };
        var userName = User.Identity!.Name;
        if (userName == null)
        {
            ViewData["Error"] = "Unauthorized";
            return View(model);
        }
        var playerRank = rankings.Items.FirstOrDefault(r => r.UserName == userName);
        if (playerRank != null)
            model.PlayerRank = playerRank;
        return View(model);
    }
}