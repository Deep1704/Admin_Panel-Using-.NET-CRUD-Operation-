using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using Admin_Panel.Areas.LOC_City.Models;
using Admin_Panel.Areas.LOC_Country.Models;
using Admin_Panel.Areas.LOC_State.Models;
using System.Collections.Generic;

namespace Admin_Panel.Areas.LOC_City.Controllers
{
    [Area("LOC_City")]
    [Route("LOC_City/[Controller]/[action]")]
    public class LOC_CityController : Controller
    {
        public IConfiguration Configuration;
       


        public LOC_CityController(IConfiguration _configuration)
        {
            Configuration = _configuration;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult LOC_CityList()
        {
            #region CityList

            string connectionstr = this.Configuration.GetConnectionString("myconnectionstring");
            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(connectionstr);
            connection.Open();
            SqlCommand objCmd = connection.CreateCommand();
            objCmd.CommandType = CommandType.StoredProcedure;
            objCmd.CommandText = "PR_City_SelectAll";
            SqlDataReader objSDR = objCmd.ExecuteReader();
            dt.Load(objSDR);
            return View("LOC_CityList", dt);

            #endregion

        }
        public IActionResult LOC_CityAdd(int? CityID)
        {

            string connectionstr = this.Configuration.GetConnectionString("myconnectionstring"); 

            #region Country DropDown
            SqlConnection connection = new SqlConnection(connectionstr);
            connection.Open();

            SqlCommand objCmd = connection.CreateCommand();
            objCmd.CommandType = CommandType.StoredProcedure;
            objCmd.CommandText = "PR_Country_SelectByCombobox";
            DataTable dt = new DataTable();
            SqlDataReader objSDR = objCmd.ExecuteReader();
            dt.Load(objSDR);

            List<LOC_CountryDropDownModel> list = new List<LOC_CountryDropDownModel>();
            foreach (DataRow dr in dt.Rows)
            {
                LOC_CountryDropDownModel countryModel = new LOC_CountryDropDownModel();
                countryModel.CountryID = Convert.ToInt32(dr["CountryID"]);
                countryModel.CountryName = dr["CountryName"].ToString();
                list.Add(countryModel);

            }
            ViewBag.CountryList = list;

            List<LOC_StateDropDownModel> list1 = new List<LOC_StateDropDownModel>();
            ViewBag.StateList = list1;
            #endregion


            #region SelectByPK

            if (CityID != null)
            {

                SqlConnection connection2 = new SqlConnection(connectionstr);
                connection2.Open();

                SqlCommand objCmd2 = connection.CreateCommand();
                objCmd2.CommandType = CommandType.StoredProcedure;
                objCmd2.CommandText = "PR_City_SelectByPK";
                objCmd2.Parameters.AddWithValue("@CityID", CityID);
                DataTable dt2 = new DataTable();
                SqlDataReader objSDR2 = objCmd2.ExecuteReader();
                dt2.Load(objSDR2);
          
                LOC_CityModel modelLOC_City = new LOC_CityModel();

                foreach (DataRow dr in dt2.Rows)
                {

                    modelLOC_City.CityID = Convert.ToInt32(dr["CityID"]);
                    modelLOC_City.StateID = Convert.ToInt32(dr["StateID"]);
                    modelLOC_City.CountryID = Convert.ToInt32(dr["CountryID"]);
                    modelLOC_City.CityName = dr["CityName"].ToString();
                    modelLOC_City.CityCode = dr["CityCode"].ToString();
                    modelLOC_City.CountryName = dr["CountryName"].ToString();
                    modelLOC_City.StateName = dr["StateName"].ToString();
                }
              
                return View("LOC_CityAdd", modelLOC_City);
            }
            else
            {
                return View("LOC_CityAdd");
            }
            #endregion
        }

        public IActionResult DropDownByCountry(int CountryID)
        {
            #region State DropDown


            string connectionstr = this.Configuration.GetConnectionString("myconnectionstring");
            SqlConnection connection1 = new SqlConnection(connectionstr);
            connection1.Open();

            SqlCommand objCmd1 = connection1.CreateCommand();
            objCmd1.CommandType = CommandType.StoredProcedure;
            objCmd1.CommandText = "PR_State_SelectComboBoxByCountryID";
            objCmd1.Parameters.AddWithValue("@CountryID", CountryID);
            DataTable dt1 = new DataTable();
            SqlDataReader objSDR1 = objCmd1.ExecuteReader();
            dt1.Load(objSDR1);

            List<LOC_StateDropDownModel> list1 = new List<LOC_StateDropDownModel>();
            foreach (DataRow dr in dt1.Rows)
            {
                LOC_StateDropDownModel stateModel = new LOC_StateDropDownModel();
                stateModel.StateID = Convert.ToInt32(dr["StateID"]);
                stateModel.StateName = dr["StateName"].ToString();
                list1.Add(stateModel);

            }
            Console.WriteLine(list1.Count());
            var vModel = list1;
            return Json(vModel);

           
            #endregion
        }

        #region Insert
        public IActionResult Save(LOC_CityModel modelLOC_City)
        {
            string connectionstr = this.Configuration.GetConnectionString("myconnectionstring");
            SqlConnection connection = new SqlConnection(connectionstr);
            connection.Open();
            SqlCommand objCmd = connection.CreateCommand();
            objCmd.CommandType = CommandType.StoredProcedure;


            if (modelLOC_City.CityID == null)
            {
                objCmd.CommandText = "PR_City_Insert";
            }
            else
            {
                objCmd.CommandText = "PR_City_UpdateByPK";
                objCmd.Parameters.AddWithValue("@CityID", modelLOC_City.CityID);

            }
            Console.WriteLine(modelLOC_City.CountryID);
            Console.WriteLine(modelLOC_City.StateID);


            objCmd.Parameters.AddWithValue("@CityName", modelLOC_City.CityName);
            objCmd.Parameters.AddWithValue("@CityCode", modelLOC_City.CityCode);
            objCmd.Parameters.AddWithValue("@CountryID", modelLOC_City.CountryID);
            objCmd.Parameters.AddWithValue("@StateID", modelLOC_City.StateID);

            objCmd.ExecuteNonQuery();

            connection.Close();
            return RedirectToAction("LOC_CityList");

            #endregion
        }



        public IActionResult Delete(int? CityID)
        {
            #region Delete

            string connectionstr = this.Configuration.GetConnectionString("myconnectionstring");
            SqlConnection connection = new SqlConnection(connectionstr);
            connection.Open();

            SqlCommand objCmd = connection.CreateCommand();
            objCmd.CommandType = CommandType.StoredProcedure;
            objCmd.CommandText = "PR_City_DeleteByPK";
            objCmd.Parameters.AddWithValue("@CityID", CityID);
            objCmd.ExecuteNonQuery();
            connection.Close();
            return RedirectToAction("LOC_CityList");


        }
        #endregion

        #region City Search By Name

        public IActionResult LOC_CitySearchByName(string CityName)
        {
            string connectionStr = this.Configuration.GetConnectionString("myConnectionString");
            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(connectionStr);
            connection.Open();
            SqlCommand objCmd = connection.CreateCommand();
            objCmd.CommandType = CommandType.StoredProcedure;
            objCmd.CommandText = "PR_City_SelectByCityName";
            objCmd.Parameters.AddWithValue("@CName", CityName);
            SqlDataReader objSDR = objCmd.ExecuteReader();
            dt.Load(objSDR);
            return View("LOC_CityList", dt);
        }

        #endregion

    }
}
