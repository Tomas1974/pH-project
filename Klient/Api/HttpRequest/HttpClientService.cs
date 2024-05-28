using System.Text.Json;

namespace api.HttpRequest;

public class HttpClientService(HttpClient httpClient)
{
    public async Task<AddressRootObject> GetAddressSuggestion(string addressSearchTerm)
    {
        var addressLookupUrl = "https://api.geoapify.com/v1/geocode/autocomplete" +
                               "?text=" + addressSearchTerm + "" +
                               "&filter=countrycode:dk&format=json&apiKey=" +
                               Environment.GetEnvironmentVariable("GEOCODEAPIKEY");

        var response = await httpClient.GetAsync(addressLookupUrl);

        //  var responseContent = await response.Content.ReadAsStringAsync();
        //  Console.WriteLine(responseContent);

        return JsonSerializer.Deserialize<AddressRootObject>(await response.Content.ReadAsStringAsync()) ??
               throw new InvalidOperationException();
    }


}