using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Data.SqlClient;
using Scannect.Models;

namespace ScannectConsole.Repository
{
    public class ResultRepository
    {
        private static readonly string connectionString = "Server=(localdb)\\mssqllocaldb;Database=ScannectDb;Trusted_Connection=True;MultipleActiveResultSets=true";
        
        public static void SaveItem(Item item)
        {
            var connection = new SqlConnection(connectionString);
            
            if (ItemExistsByUrl(item.Url)) return;

            try
            {
                if (item.Snippet != null)
                {
                    item.Snippet = item.Snippet.Replace("'", "");
                }
            }
            catch
            {
                Console.WriteLine("Error with snippet.");
            }

            var builder = new StringBuilder();
            builder.Append("INSERT INTO Item (Url, WebsiteUrl, Title, Snippet, DateCreated, Hits, Ranking, Category, Author) VALUES ");
            builder.Append("('" + item.Url + "','" + item.WebsiteUrl + "','" + item.Title + "','" + item.Snippet + "','" + DateTime.UtcNow + "','" + 0 + "','" + 0 + "','" + item.Category + "','" + item.Author + "');");
            builder.Append(" SELECT CAST(scope_identity() AS int)");
            var sql = builder.ToString();
            var itemId = 0;

            using(SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(sql, conn);
                try
                {
                    conn.Open();
                    itemId = (int)cmd.ExecuteScalar();
                    Console.WriteLine("A new item was saved to the database.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            if (item.Images != null && item.Images.Count != 0)
            {
                foreach (var image in item.Images)
                {
                    var build = new StringBuilder();
                    build.Append(
                        "INSERT INTO ItemImage (Url, Alt, Title, Annotation, Placeholder, Width, Height, ItemId) VALUES ");
                    build.Append("('" + image.Url + "','" + image.Alt + "','" + image.Title + "','" +
                                 image.Annotation + "','" + image.Placeholder + "','" + image.Width + "','" +
                                 image.Height + "','" + itemId + "')");
                    var imageSql = build.ToString();

                    connection.Open();
                    try
                    {
                        using (var command = new SqlCommand(imageSql, connection))
                        {
                            command.ExecuteNonQuery();

                            Console.WriteLine("A new item was saved to the database.");
                        }
                    }
                    catch
                    {
                        Console.WriteLine("Failed to save item.");
                    }

                    connection.Close();
                }
            }

            connection.Close();
        }
        public static bool ItemExistsByUrl(string url)
        {
            var connection = new SqlConnection(connectionString);

            connection.Open();
            var builder = new StringBuilder();
            builder.Append("SELECT * FROM Item WHERE Url = '" + url + "'");
            var sql = builder.ToString();

            using (var command = new SqlCommand(sql, connection))
            {
                var result = command.ExecuteReader();

                if (result.HasRows)
                {
                    connection.Close();
                    return true;
                }
            }
            connection.Close();
            return false;
        }
    }
}
