using api.TransferModel;
using infrastructure.DataModels;
using Microsoft.AspNetCore.Mvc;
using Service.Services;

namespace api.Controllers;

[ApiController]
public class ClientController : ControllerBase
{

    private ClientService _clientService;

    public ClientController(ClientService clientService)
    {
        _clientService = clientService;
    }
    
    public ResponseDto CreateClient([FromBody] ClientModel client)
    {
        return new ResponseDto()
        {
            MessageToClient = "Successfully created a new client entry in the database",
            ResponseData = _clientService.CreateClint(client)
        };
    }
}

