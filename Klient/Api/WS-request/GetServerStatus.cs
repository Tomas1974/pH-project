
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
        Console.WriteLine("hey jeg kom her til");
        
      
            var latestStatus = _serverStatusService.GetLatestEntry();
            //var latestStatus = new StatusModel { Date = DateTime.Now, Log = "hej hej" };


           // var message = new responseListOfStatus()
            //{
              // entries = latestStatus;
            //};

            var messageToClient = JsonSerializer.Serialize(latestStatus);
            ws.Send(messageToClient);
            
        return Task.CompletedTask;
    }




}

public class GetServerStatusDto : BaseDto
{
    public string status { get; set; }

}

public class responseListOfStatus : BaseDto
{
    public IEnumerable<StatusModel> entries { get; set; }
}

