using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.RequestModels
{
    public class ResultRequest
    {
        public string WinnerUserName { get; set; } = string.Empty;
        public string LoserUserName { get; set; } = string.Empty;
        public int Hour { get; set; }
        [Range(0, 60)]
        public int Minute { get; set; }
        [Range(0, 60)]
        public int Second { get; set; }
        public int LimitTime { get; set; }
    }
}
