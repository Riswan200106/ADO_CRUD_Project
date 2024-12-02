using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using ADO_CRUD_Project.Models;
using System.Runtime.Remoting.Messaging;

namespace ADO_CRUD_Project.DataAL
{
    public class ProductDataAL
    {
        string connString = ConfigurationManager.ConnectionStrings["adoconnectionstring"].ToString();

        //GET All Products
        public List<Product>GetAllProducts()
        {
            List<Product> productList = new List<Product>();
            using (SqlConnection conn = new SqlConnection(connString))
            {
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "sp_GetAllProducts";
                SqlDataAdapter sqladapter = new SqlDataAdapter(cmd);
                DataTable dtProducts = new DataTable();

                conn.Open();
                sqladapter.Fill(dtProducts);
                conn.Close();

                //here we are reading each row and coverting it into list
                foreach (DataRow dr in dtProducts.Rows) 
                {
                    productList.Add(new Product
                    {
                        ProductID = Convert.ToInt32(dr["ProductID"]),
                        ProductName = dr["ProductName"].ToString(),
                        Price = Convert.ToDecimal(dr["Price"]),
                        Quantity = Convert.ToInt32(dr["Quantity"]),
                        Remarks = dr["Remarks"].ToString()
                    });
                }

            }
            return productList;
        }

        //Insert Products
        public bool InsertProduct(Product product)
        {
            int id = 0;
            using (SqlConnection connection = new SqlConnection(connString))
            {
                SqlCommand command = new SqlCommand("sp_InsertProducts ", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ProductName", product.ProductName);
                command.Parameters.AddWithValue("@Price", product.Price);
                command.Parameters.AddWithValue("@Quantity", product.Quantity);
                command.Parameters.AddWithValue("Remarks", product.Remarks);

                connection.Open();
                id = command.ExecuteNonQuery();
                connection.Close();
            }
            if(id > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //Get Products by product ID
        public List<Product> GetProductByID(int ProductID)
        {
            List<Product> productList = new List<Product>();
            using (SqlConnection conn = new SqlConnection(connString))
            {
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "sp_GetProductByID";
                cmd.Parameters.AddWithValue("@ProductID", ProductID);
                SqlDataAdapter sqladapter = new SqlDataAdapter(cmd);
                DataTable dtProducts = new DataTable();

                conn.Open();
                sqladapter.Fill(dtProducts);
                conn.Close();

                //here we are reading each row and coverting it into list
                foreach (DataRow dr in dtProducts.Rows)
                {
                    productList.Add(new Product
                    {
                        ProductID = Convert.ToInt32(dr["ProductID"]),
                        ProductName = dr["ProductName"].ToString(),
                        Price = Convert.ToDecimal(dr["Price"]),
                        Quantity = Convert.ToInt32(dr["Quantity"]),
                        Remarks = dr["Remarks"].ToString()
                    });
                }

            }
            return productList;
        }

        //Update Products
        public bool UpdateProduct(Product product)
        {
            int i = 0;
            using (SqlConnection connection = new SqlConnection(connString))
            {
                SqlCommand command = new SqlCommand("sp_updateProducts ", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ProductID", product.ProductID);
                command.Parameters.AddWithValue("@ProductName", product.ProductName);
                command.Parameters.AddWithValue("@Price", product.Price);
                command.Parameters.AddWithValue("@Quantity", product.Quantity);
                command.Parameters.AddWithValue("Remarks", product.Remarks);

                connection.Open();
                i = command.ExecuteNonQuery();
                connection.Close();
            }
            if (i > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public string DeleteProduct(int productid)
        {
            string result = "";

            using (SqlConnection connection = new SqlConnection(connString))
            {
                SqlCommand command = new SqlCommand("sp_DeleteProduct", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ProductID",productid);
                command.Parameters.Add("@OUTPUTMESSAGE", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;

                connection.Open();
                command.ExecuteNonQuery();
                result = command.Parameters["@OUTPUTMESSAGE"].Value.ToString();
                connection.Close();
            }
            return result;
        }
    }
}
