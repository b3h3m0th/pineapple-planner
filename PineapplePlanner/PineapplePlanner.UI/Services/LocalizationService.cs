using PineapplePlanner.UI.Localization;
using System.Text.Json;

namespace PineapplePlanner.UI.Services
{
    public class LocalizationService
    {
<<<<<<< HEAD
        public readonly Dictionary<string, Dictionary<string, string>> _translations = new();
        private string[] _supportedCultures = [Culture.English, Culture.Swedish, Culture.German];
=======
        public readonly Dictionary<string, Dictionary<string, string>> _translations = [];
<<<<<<< HEAD
<<<<<<< HEAD
        private readonly string[] _supportedCultures = [Culture.English, Culture.Swedish];
>>>>>>> 03eb249b61799026dff86e5ee3066aefb6307708
=======
        private readonly string[] _supportedCultures = [Culture.English, Culture.Swedish, Culture.German, Culture.Spanish, Culture.Japanese, Culture.Estonian, Culture.Ukrainian, Culture.Polish, Culture.French];
>>>>>>> 45d3c6db2c3f406f19f29158a0981cb864239046
=======
        private readonly string[] _supportedCultures = [Culture.English, Culture.Swedish, Culture.German, Culture.Spanish, Culture.Japanese, Culture.Estonian, Culture.Ukrainian, Culture.Polish, Culture.French, Culture.Lithuanian];
>>>>>>> 511ba7e59083c5343a376c37de3a3eb4e9c3c423

        public string[] SupportedCultures { get => _supportedCultures; }
        public string CurrentCulture { get; private set; } = Culture.English;

        public LocalizationService()
        {
            string[] files = Directory.GetFiles(Path.Combine(Directory.GetCurrentDirectory(), "Localization", "Resources"), "*.json");
            foreach (string file in files)
            {
                string[] filenameParts = Path.GetFileName(file).Split(".");
<<<<<<< HEAD
                string culture = filenameParts[filenameParts.Length - 2];
=======
                string culture = filenameParts[^2];
>>>>>>> 03eb249b61799026dff86e5ee3066aefb6307708
                string json = File.ReadAllText(file);
                Dictionary<string, string>? deserialized = JsonSerializer.Deserialize<Dictionary<string, string>>(json, new JsonSerializerOptions()
                {
                    AllowTrailingCommas = true,
                });

                if (deserialized != null)
                {
                    _translations[culture] = deserialized;
                }
            }
        }

        public void SetCulture(string name)
        {
            CurrentCulture = name;
        }

        public string this[string key]
        {
            get => GetString(key);
        }

        public string GetString(string? key, string? culture = null)
        {
            culture ??= CurrentCulture;

            if (key == null) return string.Empty;

            return _translations.GetValueOrDefault(culture)?.GetValueOrDefault(key) ?? key;
        }
    }
}
