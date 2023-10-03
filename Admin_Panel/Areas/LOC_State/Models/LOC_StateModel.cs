using System.ComponentModel.DataAnnotations;

namespace Admin_Panel.Areas.LOC_State.Models

{
    public class LOC_StateModel
    {
        [Required]
        public int? StateID { get; set; }

        [Required]
        public int? CountryID { get; set; }
        [Required]
        public string? StateName { get; set; }
        [Required]
        public string? StateCode{ get; set;}
        [Required]
        public string? CountryName { get; set;}
    }

    public class LOC_StateDropDownModel
    {
        public int? StateID { get; set; }

        public string? StateName { get; set; }
    }

    public class LOC_StateFilterModel
    {
        public string? StateName { get; set; }
        public string? StateCode { get; set; }
        public int? CountryID { get; set; }
    }
}
