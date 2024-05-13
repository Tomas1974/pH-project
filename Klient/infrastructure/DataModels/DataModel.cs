﻿namespace infrastructure.DataModels;



public class PHModel
{
    public string Name { get; set; }
    public List<SeriesData> Series { get; set; }
}

public class SeriesData
{
    public string Name { get; set; }
    public double Value { get; set; }
}