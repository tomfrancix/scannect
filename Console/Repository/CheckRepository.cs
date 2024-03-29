﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Scannect.Models;

namespace ScannectConsole.Repository
{
    public class CheckRepository
    {
        private static readonly string connectionString = "Server=(localdb)\\mssqllocaldb;Database=ScannectDb;Trusted_Connection=True;MultipleActiveResultSets=true";
        
        public static void SaveItem(Item item)
        {
            var connection = new SqlConnection(connectionString);

            connection.Open();
            try
            {
                item.Snippet = item.Snippet.Replace("'", "");
            }
            catch
            {
                Console.WriteLine("Error with snippet.");
            }

            var builder = new StringBuilder();
            builder.Append("INSERT INTO Item (Url, WebsiteUrl, Title, Snippet, DateCreated, Hits, Ranking, Category, Author) VALUES ");
            builder.Append("('" + item.Url + "','" + item.WebsiteUrl + "','" + item.Title + "','" + item.Snippet + "','" + DateTime.UtcNow + "','" + 0 + "','" + 0 + "','" + item.Category + "','" + item.Author + "')");
            var sql = builder.ToString();

            try
            {
                using (var command = new SqlCommand(sql, connection))
                {
                    command.ExecuteNonQuery();

                    Console.WriteLine("Query Executed.");
                }
            }
            catch
            {
                Console.WriteLine("Failed to save item.");
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
                try
                {
                    var result = command.ExecuteReader();

                    if (result.HasRows)
                    {
                        connection.Close();
                        return true;
                    }
                }
                catch
                {
                    Console.WriteLine("Cannot read encode URLs");
                }
            }
            connection.Close();
            return false;
        }
    }
}
