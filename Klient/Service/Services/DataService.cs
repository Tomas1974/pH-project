using System.ComponentModel.DataAnnotations;
using infrastructure;
using infrastructure.DataModels;
using service.Services;

namespace Service1;

public class DataService
{
    public string loginEmail = "";
    
    
    private readonly DataRepository _dataRepository;

    public DataService(DataRepository dataRepository)
    {
        _dataRepository = dataRepository;
    }
    


    public string getData(string client)
    {

        var dataListe = _dataRepository.FindData(client);

        foreach (var data in dataListe)
        {
            
        }
        

        // PHModel phModel= new PHModel
        // {
        //     Name = client,
        //     Series = 
        //     
        //     
        // }
        return "Hej";
    }
        
    
}