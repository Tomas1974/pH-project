using infrastructure.DataModels;
using infrastructure.Repositories;

namespace Service.Services;

public class ClientService
{
    private ClientRepository _ClientRepository;

    public ClientService(ClientRepository clientRepository)
    {
        _ClientRepository = clientRepository;
    }

    public IEnumerable<ClientModel> GetClient(string user_id)
    {
        return _ClientRepository.GetClients(user_id);
    }
    
    public ClientModel CreateClient(ClientModel clientModel)
    {
        return _ClientRepository.CreateClient(clientModel);
    }
}

