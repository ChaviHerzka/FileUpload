using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace imagessharedpassword.Data
{
    public class ShareImagesDB
    {
        private readonly string _connectionString;
        public ShareImagesDB(string connectionString)
        {
            _connectionString = connectionString;
        }
        public void AddImage(Image image)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = connection.CreateCommand();
            command.CommandText = @"INSERT INTO Images(ImageName, Views, Password)
                                    VALUES (@imagename,0,@password)
                                    SELECT SCOPE_IDENTITY()";
            command.Parameters.AddWithValue("@imagename", image.ImageName);
            command.Parameters.AddWithValue("@password", image.Password);
            connection.Open();
            image.Id = (int)(decimal)command.ExecuteScalar();
        }
        public Image GetImage(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = connection.CreateCommand();
            command.CommandText = @"SELECT * FROM Images where Id = @id";
            command.Parameters.AddWithValue("@id", id);
            connection.Open();
            var reader = command.ExecuteReader();
            reader.Read();
            Image image = new Image()
            {
                Id = id,
                ImageName = (string)reader["ImageName"],
                Password = (string)reader["Password"],
                Views = (int)reader["views"]
            };
            return image;
        }
        public void UpdateViews(Image image) 
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = connection.CreateCommand();
            command.CommandText = @"UPDATE Images SET Views = Views+1 WHERE Id = @id";
            command.Parameters.AddWithValue("@id", image.Id);
            connection.Open();
            command.ExecuteNonQuery();
            image.Views++;
        }
    }
}
