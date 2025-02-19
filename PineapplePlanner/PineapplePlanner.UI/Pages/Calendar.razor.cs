using Microsoft.AspNetCore.Components;
using PineapplePlanner.UI.Layouts;
using System.Globalization;

namespace PineapplePlanner.UI.Pages
{
    public partial class Calendar
    {
        [CascadingParameter(Name = "AuthenticatedLayout")]
        public AuthenticatedLayout? AuthenticatedLayout { get; set; }

        [Parameter]
        public DateTime FirstDate { get; set; } = DateTime.Today;

        private string[] _dayNames = new CultureInfo("en").DateTimeFormat.AbbreviatedDayNames;

        private void HandleCreateTask()
        {
            AuthenticatedLayout?.OpenTaskDetail();
        }
    }
}
