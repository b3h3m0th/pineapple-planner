using MudBlazor;

namespace PineapplePlanner.UI.Layouts
{
    public partial class MainLayout
    {
        private bool _isDarkMode;
        private MudThemeProvider _mudThemeProvider;
        private readonly MudTheme _mudBlazorTheme = new()
        {
            PaletteLight = new PaletteLight()
            {
                Primary = Colors.LightGreen.Default,
            },
        };

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await _mudThemeProvider.WatchSystemPreference(OnSystemPreferenceChanged);
                StateHasChanged();
            }
        }

        private Task OnSystemPreferenceChanged(bool newValue)
        {
            _isDarkMode = newValue;
            StateHasChanged();
            return Task.CompletedTask;
        }

        public void SetDarkMode(bool value)
        {
            _isDarkMode = value;
            StateHasChanged();
        }
    }
}
