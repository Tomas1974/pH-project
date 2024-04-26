using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using Fleck;
using lib;
using Service1.Service;

namespace ws;


public class SletDeltager : BaseEventHandler<SletDeltagerDto>
{
    public readonly DataService _dataService;
    
    public SletDeltager(DataService dataService)
    {
        _dataService = dataService;
    }
    
    
    public override Task Handle(SletDeltagerDto dto, IWebSocketConnection socket)
    {
        
        _dataService.fjernDeltager(dto.messageContent);
        
        
        return Task.CompletedTask;
    }
    
    }

public class SletDeltagerDto : BaseDto
{
    
    public string messageContent { get; set; }
}










