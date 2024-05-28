using System.Text.Json;
using System.Text.Json.Serialization;

namespace api.HttpRequest;

public class AddressRootObject
{
    public List<Result> results { get; set; }
}

public class Result
{
    public string? country_code { get; set; }
    public string? formatted { get; set; }
    public string? postcode { get; set; }
    public string? city { get; set; }
    public string? street { get; set; }
    public string? housenumber { get; set; }
}


