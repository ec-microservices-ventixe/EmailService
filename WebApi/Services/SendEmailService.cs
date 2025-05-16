using Azure.Communication.Email;
using System.Diagnostics;
using System.Net.Mail;
using WebApi.Interfaces;

namespace WebApi.Services;

public class SendEmailService(EmailClient client, IConfiguration configuration) : ISendEmailService
{
    private readonly EmailClient _client = client;
    private readonly IConfiguration _configuration = configuration;

    public bool SendEmail(string subject, string plainText, string htmlContent, List<string> emails)
    {
        try
        {
            List<EmailAddress> emailsList = [];
            foreach (string email in emails)
                emailsList.Add(new EmailAddress(email));

            var emailMessage = new EmailMessage(
                senderAddress: _configuration["ACS:SenderAddress"],
                content: new EmailContent(subject)
                {
                    PlainText = plainText,
                    Html = htmlContent
                },
                recipients: new EmailRecipients(emailsList)
                );

            EmailSendOperation emailSendOperation = _client.Send(Azure.WaitUntil.Completed, emailMessage);
            return true;

        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return false;
        }
    }
}
