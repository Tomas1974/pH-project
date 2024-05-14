﻿using infrastructure.DataModels;
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
    
    public ClientModel CreateClient(ClientModel clientModel, string email)
    { 
        if (_ClientRepository.ClientAlreadyExist(clientModel.client_id))
        {
          clientModel.duplicate = true;
            return clientModel;
        } 
        clientModel.duplicate = false;
        return _ClientRepository.CreateClient(clientModel, email);
    }
}

