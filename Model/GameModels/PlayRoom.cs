using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.GameModels
{
    public sealed class PlayRoom
    {
        [Key]
        public string RoomName { get; init; } = default!;
        public int RoomMax { get; set; } = 2;
        public ICollection<GameUser> GameUsers { get; } = new List<GameUser>();
    }
}
