// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

class Program
{
    static void Main()
    {
        string connectionString = GetConnectionString();
        static string GetConnectionString()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
            IConfiguration configuration = builder.Build();

            var connectionString = configuration.GetConnectionString("DefaultConnection");
            Console.WriteLine($"ConnectionString: {connectionString}");

            return connectionString;
        }
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();

            SqlTransaction transaction = connection.BeginTransaction();

            try
            {
               
                AddBook(connection, transaction, "Book1", 1, 2022, 10);
                Console.WriteLine("Book1 added successfully.");

                
                AddBook(connection, transaction, "Book2", 2, 2023, 5);
                Console.WriteLine("Book2 added successfully.");

                
                transaction.Commit();
                Console.WriteLine("Transaction committed successfully.");
            }
            catch (Exception ex)
            {
                
                Console.WriteLine($"Error: {ex.Message}");
                transaction.Rollback();
            }
            finally
            {
                
                connection.Close();
            }

            static void AddBook(SqlConnection connection, SqlTransaction transaction, string title, int authorId, int publicationYear, int copiesAvailable)
            {
                string insertQuery = "INSERT INTO Books (Title, AuthorId, PublicationYear, CopiesAvailable) VALUES (@Title, @AuthorId, @PublicationYear, @CopiesAvailable)";

                using (SqlCommand command = new SqlCommand(insertQuery, connection, transaction))
                {
                    command.Parameters.AddWithValue("@Title", title);
                    command.Parameters.AddWithValue("@AuthorId", authorId);
                    command.Parameters.AddWithValue("@PublicationYear", publicationYear);
                    command.Parameters.AddWithValue("@CopiesAvailable", copiesAvailable);
                    command.ExecuteNonQuery();
                }
            }

            
        }
    }
}




