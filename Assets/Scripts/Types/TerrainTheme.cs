namespace Library
{
    public enum TerrainTheme
    {
        Grasslands,
        Desert,
        Jungle
    }

    public static class TerrainThemeExtensions
    {
        public static string GetName(this TerrainTheme theme)
        {
            return theme switch
            {
                TerrainTheme.Grasslands => "Grasslands",
                TerrainTheme.Desert => "Desert",
                _ => "Jungle"
            };
        }
        
        public static TerrainTheme NextTheme(this TerrainTheme theme)
        {
            return theme switch
            {
                TerrainTheme.Grasslands => TerrainTheme.Desert,
                TerrainTheme.Desert => TerrainTheme.Jungle,
                _ => TerrainTheme.Grasslands
            };
        }

        public static TerrainTheme PreviousTheme(this TerrainTheme theme)
        {
            return theme switch
            {
                TerrainTheme.Grasslands => TerrainTheme.Jungle,
                TerrainTheme.Desert => TerrainTheme.Grasslands,
                _ =>  TerrainTheme.Desert
            };
        }
    }
}