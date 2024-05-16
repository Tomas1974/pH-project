
using System.Text.Json;
using a;
using DefaultNamespace;
using Fleck;
using lib;


namespace ws;

public class GetServerStatus : BaseEventHandler<GetServerStatusDto>
{
    public readonly ServerStatusService _serverStatusService;

    public GetServerStatus(ServerStatusService serverStatusService)
    {
        _serverStatusService = serverStatusService;
    }


    public override Task Handle(GetServerStatusDto dto, IWebSocketConnection ws)
    {
        if (dto.eventType.Contains("GetServerStatus"))
        {
            Console.WriteLine("hey vi komer til GETSERVERSTATUS");
            var latestStatus = _serverStatusService.GetLatestEntry();
            
            var messageToClient = JsonSerializer.Serialize(latestStatus);
            ws.Send(messageToClient);
        }
             if (dto.eventType2.Contains("createEntry"))
           {
               Console.WriteLine("vi kom her til NEW ENTRY");
               _serverStatusService.CreateEntry("Server is live!", DateTime.Now);
           }

        return Task.CompletedTask;
    }
    
}

public class GetServerStatusDto : BaseDto
{
    public string eventType { get; set; }
    public string eventType2 { get; set; }
    

}

public class responseListOfStatus : BaseDto
{
    public IEnumerable<StatusModel> entries { get; set; }
}

