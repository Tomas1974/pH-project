using System.Reflection;
using api;
using api.HttpRequest;
using api.WS_request;
using Fleck;
using infrastructure;
using infrastructure.Repositories;
using lib;
using Service.Services;

public static class Startup
{

    public static void Main(string[] args)
    {
        startup(args);
        Console.ReadLine();
    }
    public static void startup(string[] args)
    {
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
        builder.Services.AddSingleton<ClientRepository>();
        builder.Services.AddSingleton<PostNrRespository>();
        builder.Services.AddSingleton<StatusRepository>();
        builder.Services.AddSingleton<PostNrService>();
        builder.Services.AddSingleton<ClientService>();
        builder.Services.AddSingleton<UserService>();
        builder.Services.AddSingleton<GetServerStatus>();
        builder.Services.AddSingleton<ServerStatusService>();
        builder.Services.AddSingleton<HttpClient>();
        builder.Services.AddSingleton<HttpClientService>();
        builder.Services.AddSingleton<DataService>();
        builder.Services.AddSingleton<DataRepository>();




        var clientEventHandlers = builder.FindAndInjectClientEventHandlers(Assembly.GetExecutingAssembly());

        var app = builder.Build();


        var server = new WebSocketServer("ws://0.0.0.0:8181");


        server.Start(ws =>
        {
            ws.OnOpen = () => { CurrentConnections.Connections.Add(ws); };
            ws.OnMessage = async message =>
            {
                Console.WriteLine(message);
                

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
    }

}

