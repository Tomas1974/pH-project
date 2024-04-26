using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Xml;
using Fleck;
using lib;
using Service1.Service;
using Websocket;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<DataService>();
builder.Services.AddSingleton<MQTT>();



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

new MQTT().Startup();

Console.ReadLine();



