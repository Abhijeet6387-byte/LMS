using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Web.Mvc.Ajax;
using System.Configuration;


namespace LMS.Controllers
{
    public class HomeController : Controller
    {
       
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString);

        // GET: Home
        public ActionResult Index()
        {
            string query = "select * from tbl_batch order by sr desc";
            SqlDataAdapter sda = new SqlDataAdapter(query, con);
            DataTable dt = new DataTable();
            sda.Fill(dt);

            ViewBag.tb1 = dt;
            return View();
        }

        [HttpPost]
        public ActionResult save(string txt_title, long txt_num,string add_email,string txt_subject,string text_desc)
        {

            string query = $"insert into tbl_contact values('{txt_title}',{txt_num},'{add_email}','{txt_subject}','{text_desc}','{DateTime.Now.ToString("yyyy-MM-dd")}')";
            SqlCommand com = new SqlCommand(query, con);
            con.Open();
            int result = com.ExecuteNonQuery();
            con.Close();

            if (result > 0)
            {
                return Content("<script>alert('Message Sent Successfully');location.href='/home/index'</script>");
            }
            else
            {
                return Content("<script>alert('Message Not Sent');location.href='/home/index'</script>");
            }
        }

        public ActionResult saveuser(string txt_name,string txt_email,long txt_mobno, HttpPostedFileBase file_profile,string ddl_course, string ddl_year,int ddl_batch,string txt_passwd)
        {
            string query = $"insert into tbl_student values('{txt_name}','{txt_mobno}','{txt_email}','{txt_passwd}','{ddl_course}','{ddl_course}','{ddl_year}','{file_profile.FileName}','{ddl_batch}',0,'{DateTime.Now.ToString("yyyy-MM-dd")}')";
            SqlCommand cmd = new SqlCommand(query,con);
            con.Open();
            int res = cmd.ExecuteNonQuery();
            con.Close();
            if (res > 0)
            {
                file_profile.SaveAs(Server.MapPath("/Content/profilepic/" + file_profile.FileName));
                return Content("<script>alert('You have registerd successfully. You will receive confirmation from admin very soon via email');location.href='/home/index'</script>");
            }
            else
            {
                return Content("<script>alert('Unable to Register');location.href='/home/index'</script>");
            }
        }
        [HttpPost]
        public ActionResult Login(string uname, string passwd)
        {
            string query = $"select * from tbl_student where emailid='{uname}' and password='{passwd}'";
            SqlDataAdapter sda = new SqlDataAdapter(query,con);
            DataTable dt = new DataTable();
            sda.Fill(dt);

            if (dt.Rows.Count > 0)
            {
                if (Convert.ToInt32(dt.Rows[0]["status"]) == 1)
                {
                    Session["name"] = dt.Rows[0]["name"];
                    Session["uname"] = dt.Rows[0]["emailid"];
                    Session["batch"] = dt.Rows[0]["batch"];
                    Session["picture"] = dt.Rows[0]["picture"];
                    Session["phno"] = dt.Rows[0]["mobno"];
                    Session["course"] = dt.Rows[0]["course"];
                    return Content("<script>alert('Welcome');location.href='/student/dashboard'</script>");
                }
                else
                {
                    return Content("<script>alert('You are not authorize by admin!!');location.href='/home/index'</script>");
                }
            }
            else
            {
                return Content("<script>alert('Invalid Id or Password');location.href='/home/index'</script>");
            }
        }
        public ActionResult adminlogin()
        {
            return View();
        }

        [HttpPost]

        public ActionResult adminsignin(string email, string password)
        {
            if(email.Equals("techpile") && password.Equals("123"))
            {
                Session["admin"] = email;
                return Content("<script>alert('Welcome');location.href='/admin/admindashboard'</script>");
            }
            else
            {
                return Content("<script>alert('Invalid Id and Password');location.href='/home/adminlogin'</script>");
            }
        }
    }
}