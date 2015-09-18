using LunchPac.Attributes;

namespace LunchPac.Models
{
    [Table("Users")]
    public class User
    {
        [PrimaryKey]
        public int UserId { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
    }
}
