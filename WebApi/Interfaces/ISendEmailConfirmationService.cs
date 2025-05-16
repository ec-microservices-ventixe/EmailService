namespace WebApi.Interfaces;

public interface ISendEmailConfirmationLinkService
{
    public void SendConfirmationLink(string email, string token);
}
