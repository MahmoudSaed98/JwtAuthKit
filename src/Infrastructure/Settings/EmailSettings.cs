namespace Infrastructure.Settings;

public sealed class EmailSettings
{
    public const string SectionName = "EmailSettings";
    public int Port { get; set; }
    public string SenderEmail { get; set; } = string.Empty;
    public string SenderName { get; set; } = string.Empty;
    public string SmtpServer { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
