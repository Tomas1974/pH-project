   using System.Text;
   using Dapper;
   using lib;
   using MQTTnet;
   using MQTTnet.Client;
   using MQTTnet.Client.Options;
   using Npgsql;
   using Websocket_fødselsdag_2.Email;

   namespace Websocket;

   public class MQTT
   {
       public readonly NpgsqlDataSource _DataSource;
       public static IMqttClient? mqttClient;
       public static IMqttClientOptions? options;
       
       private readonly IEmailService _iEmailService;

       public MQTT(NpgsqlDataSource dataSource, IEmailService service)
       {
           _iEmailService = service;
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

       /**********  Her sendes pH til frontend og skrives i konsolen **********/
       public async Task sendMessageToFrontend()
       {

           mqttClient.UseApplicationMessageReceivedHandler(async e =>
           {
               var msg = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
               var data = decimal.Parse(msg);
               var client = e.ApplicationMessage.Topic;
               var now = DateTime.Now;



               if (await GetLower(client) != -1 && await GetHigher(client) != -1) //Hvis der ikke er grænser, så skal vi ikke checke dem.
                   sendMessageToEmail(data, client);
                     
               
               var sql = "INSERT INTO ph.data(client_id, data, time) VALUES(@Client, @Data, @Time);";
               using (var conn = await _DataSource.OpenConnectionAsync())
               { 
                   await conn.ExecuteAsync(sql, new { Client = client, Data = data, Time = now });
                   
               }
           });

       }

       private async Task sendMessageToEmail(decimal data, string client)
       {
           
               var minPH = await GetLower(client);
               var maxPH = await GetHigher(client);
                   
               if (data > maxPH || data < minPH )
               {
                   
                   try
                   {

                       var mailRequest = new MailRequest
                       {
                           ToEmail = await GetEmailFromClient(client),
                           Subject = "You're PH not inside your limits for pH!",
                           Body = "You're PH level on " + client + "is at " + data
                       };
                       
                       await _iEmailService.SendEmailAsync(mailRequest);
                       
                   }
                   catch (Exception exception)
                   {
                       Console.WriteLine(exception);
                       throw;
                   }

               }
          
       }


       public async Task<decimal> GetHigher(string client_id)
       {
           var sql = @"SELECT max_value FROM ph.client WHERE client_id = @client_id;";

           await using (var conn = await _DataSource.OpenConnectionAsync())
           {
               
               {
                   var result = await conn.QueryFirstOrDefaultAsync<decimal?>(sql, new { client_id });
                   return result ?? -1; 
               }
           }
       }

       
       public async Task<decimal> GetLower(string client_id)
       {
           var sql = @"SELECT min_value FROM ph.client WHERE client_id = @client_id;";

           await using (var conn = await _DataSource.OpenConnectionAsync())
           {
               var result = await conn.QueryFirstOrDefaultAsync<decimal?>(sql, new { client_id });
               return result ?? -1; 
           }
       }
       
       

       public async Task<string> GetEmailFromClient(string client_id)
       {
           var sql = $@"SELECT email FROM ph.client_user WHERE client_id = @client_id;";

          await using (var conn = await _DataSource.OpenConnectionAsync())
           {
               Console.WriteLine(client_id + "Det her er datakald");
               return await conn.QueryFirstAsync<string>(sql, new {client_id});
           }
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


    }
   
}

   