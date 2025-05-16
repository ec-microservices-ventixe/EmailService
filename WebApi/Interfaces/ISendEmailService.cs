namespace WebApi.Interfaces;

public interface ISendEmailService
{
    public bool SendEmail(string subject, string plainText, string htmlContent, List<string> emails);
}
