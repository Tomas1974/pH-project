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
    


    public PHModel getData(string client)
    {

        var dataListe = _dataRepository.FindData(client);
        List<SeriesData> Series = null;
        
        foreach (var data in dataListe)
        {
            SeriesData seriesData = new SeriesData
            {
                Name = data.time.ToString(),
                Value = data.data

            };
            
            Series.Add(seriesData);
            
        }

        PHModel phModel = new PHModel
        {
            Name = client,
            Series = Series


        };
        return phModel;
    }
        
    
}