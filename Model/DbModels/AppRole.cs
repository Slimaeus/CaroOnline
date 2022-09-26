using Microsoft.AspNetCore.Identity;

namespace Model.DbModels;

public class AppRole : IdentityRole<Guid>
{
    public string Description { get; set; } = string.Empty;
}