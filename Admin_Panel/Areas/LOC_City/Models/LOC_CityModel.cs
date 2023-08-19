using System.ComponentModel.DataAnnotations;


namespace Admin_Panel.Areas.LOC_City.Models
{
    public class LOC_CityModel
    {
        [Required]
        public int? CityID { get; set; }
        [Required]
        public string CityName { get; set; }
        [Required]
        public string? CityCode { get; set; }
        [Required]
        public string? StateName { get; set; }
        [Required]
        public string? CountryName { get; set; }

        [Required]
        public int StateID { get;  set; }
        [Required]
        public int CountryID { get; set; }
    }
}
