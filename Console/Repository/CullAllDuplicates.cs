using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Data.SqlClient;
using Scannect.Models;

namespace ScannectConsole.Repository
{
    public class CullAllDuplicates
    {
        private static readonly string connectionString = "Server=(localdb)\\mssqllocaldb;Database=ScannectDb;Trusted_Connection=True;MultipleActiveResultSets=true";

        public static void Execute()
        {
            var connection = new SqlConnection(connectionString);

            connection.Open();
            var builder = new StringBuilder();
            builder.Append("SELECT * FROM Item");
            var sql = builder.ToString();

            var items = new List<Item>();

            using (var command = new SqlCommand(sql, connection))
            {
                try
                {
                    var reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            var item = new Item
                            {
                                Id = (int)reader["Id"],
                                Url = (string)reader["Url"]
                            };
                            items.Add(item);
                        }
                    }
                }
                catch
                {
                    Console.WriteLine("Cannot read encode URLs");
                }
            }
            connection.Close();

            foreach (var item in items)
            {
                foreach (var url in items)
                {
                    if (item.Id != url.Id && item.Url == url.Url)
                    {
                        try
                        {
                            using (var con = new SqlConnection(connectionString))
                            {
                                con.Open();
                                using (var command = new SqlCommand("DELETE FROM ItemImage WHERE ItemId = " + url.Id, con))
                                {
                                    var imageNo = command.ExecuteNonQuery();
                                    if (imageNo > 0)
                                    {
                                        Console.WriteLine("Deleted " + imageNo + " images");
                                    }
                                }
                                using (var command = new SqlCommand("DELETE FROM Tag WHERE ItemId = " + url.Id, con))
                                {
                                    var tagNo = command.ExecuteNonQuery();
                                    if (tagNo > 0)
                                    {
                                        Console.WriteLine("Deleted " + tagNo + " tag.");
                                    }
                                }
                                using (var command = new SqlCommand("DELETE FROM Item WHERE Id = " + url.Id, con))
                                {
                                    var itemNo = command.ExecuteNonQuery();
                                    if (itemNo > 0)
                                    {
                                        Console.WriteLine("Deleted " + itemNo + " duplicate item.");
                                    }
                                }
                                con.Close();
                            }
                        }
                        catch (SystemException ex)
                        {
                            Console.WriteLine("Error culling item: " + ex);
                        }
                    }
                }
            }
        }
    }
}
