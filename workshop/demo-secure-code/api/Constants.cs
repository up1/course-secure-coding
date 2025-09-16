public static class Constants
{
    public static class Jwt
    {
        // Bad code: hardcoded secret key
        // In production, use a secure method to store and retrieve secrets
        // e.g., environment variables, Azure Key Vault, AWS Secrets Manager, etc.
        public const string Key = "ThisIsADevSecretKeyForJWTGeneration123!";
        public const string Issuer = "MyApiIssuer";
        public const string Audience = "MyApiAudience";
    }

    // Get secret key from environment variable
    // public static string Key => Environment.GetEnvironmentVariable("JWT_SECRET_KEY") ?? throw new InvalidOperationException("JWT secret key not set in environment variables.");
    
}