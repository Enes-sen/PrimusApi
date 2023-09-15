using Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace DataAccess.Concrete.EntityFramework.Contexts
{
    public class ApiContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Define your connection string
            string connectionString = "Host=dpg-ck04et95mpss73cof99g-a.oregon-postgres.render.com;Port=5432;Database=primusdb;Username=primusdb_user;Password=BVJQvXq6jXfacoV4iYfdJ1A2LkPpj9O7";

            // Use NpgsqlConnection to handle exceptions
            NpgsqlConnection connection = new(connectionString);
            try
            {
                connection.Open();
                optionsBuilder.UseNpgsql(connection);
            }
            catch (NpgsqlException ex)
            {
                // Handle the exception, e.g., log the error or display a user-friendly message
                Console.WriteLine($"Error connecting to the database: {ex.Message}");
            }
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Alien> Aliens { get; set; }
    }
}
