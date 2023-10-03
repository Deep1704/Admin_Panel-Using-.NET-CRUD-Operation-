using System.ComponentModel.DataAnnotations;

namespace Admin_Panel.Areas.MST_Branch.Models
{
    public class MST_BranchModel
    {
        [Required]
        public int? BranchID { get; set; }
        [Required]
        public string? BranchName { get; set; }
        [Required]
        public string? BranchCode { get; set;}
    }

    public class LOC_BranchFilterModel
    {
        public int? BranchID { get; set; }
        public string? BranchName { get; set; }
        public string? BranchCode { get; set; }
    }

    public class MST_BranchDropDownModel
    {
        public int BranchID { get; set; }
        public string BranchName { get; set; }
    }
}
