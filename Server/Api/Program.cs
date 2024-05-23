using System.Reflection;
using api;
using Fleck;
using lib;
using MailKit;
using Npgsql;
using Service.Service;
using Websocket;
using Websocket_fødselsdag_2.Email;


var builder = WebApplication.CreateBuilder(args);

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddNpgsqlDataSource(Utilities.ProperlyFormattedConnectionString,
        dataSourceBuilder => dataSourceBuilder.EnableParameterLogging());
}

if (builder.Environment.IsProduction())
{
    builder.Services.AddNpgsqlDataSource(Utilities.ProperlyFormattedConnectionString);
}




builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddTransient<IEmailService, EmailService>();
builder.Services.AddSingleton<DataService>();


builder.Services.AddSingleton<MQTT>();





var clientEventHandlers = builder.FindAndInjectClientEventHandlers(Assembly.GetExecutingAssembly());

var app = builder.Build();
app.Services.GetService<MQTT>().Startup();
app.Services.GetService<IEmailService>();


var server = new WebSocketServer("ws://0.0.0.0:9191");


server.Start(ws =>
{
    ws.OnOpen = () =>
    {


    
        CurrentConnections.Connections.Add(ws);
    };
    ws.OnMessage = async message =>
    {
      
        try
        {
           await app.InvokeClientEventHandler(clientEventHandlers, ws, message);

        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    };
});




Console.ReadLine();



