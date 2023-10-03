using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using Admin_Panel.Areas.LOC_Country.Models;

namespace Admin_Panel.Areas.LOC_Country.Controllers
{
    [Area("LOC_Country")]
    [Route("LOC_Country/[Controller]/[action]")]
    public class LOC_CountryController : Controller
    {
        public IConfiguration Configuration;

        public LOC_CountryController(IConfiguration _configuration)
        {
            Configuration = _configuration;
        }

        public IActionResult Index()
        {
            return View();
        }

        #region CountryList SelectAll
        public IActionResult LOC_CountryList()
        {


            string connectionstr = this.Configuration.GetConnectionString("myconnectionstring");

            SqlConnection connection = new SqlConnection(connectionstr);
            connection.Open();
            SqlCommand objCmd = connection.CreateCommand();
            objCmd.CommandType = CommandType.StoredProcedure;
            objCmd.CommandText = "PR_Country_SelectAll";
            DataTable dt = new DataTable();
            SqlDataReader objSDR = objCmd.ExecuteReader();
            dt.Load(objSDR);
            connection.Close();
            return View("LOC_CountryList", dt);

            #endregion

        }




        #region Insert
        public IActionResult Save(LOC_CountryModel modelLOC_Country,int CountryID = 0)
        {
            string connectionstr = this.Configuration.GetConnectionString("myconnectionstring");
            SqlConnection connection = new SqlConnection(connectionstr);
            connection.Open();
            SqlCommand objCmd = connection.CreateCommand();
            objCmd.CommandType = CommandType.StoredProcedure;


            if (CountryID == 0)
            {
                objCmd.CommandText = "PR_Country_Insert";
            }
            else
            {
                objCmd.CommandText = "PR_Country_UpdateByPK";
                objCmd.Parameters.AddWithValue("@CountryID", modelLOC_Country.CountryID);

            }

            objCmd.Parameters.AddWithValue("@CountryName", modelLOC_Country.CountryName);
            //objCmd.Parameters.Add("@CountryName",SqlDbType.VarChar).Value = modelLOC_Country.CountryName;
            objCmd.Parameters.AddWithValue("@CountryCode", modelLOC_Country.CountryCode);

            objCmd.ExecuteNonQuery();

            connection.Close();
            return RedirectToAction("LOC_CountryList");

            #endregion


        }

        #region Add
        public IActionResult LOC_CountryAdd(int CountryID=0)
        {
                string connectionstr = this.Configuration.GetConnectionString("myconnectionstring");
                SqlConnection connection = new SqlConnection(connectionstr);
                connection.Open();
                
                SqlCommand objCmd = connection.CreateCommand();
                objCmd.CommandType = CommandType.StoredProcedure;
                objCmd.CommandText = "PR_Country_SelectByPK";
                objCmd.Parameters.AddWithValue("@CountryID",CountryID);
                DataTable dt = new DataTable();
                SqlDataReader objSDR = objCmd.ExecuteReader();
                dt.Load(objSDR);
                LOC_CountryModel modelLOC_Country = new LOC_CountryModel();

                foreach (DataRow dr in dt.Rows)
                {
                   
                    modelLOC_Country.CountryID = Convert.ToInt32(dr["CountryID"]);
                    modelLOC_Country.CountryName = dr["CountryName"].ToString();
                    modelLOC_Country.CountryCode = dr["CountryCode"].ToString();
                }
                return View("LOC_CountryAdd", modelLOC_Country);
        }

        #endregion



        #region Delete
        public IActionResult Delete(int? CountryID) 
        {
            string connectionstr = this.Configuration.GetConnectionString("myconnectionstring");
            SqlConnection connection = new SqlConnection(connectionstr);
            connection.Open();

            SqlCommand objCmd = connection.CreateCommand();
            objCmd.CommandType = CommandType.StoredProcedure;
            objCmd.CommandText = "PR_Country_DeleteByPK";
            objCmd.Parameters.AddWithValue("@CountryID", CountryID);
            objCmd.ExecuteNonQuery();
            connection.Close();
            return RedirectToAction("LOC_CountryList");


        }
        #endregion

        #region Filter

        public IActionResult LOC_CountryFilter(LOC_CountryFilterModel filterModel)
        {
            string connectionStr = this.Configuration.GetConnectionString("myconnectionString");
            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(connectionStr);
            connection.Open();
            SqlCommand objCmd = connection.CreateCommand();
            objCmd.CommandType = CommandType.StoredProcedure;
            objCmd.CommandText = "PR_Country_filter";
            objCmd.Parameters.AddWithValue("@CountryName", filterModel.CountryName);
            objCmd.Parameters.AddWithValue("@CountryCode", filterModel.CountryCode);
            SqlDataReader objSDR = objCmd.ExecuteReader();
            dt.Load(objSDR);
            ModelState.Clear();
            return View("LOC_CountryList", dt);
        }

        #endregion
    }
}
