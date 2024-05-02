namespace infrastructure.DataModels;

public class ClientModel
{
    public string client_id { get; set; }  
    public string client_name { get; set; }
    public decimal max_value { get; set; }            
    public decimal min_value { get; set; }   
}