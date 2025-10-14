using System.Globalization;
using System.Reflection;
using CsvHelper;


namespace PTANonCrown.Data.Services
{
    public static class CsvLoader
    {
        public static List<T> LoadCsv<T>(string fileName)
        {
            // BaseDirectory points to the runtime folder of the executing app
            var baseDir = AppContext.BaseDirectory;

            // CSVs are in a "Resources" folder next to the executable
            var fullPath = Path.Combine(baseDir, "Resources", fileName);

            if (!File.Exists(fullPath))
                throw new FileNotFoundException($"CSV file not found: {fullPath}");

            using var reader = new StreamReader(fullPath);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            var records = new List<T>();

            while (csv.Read())
            {
                var record = csv.GetRecord<T>(); // This will throw if mapping fails
                records.Add(record);
            }
            return records;
        }
    }
}
