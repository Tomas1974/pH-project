using System.Reflection;
using api;
using Fleck;
using lib;
using Npgsql;
using Service1.Service;
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

var npgsqlConnection = builder.Services.BuildServiceProvider().GetRequiredService<NpgsqlDataSource>();

builder.Services.AddSingleton<DataService>();
builder.Services.AddSingleton<MQTT>(_ => new MQTT(npgsqlConnection));

var clientEventHandlers = builder.FindAndInjectClientEventHandlers(Assembly.GetExecutingAssembly());

var app = builder.Build();


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

new MQTT(npgsqlConnection).Startup();

Console.ReadLine();



