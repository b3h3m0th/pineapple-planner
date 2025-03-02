using Microsoft.AspNetCore.Components;
using PineapplePlanner.UI.Layouts;

namespace PineapplePlanner.UI.Pages
{
    public partial class Settings
    {
        [CascadingParameter(Name = "MainLayout")]
        public MainLayout? MainLayout { get; set; }

        private bool _isDarkMode;

        public bool IsDarkMode
        {
            get
            {
                return _isDarkMode;
            }
            set
            {
                _isDarkMode = value;
                MainLayout?.SetDarkMode(value);
            }
        }

        protected override void OnInitialized()
        {
            _isDarkMode = MainLayout?.IsDarkMode ?? false;

            base.OnInitialized();
        }
    }
}
