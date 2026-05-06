namespace Utils
{
    public class NumberFormat
    {
        public static string Format(float num)
        {
            if (num >= 1_000_000_000f)
                return (num / 1_000_000_000f).ToString("0.0") + "B";
            if (num >= 1_000_000f)
                return (num / 1_000_000f).ToString("0.0") + "M";
            if (num >= 1_000f)
                return (num / 1_000f).ToString("0.0") + "K";

            return num.ToString("0.0");
        }
    }
}