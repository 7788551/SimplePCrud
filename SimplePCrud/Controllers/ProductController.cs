using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using SimplePCrud.Models;

namespace SimplePCrud.Controllers
{
    public class ProductController : Controller
    {
        private readonly IConfiguration _configuration;

        public ProductController(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        public IActionResult ProductList()
        {
            List<Products> list = new List<Products>();
            string connectionString =
                _configuration.GetConnectionString("DefaultConnection");

            string query = "SELECT * FROM Products";

            using (SqlConnection con =
                   new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    con.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            list.Add(new Products
                            {
                                ProductId = Convert.ToInt32(dr["ProductId"]),
                                ProductName = dr["ProductName"].ToString(),
                                Price = Convert.ToDecimal(dr["Price"]),
                                Quantity = Convert.ToInt32(dr["Quantity"])
                            });
                        }
                    }
                }
            }

            return View(list);

        }


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Products obj)
        {
            string connectionString =
                _configuration.GetConnectionString("DefaultConnection");

            string query = @"INSERT INTO Products (ProductName, Price,Quantity) VALUES (@ProductName,@Price,@Quantity)";


            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@ProductName", obj.ProductName);
                    cmd.Parameters.AddWithValue("@Price",obj.Price);
                    cmd.Parameters.AddWithValue("@Quantity", obj.Quantity);
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }

                return RedirectToAction("ProductList");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            Products obj = new Products();
            string connectionString =
                _configuration.GetConnectionString("DefaultConnection");

            string query = @"SELECT * FROM Products WHERE ProductId = @ProductId";

            using ( SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@ProductId",id );
                    con.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            obj.ProductId = Convert.ToInt32(dr["ProductId"]);
                            obj.ProductName = dr["ProductName"].ToString();
                            obj.Price = Convert.ToDecimal(dr["Price"]);
                            obj.Quantity = Convert.ToInt32(dr["Quantity"]);

                        }
                    }
                }

            }

                return View(obj);
        }

        [HttpPost]
        public IActionResult Edit (Products obj)
        {
            string connectionString =
                _configuration.GetConnectionString("DefaultConnection");

            string query = @"UPDATE Products SET
                              ProductName = @ProductName,
                              Price = @Price,
                              Quantity = @Quantity 
                      WHERE ProductId = @ProductId";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@ProductName", obj.ProductName);
                    cmd.Parameters.AddWithValue("@Price", obj.Price);
                    cmd.Parameters.AddWithValue("@Quantity", obj.Quantity);
                    cmd.Parameters.AddWithValue("@ProductId", obj.ProductId);
                    con.Open();
                    cmd.ExecuteNonQuery();

                }
            }

                return RedirectToAction("ProductList","Product");
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            string connectionString =
                _configuration.GetConnectionString("DefaultConnection");

            string query = @"DELETE FROM Products
                             WHERE ProductId = @ProductId";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@ProductId", id);
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }

                return RedirectToAction("ProductList", "Product");
        }


    }
}

