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
                    _ => "Ukendt"
                };
            }

            public static Difficulty NextDifficulty(this Difficulty difficulty)
            {
                return difficulty switch
                {
                    Difficulty.Nemt => Difficulty.Mellem,
                    Difficulty.Mellem => Difficulty.Svært,
                    Difficulty.Svært => Difficulty.EkstraSvært,
                    _ => Difficulty.Nemt
                };
            }
            
            public static Difficulty PreviousDifficulty(this Difficulty difficulty)
            {
                return difficulty switch
                {
                    Difficulty.Nemt => Difficulty.EkstraSvært,
                    Difficulty.Mellem => Difficulty.Nemt,
                    Difficulty.Svært => Difficulty.Mellem,
                    _ => Difficulty.Svært
                };
            }
        }
    }
}