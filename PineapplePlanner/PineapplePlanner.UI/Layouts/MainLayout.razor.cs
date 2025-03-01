using MudBlazor;

namespace PineapplePlanner.UI.Layouts
{
    public partial class MainLayout
    {
        public bool IsDarkMode { get; private set; }
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
            IsDarkMode = newValue;
            StateHasChanged();
            return Task.CompletedTask;
        }

        public void SetDarkMode(bool value)
        {
            IsDarkMode = value;
            StateHasChanged();
        }
    }
}
