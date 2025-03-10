using PineapplePlanner.UI.Localization;
using System.Text.Json;

namespace PineapplePlanner.UI.Services
{
    public class LocalizationService
    {
        public readonly Dictionary<string, Dictionary<string, string>> _translations = new();
        private string[] _supportedCultures = [Culture.English, Culture.Swedish];

        public string[] SupportedCultures { get => _supportedCultures; }
        public string CurrentCulture { get; private set; } = Culture.English;

        public LocalizationService()
        {
            string[] files = Directory.GetFiles(Path.Combine(Directory.GetCurrentDirectory(), "Localization", "Resources"), "*.json");
            foreach (string file in files)
            {
                string[] filenameParts = Path.GetFileName(file).Split(".");
                string culture = filenameParts[filenameParts.Length - 2];
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

        public string GetString(string key, string? culture = null)
        {
            culture ??= CurrentCulture;

            return _translations.GetValueOrDefault(culture)?.GetValueOrDefault(key) ?? key;
        }
    }
}
