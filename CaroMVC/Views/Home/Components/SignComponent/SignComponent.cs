using Microsoft.AspNetCore.Mvc;
using System.Drawing;

namespace CaroMVC.Views.Home.Components.SignComponent
{
    public class SignComponent : ViewComponent
    {
        public IViewComponentResult Invoke(int row, int col)
        {
            //string color = "primary";
            //if (row % 2 == 0)
            //    color = "black";
            var model = new SignViewModel
            {
                Row = row,
                Col = col,
                Color = (row % 2 == 0) ? "primary" : "danger",
                Shape = (row % 2 == 0) ? "x" : "circle"
            };
            return View(model);
        }
        public class SignViewModel {
            public int Row { get; set; }
            public int Col { get; set; }
            public string Color { get; set; } = "black";
            public string Shape { get; set; } = "circle";
        }

    }
}
