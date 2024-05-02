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

    public ClientModel CreateClint(ClientModel clientModel)
    {
        return _ClientRepository.CreateClient(clientModel);
    }
}

