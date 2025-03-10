public class User
{
    public int Id { get; set; }
    public required string Username { get; set; }
    public required string PasswordHash { get; set; } // Store hashed password


    // More properties
    public string? Email { get; set; }
    public string? Role { get; set; } // "User", "Admin"
    public bool IsAdmin { get; set; } // Sensitive property
}