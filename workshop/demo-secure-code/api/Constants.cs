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
}