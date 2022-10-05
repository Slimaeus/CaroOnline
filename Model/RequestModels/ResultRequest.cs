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
        [Required]
        public string WinnerUserName { get; set; } = string.Empty;
        [Required]

        public string LoserUserName { get; set; } = string.Empty;
        public DateTime StartedTime  { get; set; } = DateTime.Now;
        public DateTime EndedTime { get; set; } = DateTime.Now;
        public int LimitTime { get; set; } = 0;
    }
}
