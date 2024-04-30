using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using Fleck;
using lib;
using Service1.Service;

namespace ws;


public class saveUser : BaseEventHandler<OpdaterDeltagerDto>
{
    public readonly DataService _dataService;
    
    public saveUser(DataService dataService)
    {
        _dataService = dataService;
    }
    
    
    public override Task Handle(OpdaterDeltagerDto dto, IWebSocketConnection socket)
    {
        
        
        
        return Task.CompletedTask;
    }
    
    }

public class OpdaterDeltagerDto : BaseDto
{
    
    public string tidligereNavn { get; set; }
    public string nytNavn { get; set; }
}










