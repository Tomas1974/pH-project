using System.Text.Json;
using Fleck;
using infrastructure.DataModels;
using lib;
using Service.Services;

namespace ws;

public class getClient : BaseEventHandler<getClientDto>
{
    private readonly ClientService _clientService;

    public getClient(ClientService clientService)
    {
        _clientService = clientService;
    }
    
    public override Task Handle(getClientDto dto, IWebSocketConnection socket)
    {
        var userClients = _clientService.GetClient(dto.email);
        Console.WriteLine("Hej");
        
        foreach (ClientModel client in userClients)
        {
            Console.WriteLine(client.client_name);
            Console.WriteLine(client.min_value);
            Console.WriteLine(client.max_value);
            
            
        }
        

        var message = new responseListOfClients()
        {
            clients = userClients
        };
        
        var messageToClient = JsonSerializer.Serialize(message);
        socket.Send(messageToClient);
        return Task.CompletedTask;
    }
}

public class getClientDto : BaseDto
{
    public string email { get; set; }
}

public class responseListOfClients : BaseDto
{
    public IEnumerable<ClientModel> clients { get; set; }
}