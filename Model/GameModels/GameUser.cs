using System.ComponentModel.DataAnnotations;

namespace Model.GameModels;

public sealed class GameUser
{
    [Key]
    public string UserName { get; init; } = default!;
    public ICollection<Connection> Connections { get; set; } = new List<Connection>();
    public ICollection<PlayRoom> Rooms { get; set; } = new List<PlayRoom>();
}