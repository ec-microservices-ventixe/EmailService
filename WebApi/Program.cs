using Azure.Communication.Email;
using Azure.Messaging.ServiceBus;
using WebApi.Interfaces;
using WebApi.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddSingleton(x => new EmailClient(builder.Configuration["ACS:ConnectionString"]));
builder.Services.AddSingleton(new ServiceBusClient(builder.Configuration["ASB:ConnectionString"]));
builder.Services.AddSingleton<ServiceBusListener>();
builder.Services.AddSingleton<ISendEmailService, SendEmailService>();
builder.Services.AddSingleton<ISendEmailConfirmationLinkService, SendEmailConfirmationLinkService>();

var app = builder.Build();

var listener = app.Services.GetRequiredService<ServiceBusListener>();
await listener.StartAsync();

app.MapOpenApi();
app.UseHttpsRedirection();
app.UseCors(x => x.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

app.UseAuthorization();

app.MapControllers();

app.Run();
