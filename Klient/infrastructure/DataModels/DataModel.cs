namespace infrastructure.DataModels;

public class PHModel
{
    public string Name { get; set; }
    public List<SeriesData> Series { get; set; }
}

public class SeriesData
{
    public string name { get; set; }
    public double value { get; set; }
}

public class DataModel
{
    public double data { get; set; }
    public DateTime time { get; set; }
}