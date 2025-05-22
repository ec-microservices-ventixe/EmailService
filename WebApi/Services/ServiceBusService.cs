using Azure.Messaging.ServiceBus;
using System.Diagnostics;
using System.Text.Json;
using WebApi.Interfaces;
using WebApi.Models;

namespace WebApi.Services;

public class ServiceBusListener
{
    private readonly ServiceBusClient _client;
    private readonly ServiceBusProcessor _processor;
    private readonly ISendEmailConfirmationLinkService _sendEmailConfirmationLinkService;
    private readonly IConfiguration _configuration;

    public ServiceBusListener(IConfiguration configuration, ServiceBusClient client, ISendEmailConfirmationLinkService sendEmailConfirmationLinkService)
    {
        _client = client;
        _configuration = configuration; 
        _processor = _client.CreateProcessor(_configuration["ASB:QueueName"], new ServiceBusProcessorOptions());
        _processor.ProcessMessageAsync += OnMessageReceivedAsync;
        _processor.ProcessErrorAsync += ErrorHandlerAsync;
        _sendEmailConfirmationLinkService = sendEmailConfirmationLinkService;
    }
    public async Task StartAsync()
    {
        await _processor.StartProcessingAsync();
    }

    private async Task OnMessageReceivedAsync(ProcessMessageEventArgs args)
    {
        try
        {
            string body = args.Message.Body.ToString();
            var message = JsonSerializer.Deserialize<ValidateEmailMessage>(body);
            if (message is not null)
            {
                _sendEmailConfirmationLinkService.SendConfirmationLink(message.Email, message.Token);
            }
            await args.CompleteMessageAsync(args.Message);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
        }

    }
    private Task ErrorHandlerAsync(ProcessErrorEventArgs args)
    {
        Console.WriteLine(args.Exception.Message);
        return Task.CompletedTask;
    }
}