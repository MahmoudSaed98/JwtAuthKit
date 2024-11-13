using Application.Interfaces;

namespace Application.Services;

internal sealed class EmailContentGenerator : IEmailContentGenerator
{
    public string GenerateVerificationEmailContent(string username, string verificationLink, DateTime tokenValidity)
    {
        var template = @"
            <html>
                <body>
                    <h1>Hello, {{Username}}!</h1>
                    <p>
                        Thank you for registering. To verify your email address, 
                        please <a href='{{VerificationLink}}'>click here</a>.
                    </p>
                    <p>
                        This verification link is valid for {{TokenValidity}}.
                    </p>
                    <p>
                        If you did not request this, please ignore this email.
                    </p>
                    <p>
                        Best regards,<br>
                    </p>
                </body>
            </html>";

        return template
            .Replace("{{Username}}", username)
            .Replace("{{VerificationLink}}", verificationLink)
            .Replace("{{TokenValidity}}", tokenValidity.ToString());
    }
}
