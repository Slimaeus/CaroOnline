using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DbModels
{
    public class UserResult
    {
        [Column(Order = 0)]
        public Guid UserId { get; set; }
        public AppUser User { get; set; } = default!;

        [Column(Order = 1)]
        public Guid ResultId { get; set; }
        public Result Result { get; set; } = default!;

        public bool IsWinner { get; set; }
        public int Score { get; set; }
        public int Order { get; set; }
    }
}
