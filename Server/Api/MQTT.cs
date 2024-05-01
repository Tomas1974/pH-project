   using System.Text;
   using System.Text.Json;
   using api;
   using Dapper;
   using lib;
   using MQTTnet;
        using MQTTnet.Client;
        using MQTTnet.Client.Options;
   using Npgsql;
   using Service1.Service;

   namespace Websocket;

public class MQTT
{
    public readonly NpgsqlDataSource _DataSource;
    public static IMqttClient? mqttClient;
    public static IMqttClientOptions? options;

    public MQTT(NpgsqlDataSource dataSource)
    {
        _DataSource = dataSource;
    }
    
    public async void Startup()
    {
        await connectToBroker();
        await getConnectionToesb_temp();
        await sendMessageToFrontend();
        await closeConnection();
    }


    /**********  Forbindels til broker  **********/
    public async Task connectToBroker()
    {


        string server = "mqtt.flespi.io";
        int port = 1883;

        string user = new(Environment.GetEnvironmentVariable("mqtt_user"));
        string password = "";

       options = new MqttClientOptionsBuilder()
            .WithTcpServer(server, port)
            .WithCredentials(user, password)
            .WithClientId(Guid.NewGuid().ToString())
            .WithCleanSession()
            .Build();

        var factory = new MqttFactory();
        mqttClient = factory.CreateMqttClient();

    }

/**********  Her laves et abonnement på info fra esb/pH  **********/
    public async Task getConnectionToesb_temp()
    {
       
        mqttClient.UseConnectedHandler(async e =>
        {
            await mqttClient.SubscribeAsync(new MqttTopicFilterBuilder().WithTopic("client/#").Build());
            Console.WriteLine("Subscribed to esp/temp topic.");
            //Her tilføjes adgang til esp/pH

        });

    }
    
    /**********  Her sendes temperaturne til frontend og skrives i konsolen **********/
    public async Task sendMessageToFrontend()
    {

        mqttClient.UseApplicationMessageReceivedHandler(e =>
        {
            
            var msg = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
            Console.WriteLine($"Received message: {msg}");
            var data = decimal.Parse(msg);
            var client = e.ApplicationMessage.Topic;
            var today = DateTime.Today.Date;

            var sql = "INSERT INTO ph.data(client_id, data, date) VALUES(@client, @data, @date);";
            using (var conn = _DataSource.OpenConnection())
            {
                conn.Execute(sql, new {client = client, data = data, date = today});
            }


            var resp = new ServerSendsIOTDataToClients()
            {
                data = msg
            };
            CurrentConnections.Connections.ForEach(e => { e.Send(JsonSerializer.Serialize(resp)); });


        });

    }
    
    public async Task closeConnection()
    {


    /**********  Her er er disconnection til broker   **********/
        
        mqttClient.UseDisconnectedHandler(async e =>
        {
            Console.WriteLine("Disconnected from MQTT broker.");
            await Task.Delay(TimeSpan.FromSeconds(5));
            //Her afbrydes forbindelsen til broker
    
    
            try
            {
                await mqttClient.ConnectAsync(options, CancellationToken.None);
            }
            catch
            {
                Console.WriteLine("Reconnection failed.");
            }
        });

        await mqttClient.ConnectAsync(options, CancellationToken.None);
        
        Console.WriteLine("Press any key to exit.");
        Console.ReadKey();
        
        await mqttClient.DisconnectAsync();

//Her afbrydes forbindelsen
    }
   
}

   public class ServerSendsIOTDataToClients : BaseDto
   {
       public string data { get; set; }
   }