using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using SimplePCrud.Models;

namespace SimplePCrud.Controllers
{
    public class AccountController : Controller
    {
        private readonly IConfiguration _configuration;

        public AccountController(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(Users user)
        {
            string connectionString = _configuration.GetConnectionString("DefaultConnection");

            string query = @"INSERT INTO Users (FullName,Email,Password,RoleId) 
                             VALUES(@FullName,@Email,@Password,@RoleId)";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@FullName", user.FullName);
                    cmd.Parameters.AddWithValue("@Email", user.Email);
                    cmd.Parameters.AddWithValue("@Password", user.Password);
                    cmd.Parameters.AddWithValue("@RoleId", 2);

                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }


                return RedirectToAction("Login","Account");
        }


        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(Users user)
        {
            string connectionString = _configuration.GetConnectionString("DefaultConnection");
            string query = @"SELECT UserId,FullName,Email,Password,RoleId FROM Users
                             WHERE Email=@Email AND Password=@Password";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Email", user.Email);
                    cmd.Parameters.AddWithValue("@Password", user.Password);
                    con.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            HttpContext.Session.SetString(
                                "UserId", dr["UserId"].ToString()
                                );

                            HttpContext.Session.SetString(
                                "FullName",
                                dr["FullName"].ToString());

                            HttpContext.Session.SetString(
                                "RoleId", dr["RoleId"].ToString()
                                );
                            int roleId = Convert.ToInt32(dr["RoleId"]);

                            if(roleId == 1)
                            {
                                return RedirectToAction(
                                    "AdminDashboard",
                                    "Dashboard"
                                    );
                            }
                            else
                            {
                                return RedirectToAction(
                                    "UserDashboard",
                                    "Dashboard"
                                    );
                            }
                        }
                    }       
                }

            }

            ViewBag.Error = "Invalid Email or Password";

                return View(user);
        }


        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");

        }

    }
}
