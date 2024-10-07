using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace LMS.Controllers
{
    public class StudentController : Controller
    {

        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString);

        // GET: Student


        public ActionResult Submit()
        {
            return View();
        }

        public ActionResult dashboard()
        {
            return View();
        }

        public ActionResult courses() {
            string query = "select * from tbl_category order by sr desc";
            SqlDataAdapter sda = new SqlDataAdapter(query, con);
            DataTable dt = new DataTable();
            sda.Fill(dt);

            ViewBag.cat = dt;
            return View();
        }

        public ActionResult softwarekit()
        {
            return View();
        }

        public ActionResult task() {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString);


            int batchid = Convert.ToInt32(Session["batch"]);
            string query = $"select * from tbl_assignment left join tbl_submittedtask on tbl_assignment.sr = taskno where batch = {batchid} order by tbl_assignment.sr desc";
            SqlDataAdapter sda = new SqlDataAdapter(query, con);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            ViewBag.task = dt;
            return View();
        }

        public ActionResult videoLecture(int? catid) {
            if (catid.HasValue)
            {
                int batchid = Convert.ToInt32(Session["batch"]);
                string query = $"select * from tbl_video where batch = {batchid} and category={catid} order by sr ";
                SqlDataAdapter sda = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                ViewBag.video = dt;
                return View();
            }
            else
            {
                return Content("<script>alert('Please select video category');location.href='/student/courses'</script>");
            }
           
        }

        public ActionResult changepassword()
        {
            return View();
        }

        public ActionResult notes() {
            string batch = Session["batch"].ToString();
            string query = $"select * from tbl_notes where batch = {batch} order by sr desc";
            SqlDataAdapter ada = new SqlDataAdapter (query, con);
            DataTable dt = new DataTable();
            ada.Fill(dt);
            ViewBag.notes = dt;
            return View();
        }

        public ActionResult logout()
        {
            Session.RemoveAll();
            return Content("<script>location.href='/home/index'</script>");
        }
        [HttpPost]
        public ActionResult submittask(int? taskno, string email, HttpPostedFileBase taskfile)
        {
            string query = $"insert into tbl_submittedtask values({taskno},'{email}','{taskfile.FileName}','{DateTime.Now.ToString("yyyy-MM-dd")}',0,'')";
            SqlCommand cmd = new SqlCommand(query, con);
            con.Open();
            int result = cmd.ExecuteNonQuery();
            con.Close();
            if (result > 0)
            {
                taskfile.SaveAs(Server.MapPath("/Content/answerfile/" + taskfile.FileName));
                return Content("<script>alert('File uploaded Successfully');location.href='/student/task'</script>");
            }
            else
            {
                return Content("<script>alert('File not submitted');location.href='/student/task'</script>");
            }
        }
        [HttpPost]
        public ActionResult changepass(string opasswd, string npasswd, string cpasswd)
        {
            if (npasswd.Equals(cpasswd))
            {
                string email = Session["uname"].ToString();
                string query = $"update tbl_student set password = '{npasswd}' where emailid = '{email}' and password = '{opasswd}'";
                SqlCommand cmd = new SqlCommand(query, con);
                con.Open();
                int result = cmd.ExecuteNonQuery();
                con.Close();
                if (result > 0) {
                    Session.RemoveAll();
                    return Content("<script>alert('Your password has been updated');location.href = '/home/index'</script>");
                }
                else
                {
                    return Content("<script>alert('Not changed');location.href = '/student/changepassword'</script>");
                }
            }
            else
            {
                return Content("<script>alert('New password and confirm password should be match');location.href='/student/changepassword'</script>");
            }
            
        }
    }
}