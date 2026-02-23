public class GridCell
{
    public int x;
    public int z;
    public bool isOccupied;
    public string buildingId;

    public GridCell(int x, int z)
    {
        this.x = x;
        this.z = z;
        this.isOccupied = false;
        this.buildingId = "none";
    }
}
