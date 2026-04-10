namespace Library
{
    namespace Library
    {
        public enum Difficulty
        {
            Nemt,
            Mellem,
            Svært,
            EkstraSvært
        }

        public static class DifficultyExtensions
        {
            public static string GetName(this Difficulty difficulty)
            {
                return difficulty switch
                {
                    Difficulty.Nemt       => "Nemt",
                    Difficulty.Mellem     => "Mellem",
                    Difficulty.Svært      => "Svært",
                    Difficulty.EkstraSvært => "Ekstra Svært",
                    _                     => "Ukendt"
                };
            }
        }
    }
}