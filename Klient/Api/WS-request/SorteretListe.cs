using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using Fleck;
using lib;
using Service1.Service;

namespace ws;


public class SorteretListe : BaseEventHandler<SorteretListeDto>
{
    public readonly DataService _dataService;
    
    public SorteretListe(DataService dataService)
    {
        _dataService = dataService;
    }
    
    
    public override Task Handle(SorteretListeDto dto, IWebSocketConnection socket)
    {
        
        _dataService.sorteretListe(dto.opdateretListe);
        
        
        return Task.CompletedTask;
    }
    
    }

public class SorteretListeDto : BaseDto
{
    
    public List<string> opdateretListe { get; set; }
}










