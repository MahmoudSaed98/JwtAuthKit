namespace Application.Common.Constants;

internal static class ErrorMessages
{
    // Refresh and Access Token Errors
    public const string AccessTokenMalformed = "Access token is malformed or not in the expected format.";
    public const string RefreshTokenInvalid = "The refresh token is invalid or has been tampered with.";
    public const string RefreshTokenExpired = "Refresh token has expired. Please log in again.";
    public const string RefreshTokenRevoked = "Refresh token has revoked.";
    public const string InvalidAccessToken = "Access token has expired. Please refresh your token";
    public const string RefreshTokenUserMismatch = "Refresh token does not match the user. Authentication failed.";
    public const string RefreshTokenNotFound = "Refresh token does not found.";
    // Login Errors
    public const string InvalidCredentials = "Invalid username or password.";
    public const string AccountLocked = "Your account has been locked due to multiple unsuccessful login attempts.";
    public const string AccountInactive = "Your account is inactive. Please contact support.";
    public const string EmailNotVerified = "Email not verified. Please verify your email to log in.";

    // Registration Errors
    public const string UsernameAlreadyExists = "Username is already taken. Please choose a different one.";
    public const string EmailAlreadyExists = "An account with this email already exists.";
    public const string WeakPassword = "Password does not meet the minimum security requirements.";
    public const string RegistrationFailed = "Registration failed. Please try again.";

    // Password Management Errors
    public const string PasswordMismatch = "Passwords do not match.";
    public const string PasswordResetFailed = "Password reset failed. Please try again.";

    // User Errors
    public const string RoleRequired = "User must have at least one role assigned.";
    public const string CannotDeleteAdmin = "Admin users cannot be deleted.";
}
