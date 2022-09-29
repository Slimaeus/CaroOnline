using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ResponseModels
{
    public class ResultResponse
    {
        public Guid Id { get; set; }
        public DateTime StartedTime { get; set; } = DateTime.Now;
        public DateTime EndedTime { get; set; } = DateTime.Now;

        public int LimitTime { get; set; }
    }
}
