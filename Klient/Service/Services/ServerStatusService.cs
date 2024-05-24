using infrastructure.DataModels;
using infrastructure.Repositories;

namespace Service.Services;

public class ServerStatusService
{
    private StatusRepository _statusRepository;

    public ServerStatusService(StatusRepository statusRepository)
    {
        _statusRepository = statusRepository;
    }

    //Fetches the latest entry in the ph.status table as a StatusModel object.
    public StatusModel GetLatestEntry()
    {
        return _statusRepository.GetLatestEntry();
    }

    //Returns a StatusModel to be entered in the ph.status table
    public StatusModel CreateEntry(string log, DateTime date)
    {
        var entry = _statusRepository.CreateStatusEntry(log, date);
        return entry;
    }
}