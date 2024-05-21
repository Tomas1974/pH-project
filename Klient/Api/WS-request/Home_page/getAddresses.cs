using System.Text.Json;
using api.HttpRequest;
using Fleck;
using lib;

namespace api.WS_request.Home_page;

public class getAddresses(HttpClientService httpService) : BaseEventHandler<getAddressesDto>
{
    public override async Task Handle(getAddressesDto dto, IWebSocketConnection socket)
    {
        var message = new sendAddresses()
        {
            results = await httpService.GetAddressSuggestion(dto.addressSearchTerm),
        };

        var messageToClient = JsonSerializer.Serialize(message);
        socket.Send(messageToClient);
    }
}

public class getAddressesDto : BaseDto
{
    public string addressSearchTerm { get; set; }
}

public class sendAddresses : BaseDto
{
    public AddressRootObject results { get; set; }
}