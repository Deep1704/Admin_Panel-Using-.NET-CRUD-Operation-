using System.ComponentModel.DataAnnotations;

namespace Admin_Panel.Areas.LOC_Country.Models
{
    public class LOC_CountryModel
    {
        [Required]
        public int? CountryID { get; set; }

        [Required]
        public string? CountryName { get; set; }

        [Required]
        public string? CountryCode { get; set; }

    }

    public class LOC_CountryDropDownModel
    {
        public int? CountryID { get; set; }

        public string? CountryName { get; set; }
    }
}
