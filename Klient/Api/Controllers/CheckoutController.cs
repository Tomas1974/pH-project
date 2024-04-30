
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[ApiController]
public class CheckoutController(HttpClientService httpService) : ControllerBase
{


    [HttpGet]
    [Route("/api/address")]
    public async Task<AddressRootObject> AddressAutoComplete([FromQuery] string addressSearchTerm)
    {
        return await httpService.GetAddressSuggestion(addressSearchTerm);
    }

    
    [HttpGet]
    [Route("/api/CVR")]
    public async Task<ApiResult> CVR([FromQuery] string SearchTerm)
    {
        return await httpService.GetCvrToAddress(SearchTerm);
    }
    
}