using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using Admin_Panel.Areas.MST_Branch.Models;
using Admin_Panel.Areas.LOC_Country.Models;

namespace Admin_Panel.Areas.MST_Branch.Controllers
{

    [Area("MST_Branch")]
    [Route("MST_Branch/[Controller]/[action]")]
    public class MST_BranchController : Controller
    {
        #region Configuration

        public IConfiguration Configuration;
        public MST_BranchController(IConfiguration _configuration)
        {
            Configuration = _configuration;
        }

        #endregion

        public IActionResult Index()
        {
            return View();
        }

        #region Branch Select All

        public IActionResult MST_BranchList()
        {

            string connectionstr = this.Configuration.GetConnectionString("myconnectionstring");
            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(connectionstr);
            connection.Open();
            SqlCommand objCmd = connection.CreateCommand();
            objCmd.CommandType = CommandType.StoredProcedure;
            objCmd.CommandText = "PR_Branch_SelectAll";
            SqlDataReader objSDR = objCmd.ExecuteReader();
            dt.Load(objSDR);
            return View("MST_BranchList",dt);
            
         }

        #endregion

        #region Delete

        public IActionResult Delete(int? BranchID)
        {
            string connectionstr = this.Configuration.GetConnectionString("myconnectionstring");
            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(connectionstr);
            connection.Open();
            SqlCommand objCmd = connection.CreateCommand();
            objCmd.CommandType = CommandType.StoredProcedure;
            objCmd.CommandText = "PR_Branch_DeleteByPK";
            objCmd.Parameters.AddWithValue("@BranchID", BranchID);
            SqlDataReader objSDR = objCmd.ExecuteReader();
            dt.Load(objSDR);
            return RedirectToAction("MST_BranchList");
        }
        #endregion

        #region Add/Edit

        public IActionResult Save(MST_BranchModel modelMST_Branch)
        {
            string connectionstr = this.Configuration.GetConnectionString("myconnectionstring");
            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(connectionstr);
            connection.Open();
            SqlCommand objCmd = connection.CreateCommand();
            objCmd.CommandType = CommandType.StoredProcedure;

            if(modelMST_Branch.BranchID == null)
            {
                objCmd.CommandText = "PR_Branch_Insert";
            }
            else
            {
                objCmd.CommandText = "PR_Branch_UpdateByPK";
                objCmd.Parameters.AddWithValue("@BranchID",modelMST_Branch.BranchID);
            }

            objCmd.Parameters.AddWithValue("@BranchName", modelMST_Branch.BranchName);
            objCmd.Parameters.AddWithValue("@BranchCode", modelMST_Branch.BranchCode);
            objCmd.ExecuteNonQuery();
            connection.Close();
            return RedirectToAction("MST_BranchList");
        }
        #endregion

        #region Add

        public IActionResult MST_BranchAdd(int BranchID=0)
        {
            string connectionstr = this.Configuration.GetConnectionString("myconnectionstring");
            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(connectionstr);
            connection.Open();
            SqlCommand objCmd = connection.CreateCommand();
            objCmd.CommandType = CommandType.StoredProcedure;
            objCmd.CommandText = "PR_Branch_SelectByPK";
            objCmd.Parameters.AddWithValue("@BranchID", BranchID);
            SqlDataReader objSDR = objCmd.ExecuteReader();
            dt.Load(objSDR);
            MST_BranchModel modelMST_Branch = new MST_BranchModel();

            foreach(DataRow dr in dt.Rows)
            {
                modelMST_Branch.BranchID = Convert.ToInt32(dr["BranchID"]);
                modelMST_Branch.BranchName = dr["BranchName"].ToString();
                modelMST_Branch.BranchCode = dr["BranchCode"].ToString();
            }

            return View("MST_BranchAdd",modelMST_Branch);
        }
        #endregion


        #region Filter

        public IActionResult LOC_BranchFilter(LOC_BranchFilterModel filterModel)
        {
            string connectionStr = this.Configuration.GetConnectionString("myconnectionString");
            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(connectionStr);
            connection.Open();
            SqlCommand objCmd = connection.CreateCommand();
            objCmd.CommandType = CommandType.StoredProcedure;
            objCmd.CommandText = "PR_Branch_Filter";
            objCmd.Parameters.AddWithValue("@BranchName", filterModel.BranchName);
            objCmd.Parameters.AddWithValue("@BranchCode", filterModel.BranchCode);
            SqlDataReader objSDR = objCmd.ExecuteReader();
            dt.Load(objSDR);
            ModelState.Clear();
            return View("MST_BranchList", dt);
        }

        #endregion
    }
}
