using Fleck;
using infrastructure.DataModels;
using lib;
using Service.Services;

namespace ws;

public class saveClient : BaseEventHandler<saveClientDto>
{
    public readonly ClientService _ClientService;
    
    public saveClient(ClientService clientService)
    {
        _ClientService = clientService;
    }
    
    
    public override Task Handle(saveClientDto dto, IWebSocketConnection socket)
    {
        var message = new ClientModel
        {
            client_id = dto.client_id,
            client_name = dto.client_name,
            max_value = dto.max_value,
            min_value = dto.min_value
        };

        _ClientService.CreateClint(message);
        
        return Task.CompletedTask;
    }
}

public class saveClientDto : BaseDto
{
    public string client_id { get; set; }  
    public string client_name { get; set; }
    public decimal max_value { get; set; }            
    public decimal min_value { get; set; }   
}