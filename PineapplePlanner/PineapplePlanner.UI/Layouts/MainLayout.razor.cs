using MudBlazor;

namespace PineapplePlanner.UI.Layouts
{
    public partial class MainLayout
    {
        private readonly MudTheme _mudBlazorTheme = new()
        {
            PaletteLight = new PaletteLight()
            {
                Primary = Colors.LightGreen.Default,
            },
        };
    }
}
