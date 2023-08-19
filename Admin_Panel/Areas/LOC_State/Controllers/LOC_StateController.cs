using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using Admin_Panel.Areas.LOC_State.Models;
using Admin_Panel.Areas.LOC_Country.Models;

namespace Admin_Panel.Areas.LOC_State.Controllers
{

    [Area("LOC_State")]
    [Route("LOC_State/[Controller]/[action]")]
    public class LOC_StateController : Controller
    {
        public IConfiguration Configuration;

        public LOC_StateController(IConfiguration _configuration)
        {
            Configuration = _configuration;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult LOC_StateList()
        {
            #region StateList

            string connectionstr = this.Configuration.GetConnectionString("myconnectionstring");
            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(connectionstr);
            connection.Open();
            SqlCommand objCmd = connection.CreateCommand();
            objCmd.CommandType = CommandType.StoredProcedure;
            objCmd.CommandText = "PR_State_SelectAll";
            SqlDataReader objSDR = objCmd.ExecuteReader();
            dt.Load(objSDR);
            return View("LOC_StateList", dt);

            #endregion

        }

        #region Insert
        public IActionResult Save(LOC_StateModel modelLOC_State)
        {
            string connectionstr = this.Configuration.GetConnectionString("myconnectionstring");
            SqlConnection connection = new SqlConnection(connectionstr);
            connection.Open();
            SqlCommand objCmd = connection.CreateCommand();
            objCmd.CommandType = CommandType.StoredProcedure;


            if (modelLOC_State.StateID == null)
            {
                objCmd.CommandText = "PR_State_Insert";
            }
            else
            {
                objCmd.CommandText = "PR_State_UpdateByPK";
                objCmd.Parameters.AddWithValue("@StateID", modelLOC_State.StateID);

            }


            objCmd.Parameters.AddWithValue("@StateName", modelLOC_State.StateName);
            objCmd.Parameters.AddWithValue("@StateCode", modelLOC_State.StateCode);
            objCmd.Parameters.AddWithValue("@CountryID", modelLOC_State.CountryID);

            objCmd.ExecuteNonQuery();

            connection.Close();
            return RedirectToAction("LOC_StateList");

            #endregion
        }


        public IActionResult LOC_StateAdd(int? StateID)
        {
            string connectionstr = this.Configuration.GetConnectionString("myconnectionstring");

            #region  Country DropDown

            SqlConnection connection1 = new SqlConnection(connectionstr);
            connection1.Open();

            SqlCommand objCmd1 = connection1.CreateCommand();
            objCmd1.CommandType = CommandType.StoredProcedure;
            objCmd1.CommandText = "PR_Country_SelectByCombobox";
            DataTable dt1 = new DataTable();
            SqlDataReader objSDR1 = objCmd1.ExecuteReader();
            dt1.Load(objSDR1);

            List<LOC_CountryDropDownModel> list = new List<LOC_CountryDropDownModel>();
            foreach (DataRow dr in dt1.Rows)
            {
                LOC_CountryDropDownModel countryModel = new LOC_CountryDropDownModel();
                countryModel.CountryID = Convert.ToInt32(dr["CountryID"]);
                countryModel.CountryName = dr["CountryName"].ToString();
                list.Add(countryModel);

            }
            ViewBag.CountryList = list;

            #endregion

            #region SelectByPK

            if (StateID != null)
            {

                SqlConnection connection = new SqlConnection(connectionstr);
                connection.Open();

                SqlCommand objCmd = connection.CreateCommand();
                objCmd.CommandType = CommandType.StoredProcedure;
                objCmd.CommandText = "PR_State_SelectByPK";
                objCmd.Parameters.AddWithValue("@StateID", StateID);
                DataTable dt = new DataTable();
                SqlDataReader objSDR = objCmd.ExecuteReader();
                dt.Load(objSDR);
                LOC_StateModel modelLOC_State = new LOC_StateModel();

                foreach (DataRow dr in dt.Rows)
                {

                
                    modelLOC_State.CountryID = Convert.ToInt32(dr["CountryID"]);
                    modelLOC_State.StateName = dr["StateName"].ToString();
                    modelLOC_State.StateCode = dr["StateCode"].ToString();
                    modelLOC_State.CountryName = dr["CountryName"].ToString();
                }
                return View("LOC_StateAdd", modelLOC_State);
            }
            else
            {
                return View("LOC_StateAdd");
            }
        }

        #endregion

        #region Delete
        public IActionResult Delete(int? StateID)
        {
            string connectionstr = this.Configuration.GetConnectionString("myconnectionstring");
            SqlConnection connection = new SqlConnection(connectionstr);
            connection.Open();

            SqlCommand objCmd = connection.CreateCommand();
            objCmd.CommandType = CommandType.StoredProcedure;
            objCmd.CommandText = "PR_State_DeleteByPK";
            objCmd.Parameters.AddWithValue("@StateID", StateID);
            objCmd.ExecuteNonQuery();
            connection.Close();
            return RedirectToAction("LOC_StateList");


        }
        #endregion

        #region State Search By Name

        public IActionResult LOC_StateSearchByName(string StateName)
        {
            string connectionStr = this.Configuration.GetConnectionString("myConnectionString");
            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(connectionStr);
            connection.Open();
            SqlCommand objCmd = connection.CreateCommand();
            objCmd.CommandType = CommandType.StoredProcedure;
            objCmd.CommandText = "PR_State_SelectByStateName";
            objCmd.Parameters.AddWithValue("@SName", StateName);
            SqlDataReader objSDR = objCmd.ExecuteReader();
            dt.Load(objSDR);
            return View("LOC_StateList", dt);
        }

        #endregion
    }
}
