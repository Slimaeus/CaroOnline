using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ActionModels
{
    public class HistoryModel
    {
        public Guid Id { get; set; }
        public string GameMode { get; set; } = string.Empty;
        public DateTime StartedTime { get; set; }
        public DateTime EndedTime { get; set; }
        public string Opponent { get; set; } = string.Empty;
        public int Win { get; set; } = 0;
        public int Draw { get; set; } = 0;
        public int Lose { get; set; } = 0;
        public int Score { get; set; } = 0;
    }
}
