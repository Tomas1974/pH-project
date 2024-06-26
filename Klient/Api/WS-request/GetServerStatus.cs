using System.Text.Json;
using Fleck;
using infrastructure.DataModels;
using lib;
using Service.Services;


namespace api.WS_request;

public class GetServerStatus : BaseEventHandler<GetServerStatusDto>
{
    public readonly ServerStatusService _serverStatusService;

    public GetServerStatus(ServerStatusService serverStatusService)
    {
        _serverStatusService = serverStatusService;
    }
    
    //Method for handling an incoming message.
    public override Task Handle(GetServerStatusDto dto, IWebSocketConnection ws)
    {
        if (dto.eventType.Contains("GetServerStatus"))
        {
            var latestStatus = _serverStatusService.GetLatestEntry();
        
            var messageToClient = JsonSerializer.Serialize(latestStatus);
            ws.Send(messageToClient);
        }

        if (dto.eventType2.Contains("createEntry"))
        {
            _serverStatusService.CreateEntry("Server is live!", DateTime.Now);
        }

        return Task.CompletedTask;
    }
}

    //Objects containing WS message.
    public class GetServerStatusDto : BaseDto
    {
    public string eventType { get; set; }
    public string eventType2 { get; set; }
    }
