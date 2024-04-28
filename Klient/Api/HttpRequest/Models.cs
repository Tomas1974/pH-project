using System.Text.Json;
using System.Text.Json.Serialization;

namespace api;

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



  public class Owners
    {
      public string name { get; set; }
    }

    public class Productionunits
    {
      public int pno { get; set; }
      public bool  main { get; set; }
      public string name { get; set; }
      public string address { get; set; }
      public string zipcode { get; set; }
      public string city { get; set; }
      public string cityname { get; set; }
      public bool @protected { get; set; }
      public string phone { get; set; }
      public string email { get; set; }
      public string fax { get; set; }
      public string startdate { get; set; }
      public string enddate { get; set; }
      
      [JsonConverter(typeof(NumberToStringConverter))]
      public string Employees { get; set; } //Den bøvler. Enten var tallet ikke angivet, så forventes det at være int? eller også var det et interval 50-100 så var det en string. Nu er det konventeret til altid at være en streng.
      
      public string addressco { get; set; }
      public int industrycode { get; set; }
      public string industrydesc { get; set; }
      
    }

    public class ApiResult
    {
      public int vat { get; set; }
      public string name { get; set; }
      public string address { get; set; }
      public string zipcode { get; set; }
      public string city { get; set; }
      public bool @protected { get; set; }
      public string phone { get; set; }
      public string email { get; set; }
      public string fax { get; set; }
      public string startdate { get; set; }
      public string enddate { get; set; }
      
      [JsonConverter(typeof(NumberToStringConverter))]
      public string  employees { get; set; }
      
      public string addressco { get; set; }
      public int industrycode { get; set; }
      public string industrydesc { get; set; }
      public int companycode { get; set; }
      public string companydesc { get; set; }
      public string creditstartdate { get; set; }
      public bool creditbankrupt { get; set; }
      public int? creditstatus { get; set; }
      public List<Owners> owners { get; set; }
      public List<Productionunits> productionunits { get; set; }
      public int t { get; set; }
      public int version { get; set; }

    }


    public class NumberToStringConverter : JsonConverter<string>
    {
      public override string Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
      {
        if (reader.TokenType == JsonTokenType.Number)
        {
          return reader.GetInt32().ToString(); // Or use GetDouble(), GetDecimal(), etc., depending on the expected number type
        }
        else if (reader.TokenType == JsonTokenType.String)
        {
          return reader.GetString();
        }
        throw new JsonException("Expected a number or string");
      }

      public override void Write(Utf8JsonWriter writer, string value, JsonSerializerOptions options)
      {
        writer.WriteStringValue(value);
      }
    }
    