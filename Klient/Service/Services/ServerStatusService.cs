using DefaultNamespace;
using infrastructure.Repositories;

namespace a;

public class ServerStatusService
{

    private StatusRepository _statusRepository;

    public ServerStatusService(StatusRepository statusRepository)
    {
        _statusRepository = statusRepository;
    }
    
    public StatusModel GetLatestEntry()
    {
        Console.WriteLine("og jeg kom os her til");
        return _statusRepository.GetLatestEntry();
    }
    
    
    public StatusModel CreateEntry(string log, DateTime date)
    {
        
        var entry = _statusRepository.CreateStatusEntry(log, date);
        return entry;
    }



}