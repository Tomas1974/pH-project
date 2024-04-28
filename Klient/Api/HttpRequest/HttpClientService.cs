using System.Text.Json;

namespace api;

public class HttpClientService(HttpClient httpClient)
{
    public async Task<AddressRootObject> GetAddressSuggestion(string addressSearchTerm)
    {
        var addressLookupUrl = "https://api.geoapify.com/v1/geocode/autocomplete" +
                               "?text=" + addressSearchTerm + "" +
                               "&filter=countrycode:dk&format=json&apiKey=" + Environment.GetEnvironmentVariable("GEOCODEAPIKEY");
       
        var response = await httpClient.GetAsync(addressLookupUrl);
        
      //  var responseContent = await response.Content.ReadAsStringAsync();
      //  Console.WriteLine(responseContent);
        
        return JsonSerializer.Deserialize<AddressRootObject>(await response.Content.ReadAsStringAsync()) ??
               throw new InvalidOperationException();
    }
    
    
    public async Task<ApiResult> GetCvrToAddress(string SearchTerm)
    {
        Random rnd = new Random();
        int num = rnd.Next();
        
        httpClient.DefaultRequestHeaders.Add("User-Agent", "Testing "+ num); //Der er en grænse på 50 søgninger pr. dag. Jeg har en mistanke om, at der logges på denne linje. Derfor ændres den hver gang.
        
        var addressLookupUrl = "http://cvrapi.dk/api?search="+SearchTerm+"&country=dk";
        
        Console.WriteLine(addressLookupUrl);
       
        var response = await httpClient.GetAsync(addressLookupUrl);
        //var responseContent = await response.Content.ReadAsStringAsync();
        //Console.WriteLine(responseContent);
        
      return JsonSerializer.Deserialize<ApiResult>(await response.Content.ReadAsStringAsync()) ??
               throw new InvalidOperationException();
        
        
    }
    
    

   
}