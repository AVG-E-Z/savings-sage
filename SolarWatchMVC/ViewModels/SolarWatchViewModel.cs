using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace SolarWatchMVC.ViewModels
{
    public class SolarWatchViewModel
    {
        public string City { get; set; }
        public DateTime? Date { get; set; }
        public string? SunData { get; set; }
        public bool IsDisplayed { get; set; }
        public string? DataToDisplay { get; set; }
       public string? CityToDisplay { get; set; }
        public string? TypeOfSunData { get; set; }
        public bool IsSomethingWrong { get; set; }
    }
}