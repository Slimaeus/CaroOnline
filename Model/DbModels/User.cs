using Microsoft.AspNetCore.Identity;

namespace Model.DbModels
{
    public class User : IdentityUser<Guid>
    {
        [PersonalData]
        public string InGameName { get; set; } = string.Empty;
        [PersonalData]
        public int Level { get; set; } = 1;
        [PersonalData]
        public int Exp { get; set; } = 0;
        [PersonalData]
        public int Score { get; set; } = 0;

        public ICollection<UserResult> UserResults { get; set; } = new List<UserResult>();
    }
}
