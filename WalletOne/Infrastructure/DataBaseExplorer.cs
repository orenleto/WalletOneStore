using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using WalletOne.Domain.Entities;

namespace WalletOne.Infrastructure {
    public static class DataBaseExplorer {
        private static string connectionString =
            @"Data Source=SQL5024.myASP.NET;Initial Catalog=DB_9F5EFE_wos;User Id=DB_9F5EFE_wos_admin;Password=1234qwerR!;";

        public static int GetTotalCount(string category = null) {
            using (SqlConnection connection = new SqlConnection(connectionString)) {
                SqlCommand cmd;

                cmd = connection.CreateCommand();
                cmd.CommandText = "TotalCount";
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter categoryParameter = new SqlParameter();
                categoryParameter.ParameterName = "@category";
                categoryParameter.SqlDbType = SqlDbType.NVarChar;
                categoryParameter.Size = 50;
                categoryParameter.Value = category;
                cmd.Parameters.Add(categoryParameter);

                SqlParameter TotalCount = new SqlParameter();
                TotalCount.ParameterName = "@TotalCount";
                TotalCount.SqlDbType = SqlDbType.Int;
                TotalCount.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(TotalCount);

                connection.Open();
                cmd.ExecuteNonQuery();
                int totalCount = (int)cmd.Parameters["@TotalCount"].Value;
                connection.Close();
                return totalCount;
            }
        }
        public static IEnumerable<Product> GetProductsFromDataBase(string category = null, int page = 1, int count = 4) {
            using (SqlConnection connection = new SqlConnection(connectionString)) {
                SqlCommand cmd;

                cmd = connection.CreateCommand();
                cmd.CommandText = "TakeProducts";
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter categoryParameter = new SqlParameter();
                categoryParameter.ParameterName = "@category";
                categoryParameter.SqlDbType = SqlDbType.NVarChar;
                categoryParameter.Size = 50;
                categoryParameter.Value = category;
                cmd.Parameters.Add(categoryParameter);

                SqlParameter pageParameter = new SqlParameter("@page", page);
                cmd.Parameters.Add(pageParameter);

                SqlParameter countParameter = new SqlParameter("@count", count);
                cmd.Parameters.Add(countParameter);

                connection.Open();
                var obj = cmd.ExecuteReader();

                List<Product> collection = new List<Product>();
                Product product;

                while (obj.Read()) {
                    product = new Product();
                    product.ProductID = int.Parse(obj["ProductID"].ToString());
                    product.Name = obj["Name"].ToString();
                    product.Category = obj["Category"].ToString();
                    product.Description = obj["Description"].ToString();
                    product.Price = decimal.Parse(obj["Price"].ToString());
                    product.ImageMimeType = obj["ImageMimeType"].ToString();
                    if (product.ImageMimeType != "")
                        product.ImageData = (byte[])obj["ImageData"];
                    collection.Add(product);
                }
                connection.Close();
                return collection;
            }
        }
        public static Product GetProductFromDataBase(int productId) {
            using (SqlConnection connection = new SqlConnection(connectionString)) {
                SqlCommand cmd;

                cmd = connection.CreateCommand();
                cmd.CommandText = "TakeProduct";
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter idParameter = new SqlParameter();
                idParameter.ParameterName = "@ProductID";
                idParameter.SqlDbType = SqlDbType.Int;
                idParameter.Value = productId;
                cmd.Parameters.Add(idParameter);

                connection.Open();
                var obj = cmd.ExecuteReader();

                Product product = null;

                while (obj.Read()) {
                    product = new Product();
                    product.ProductID = int.Parse(obj["ProductID"].ToString());
                    product.Name = obj["Name"].ToString();
                    product.Category = obj["Category"].ToString();
                    product.Description = obj["Description"].ToString();
                    product.Price = decimal.Parse(obj["Price"].ToString());
                    product.ImageMimeType = obj["ImageMimeType"].ToString();
                    if (product.ImageMimeType != "")
                        product.ImageData = (byte[])obj["ImageData"];
                }
                connection.Close();
                return product;
            }
        }
        public static IEnumerable<string> GetCategories()
        {
            using (SqlConnection connection = new SqlConnection(connectionString)) {
                SqlCommand cmd;

                cmd = connection.CreateCommand();
                cmd.CommandText = "GetCategories";
                cmd.CommandType = CommandType.StoredProcedure;

                
                connection.Open();
                var obj = cmd.ExecuteReader();

                List<string> collection = new List<string>();
                

                while (obj.Read()) {
                    collection.Add(obj["Category"].ToString());
                }
                connection.Close();
                return collection;
            }
        } 

        public static void DeleteProduct(int productId) {
            using (SqlConnection connection = new SqlConnection(connectionString)) {
                SqlCommand cmd;

                cmd = connection.CreateCommand();
                cmd.CommandText = "DeleteProduct";
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter idParameter = new SqlParameter();
                idParameter.ParameterName = "@ProductID";
                idParameter.SqlDbType = SqlDbType.Int;
                idParameter.Value = productId;
                cmd.Parameters.Add(idParameter);

                connection.Open();
                cmd.ExecuteNonQuery();
                connection.Close();
            }
        }
        public static void InsertProduct(bool Create, Product product, HttpPostedFileBase image) {
            using (SqlConnection connection = new SqlConnection(connectionString)) {
                SqlCommand cmd;
                SqlParameter parameter = new SqlParameter();

                cmd = connection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                if (Create) {
                    cmd.CommandText = "InsertProduct";
                } else {
                    cmd.CommandText = "UpdateProduct";
                    parameter.ParameterName = "@ProductId";
                    parameter.SqlDbType = SqlDbType.Int;
                    parameter.Value = product.ProductID;
                    cmd.Parameters.Add(parameter);
                }

                parameter = new SqlParameter();
                parameter.ParameterName = "@Name";
                parameter.SqlDbType = SqlDbType.NVarChar;
                parameter.Size = 100;
                parameter.Value = product.Name;
                cmd.Parameters.Add(parameter);

                parameter = new SqlParameter();
                parameter.ParameterName = "@Description";
                parameter.SqlDbType = SqlDbType.NVarChar;
                parameter.Size = 500;
                parameter.Value = product.Description;
                cmd.Parameters.Add(parameter);

                parameter = new SqlParameter();
                parameter.ParameterName = "@Category";
                parameter.SqlDbType = SqlDbType.NVarChar;
                parameter.Size = 50;
                parameter.Value = product.Category;
                cmd.Parameters.Add(parameter);

                parameter = new SqlParameter();
                parameter.ParameterName = "@Price";
                parameter.SqlDbType = SqlDbType.Decimal;
                parameter.Value = product.Price;
                cmd.Parameters.Add(parameter);

                parameter = new SqlParameter();
                parameter.ParameterName = "@ImageMimeType";
                parameter.SqlDbType = SqlDbType.VarChar;
                parameter.Size = 50;
                if (image != null) {
                    parameter.Value = image.ContentType;
                } else {
                    parameter.Value = product.ImageMimeType;
                }
                cmd.Parameters.Add(parameter);

                parameter = new SqlParameter();
                parameter.ParameterName = "@ImageData";
                parameter.SqlDbType = SqlDbType.VarBinary;
                if (image != null) {
                    byte[] ImageData = new byte[image.ContentLength];
                    image.InputStream.Read(ImageData, 0, image.ContentLength);
                    parameter.Value = ImageData;
                } else {
                    parameter.Value = product.ImageData;
                }

                cmd.Parameters.Add(parameter);


                connection.Open();
                cmd.ExecuteNonQuery();
                connection.Close();
            }
        }
    }
}