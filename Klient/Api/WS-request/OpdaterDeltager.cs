using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using Fleck;
using lib;
using Service1.Service;

namespace ws;


public class OpdaterDeltager : BaseEventHandler<OpdaterDeltagerDto>
{
    public readonly DataService _dataService;
    
    public OpdaterDeltager(DataService dataService)
    {
        _dataService = dataService;
    }
    
    
    public override Task Handle(OpdaterDeltagerDto dto, IWebSocketConnection socket)
    {
        
        _dataService.opdaterDeltager(dto.tidligereNavn,dto.nytNavn);
        
        return Task.CompletedTask;
    }
    
    }

public class OpdaterDeltagerDto : BaseDto
{
    
    public string tidligereNavn { get; set; }
    public string nytNavn { get; set; }
}










