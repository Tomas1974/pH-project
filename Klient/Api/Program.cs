using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Xml;
using api;
using Fleck;
using infrastructure;
using lib;
using Service1;

using Websocket;


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

builder.Services.AddHttpClient();

builder.Services.AddSingleton<UserRepository>();
builder.Services.AddSingleton<UserService>();
builder.Services.AddSingleton<HttpClient>();
builder.Services.AddSingleton<HttpClientService>();


var clientEventHandlers = builder.FindAndInjectClientEventHandlers(Assembly.GetExecutingAssembly());

var app = builder.Build();


var server = new WebSocketServer("ws://0.0.0.0:8181");


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



