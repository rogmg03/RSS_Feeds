namespace RSS_Feeds.MVC.Models
{
    public class RegisterViewModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string? Nombre { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
