using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Web.Mvc.Ajax;
using System.Web.WebPages;
using System.Configuration;


namespace LMS.Controllers
{
    public class AdminController : Controller
    {
        
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString);
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AddBatch()
        {
            string query = "select * from tbl_batch order by sr desc";
            SqlDataAdapter sda = new SqlDataAdapter(query, con);
            DataTable dt = new DataTable();
            sda.Fill(dt);

            ViewBag.tb1 = dt;
            return View();
        }

        public ActionResult AddVideoCategory()
        {
            string query1 = "select * from tbl_category order by sr desc";
            SqlDataAdapter sd = new SqlDataAdapter(query1, con);
            DataTable dt1 = new DataTable();
            sd.Fill(dt1);

            ViewBag.tb2 = dt1;
            return View();
        }

        //onload event of addvideo form
        public ActionResult Video()
        {
            //select batch & select category
            string query = "select * from tbl_batch order by sr desc";
            SqlDataAdapter sda = new SqlDataAdapter(query, con);
            DataTable dt = new DataTable();
            sda.Fill(dt);

            //select data from video category
            string query1 = "select * from tbl_category order by sr desc";
            SqlDataAdapter sd = new SqlDataAdapter(query1, con);
            DataTable dt1 = new DataTable();
            sd.Fill(dt1);

            string query2 = "select * from tbl_video order by sr desc";
            SqlDataAdapter sd2 = new SqlDataAdapter(query2, con);
            DataTable dt2 = new DataTable();
            sd2.Fill(dt2);

            ViewBag.tb1 = dt;
            ViewBag.tb2 = dt1;
            ViewBag.tb3 = dt2;
            return View();
        }

        public ActionResult assignment()
        {
            string query = "select * from tbl_batch order by sr desc";
            SqlDataAdapter sda = new SqlDataAdapter(query, con);
            DataTable dt = new DataTable();
            sda.Fill(dt);

            string query1 = "select * from tbl_assignment order by sr desc";
            SqlDataAdapter sd = new SqlDataAdapter(query1, con);
            DataTable dt1 = new DataTable();
            sd.Fill(dt1);


            ViewBag.tb1 = dt;
            ViewBag.tb2 = dt1;

            return View();
        }

        public ActionResult ManageStudent()
        {
            string query = "select * from tbl_student left join tbl_batch on tbl_student.batch=tbl_batch.sr order by regdate";
            SqlDataAdapter sda = new SqlDataAdapter(query, con);
            DataTable dt = new DataTable();
            sda.Fill(dt);

            ViewBag.data = dt;
            return View();
        }
        public ActionResult Manageenquiry()
        {
            string query = "select * from tbl_contact order by sr desc";
            SqlDataAdapter sda = new SqlDataAdapter(query, con);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            ViewBag.table1 = dt;
            return View();
        }

        [HttpPost]
        public ActionResult SaveBatch(string txt_class)
        {


            string query = $"insert into tbl_batch values('{txt_class}',1)";

            SqlCommand cmd = new SqlCommand(query, con);
            con.Open();
            int n = cmd.ExecuteNonQuery();
            con.Close();
            if (n > 0)
            {
                return Content("<script>alert('Batch Added Successfully');location.href='/admin/addbatch'</script>");
            }
            else
            {
                return Content("<script>alert('Batch Not Added ');location.href='/admin/addbatch'</script>");
            }
        }

        public ActionResult savevideocategory(string txt_category, HttpPostedFileBase file_thumbnail)
        {
            string query = $"insert into tbl_category values('{txt_category}','{file_thumbnail.FileName}','{DateTime.Now.ToString("yyyy-MM-dd")}')";

            SqlCommand cmd = new SqlCommand(query, con);
            con.Open();
            int n = cmd.ExecuteNonQuery();
            con.Close();
            if (n > 0)
            {
                file_thumbnail.SaveAs(Server.MapPath("/Content/catpic/" + file_thumbnail.FileName));
                return Content("<script>alert('Video Category Added Successfully');location.href='/admin/addvideocategory'</script>");
            }
            else
            {
                return Content("<script>alert('Video Category Not Added ');location.href='/admin/addvideocategory'</script>");
            }
        }

        public ActionResult savevideo(string txt_title, int ddl_batch, int ddl_category, string txt_desc, string txt_link, HttpPostedFileBase file_thumbnail)
        {
            string query = $"insert into tbl_video values('{txt_title}',{ddl_batch},{ddl_category},'{txt_desc}','{txt_link}','{file_thumbnail.FileName}','{DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")}')";

            SqlCommand cmd = new SqlCommand(query, con);
            con.Open();
            int n = cmd.ExecuteNonQuery();
            con.Close();
            if (n > 0)
            {
                file_thumbnail.SaveAs(Server.MapPath("/Content/videopic/" + file_thumbnail.FileName));
                return Content("<script>alert('Video Added Successfully');location.href='/admin/video'</script>");
            }
            else
            {
                return Content("<script>alert('Video Not Added ');location.href='/admin/video'</script>");
            }
        }

        public ActionResult saveassignment(int ddl_batch, string txt_subject, string txt_title, string txt_desc, HttpPostedFileBase file_assignment, string txt_teacher, DateTime date_lastdate)
        {
            string query = $"insert into tbl_assignment values('{ddl_batch}','{txt_subject}','{txt_title}','{txt_desc}','{file_assignment.FileName}','{txt_teacher}','{date_lastdate.ToString("yyyy-MM-dd")}','{DateTime.Now.ToString("yyyy-MM-dd")}',1)";

            SqlCommand cmd = new SqlCommand(query, con);
            con.Open();
            int res = cmd.ExecuteNonQuery();
            con.Close();

            if (res > 0)
            {
                file_assignment.SaveAs(Server.MapPath("/Content/saveassignment/" + file_assignment.FileName));
                return Content("<script>alert('Assignment Added Successfully');location.href='/admin/assignment'</script>");
            }
            else
            {
                return Content("<script>alert('Assignment Not Added ');location.href='/admin/assignment'</script>");
            }
        }

        public ActionResult updatestatus(string email, int? status)
        {
            if (!email.IsEmpty() && status.HasValue)
            {
                int s = status == 0 ? 1 : 0;
                string query = $"update tbl_student set status={s} where emailid='{email}'";
                SqlCommand cmd = new SqlCommand(query, con);
                con.Open();
                int result = cmd.ExecuteNonQuery();
                con.Close();
                return Content("<script>alert('Status updated');location.href='/admin/managestudent'</script>");
            }
            else
            {
                return Content("<script>alert('Try again');location.href='/admin/managestudent'</script>");
            }
        }
        public ActionResult logout()
        {
            Session.Remove("admin");
            return Content("<script>alert('Log out');location.href='/home/index'</script>");
        }
        public ActionResult admindashboard()
        {
            return View();
        }
        public ActionResult manageassignment()
        {
            string query = "select * from tbl_submittedtask left join tbl_assignment on  taskno=tbl_assignment.sr left join tbl_student on tbl_submittedtask.userid = tbl_student.emailid order by tbl_submittedtask.sr desc";
            SqlDataAdapter sda = new SqlDataAdapter(query, con);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            ViewBag.assignmgmt = dt;
            return View();
        }
        public ActionResult notes()
        {
            string query = "select * from tbl_batch order by sr desc";
            SqlDataAdapter sda = new SqlDataAdapter(query, con);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            ViewBag.table1 = dt;
            return View();
        }
        [HttpPost]
        public ActionResult savenotes(string txt_title, string txt_desc,string txt_teacher, int ddl_batch, HttpPostedFileBase file_notes)
        {
            string query = $"insert into tbl_notes values('{txt_title}','{txt_desc}','{txt_teacher}','{file_notes.FileName}',{ddl_batch},'{DateTime.Now.ToString("yyyy-MM-dd")}')";
            SqlCommand cmd = new SqlCommand(query, con);
            con.Open();
            int result = cmd.ExecuteNonQuery();
            con.Close();
            if (result > 0)
            {
                file_notes.SaveAs(Server.MapPath("/Content/notes/" + file_notes.FileName));
                return Content("<script>alert('Notes Added Successfully');location.href='/admin/notes'</script>");
            }
            else
            {
                return Content("<script>alert('Notes Not Added ');location.href='/admin/notes'</script>");
            }
        }
        
    }
}
