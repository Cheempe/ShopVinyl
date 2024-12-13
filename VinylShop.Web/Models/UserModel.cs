namespace VinylShop.Web.Models
{
    public class UserModel
    {
        public required Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required string Age { get; set; }
    }
}
