namespace Domain.User
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        // Authentication-related fields
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string Role { get; set; }
        // Refresh token fields
        //public string RefreshTokenHash { get; set; }
        //public DateTime? RefreshTokenExpiresAt { get; set; }
        //public DateTime? RefreshTokenRevokedAt { get; set; }
    }
  
}