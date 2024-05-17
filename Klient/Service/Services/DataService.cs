using System.ComponentModel.DataAnnotations;
using infrastructure;
using infrastructure.DataModels;
using service.Services;

namespace Service1;

public class DataService
{
    
    
    private readonly DataRepository _dataRepository;

    public DataService(DataRepository dataRepository)
    {
        _dataRepository = dataRepository;
    }
    


    public  List<SeriesData> getData(string client)
    {

        var dataListe = _dataRepository.FindData(client);
        List<SeriesData> Series  = new List<SeriesData>(); 
        
       
        
        foreach (var data in dataListe)
        {
            
   
            
            SeriesData seriesData = new SeriesData
            {
                name = data.time.ToString(),
                value = data.data

            };
            
            
            Series.Add(seriesData);
           
        }

               return Series;
    }
        
    
}