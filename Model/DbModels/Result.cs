using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DbModels
{
    public class Result
    {
        [Key]
        public Guid Id { get; set; }

        public DateTime StartedTime { get; set; } = default!;
        public DateTime EndedTime { get; set; } = default!;

        public int LimitTime { get; set; }

        public ICollection<UserResult> UserResults { get; set; } = default!;
    }
}
