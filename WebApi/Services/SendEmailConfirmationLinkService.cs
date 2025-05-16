using WebApi.Interfaces;

namespace WebApi.Services
{
    public class SendEmailConfirmationLinkService(ISendEmailService sendEmailService) : ISendEmailConfirmationLinkService
    {
        private readonly ISendEmailService _sendEmailService = sendEmailService;
        public void SendConfirmationLink(string email, string token)
        {
            string textContent = $"Please copy this link https://yourwebsite.com/confirm-email?email={email}&token={token} into your browser to confirm your email";
            string htmlContent = GetHtmlContent(email, token);
            _sendEmailService.SendEmail("Confirm Email", textContent, htmlContent, new List<string> { email });
        }
        private string GetHtmlContent(string email, string token)
        {
            return $@"
                <!DOCTYPE html>
                <html>
                <head>
                  <meta charset=""UTF-8"">
                  <title>Email Confirmation</title>
                </head>
                <body style=""margin:0; padding:0; font-family: Arial, sans-serif; background-color:#f4f4f4;"">
                  <table align=""center"" border=""0"" cellpadding=""0"" cellspacing=""0"" width=""100%"" style=""max-width:600px; background-color:#ffffff; margin-top:30px; border-radius:8px; box-shadow:0 0 10px rgba(0,0,0,0.1);"">
                    <tr>
                      <td align=""center"" style=""padding: 40px 30px 20px 30px;"">
                        <h2 style=""color:#333333;"">Confirm Your Email</h2>
                        <p style=""color:#666666;"">Click the button below to confirm your email</p>
                      </td>
                    </tr>
                    <tr>
                      <td align=""center"" style=""padding: 20px;"">
                        <a href=""https://yourwebsite.com/confirm-email?email={email}&token={token}"" 
                           style=""background-color:#f26cf9; color:#eeefff; padding:12px 24px; text-decoration:none; border-radius:5px; display:inline-block; font-weight:bold;"">
                          Confirm Email
                        </a>
                      </td>
                    </tr>
                    <tr>
                      <td align=""center"" style=""padding: 20px 30px 40px 30px; font-size:12px; color:#aaaaaa;"">
                        If the button doesn’t work, copy and paste the following link into your browser:<br>
                        <a href=""https://yourwebsite.com/confirm-email?email={email}&token={token}"" style=""color:#007bff;"">
                          https://yourwebsite.com/confirm-email?email={email}&token={token}
                        </a>
                      </td>
                    </tr>
                  </table>
                </body>
                </html>";
        }
    }
}

