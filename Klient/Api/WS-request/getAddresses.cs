using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using Fleck;
using lib;
using Service1.Service;

namespace ws;


public class getAddresses : BaseEventHandler<NyDeltagerDto>
{
    public readonly DataService _dataService;
    
    public getAddresses(DataService dataService)
    {
        _dataService = dataService;
    }
    
    
    public override Task Handle(NyDeltagerDto dto, IWebSocketConnection socket)
    {
        
      //  _dataService.tilFøjDeltager(dto.messageContent);
        
        
        return Task.CompletedTask;
    }
    
    }

public class NyDeltagerDto : BaseDto
{
    
    public string messageContent { get; set; }
}










