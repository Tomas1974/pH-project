using Fleck;
using lib;
using Service.Services;

namespace api.WS_request;

public class deleteClient : BaseEventHandler<deleteClientDto>
{
    public readonly ClientService _ClientService;

    public deleteClient(ClientService clientService)
    {
        _ClientService = clientService;
    }

    public override Task Handle(deleteClientDto dto, IWebSocketConnection socket)
    {
        _ClientService.deleteClient(dto.client_id);
        return Task.CompletedTask;
    }
}

public class deleteClientDto : BaseDto
    {
        public string client_id { get; set; }
    }