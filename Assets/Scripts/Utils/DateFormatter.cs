using System;

public static class DateFormatter
{
    public static string FormatLeagueDateRange(DateTime startDate, DateTime endDate)
    {
        if (startDate != default && endDate != default)
        {
            string startMonth = startDate.ToString("MMMM");
            string endMonth = endDate.ToString("MMMM");
            string year = endDate.Year.ToString();
            
            // Eğer aynı ay içindeyse: "Ocak 2024" gibi tek ay gösterilebilir 
            // veya kullanıcı isteği şu anki formatı korumaksa: "Ocak - Şubat 2024"
            // Mevcut mantığı koruyoruz:
            return $"{startMonth} - {endMonth} {year}";
        }

        return "TBD";
    }
}
