using System.Collections.Generic;

public class Building
{
    public List<GridCell> fields;
    public string type;
    public Dictionary<string, object> properties;

    public Building(List<GridCell> fields, string type, Dictionary<string, object> properties)
    {
        this.fields = fields;
        this.type = type;
        this.properties = properties;
    }

    public Building(GridCell field, string type, Dictionary<string, object> properties)
    {
        this.fields = new List<GridCell> { field };
        this.type = type;
        this.properties = properties;
    }
}
