using System.Text.Json;
using Fleck;
using lib;
using Service.Services;

namespace api.WS_request.Home_page;

public class PostNr : BaseEventHandler<GetTownDto>
{
    public readonly PostNrService _PostNrService;

    public PostNr(PostNrService postNrService)
    {
        _PostNrService = postNrService;
    }


    public override Task Handle(GetTownDto dto, IWebSocketConnection socket)
    {
        var message = new SendTown()
        {
            town = _PostNrService.findTown(dto.postNr)
        };

        var messageToClient = JsonSerializer.Serialize(message);
        socket.Send(messageToClient);

        return Task.CompletedTask;
    }
}

public class GetTownDto : BaseDto
{
    public int postNr { get; set; }
}

public class SendTown : BaseDto
{
    public string town { get; set; }
}