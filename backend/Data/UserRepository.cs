using TesApi.Models;
using System.ComponentModel.DataAnnotations;
using MySql.Data.MySqlClient;
using System.Data;

namespace TesApi.Data
{
    public interface IUserRepository
    {
        //untuk register
        Task<bool> ExistsByEmailAsync(string email);
        Task AddUserAsync(RegisterRequestDto user);

        //Untuk login
        Task<User?> GetByEmailAsync(string email);
    }
    public class UserRepository : IUserRepository
    {
        private readonly string _connString;
        public UserRepository(IConfiguration config)
        {
            _connString = config.GetConnectionString("DefaultConnection")
            ?? throw new ArgumentNullException("Connection string tidak ditemukan");
        }

        public async Task<bool> ExistsByEmailAsync(string email)
        {
            await using var conn = new MySqlConnection(_connString);
            await conn.OpenAsync();
            await using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT COUNT(*) FROM users WHERE email = @email";
            cmd.Parameters.AddWithValue("@email", email);
            var count = Convert.ToInt32(await cmd.ExecuteScalarAsync());
            return count > 0;
        }

        public async Task AddUserAsync(RegisterRequestDto user)
        {
            await using var conn = new MySqlConnection(_connString);
            await conn.OpenAsync();
            await using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                INSERT INTO users (username, email, password)
                VALUES (@username, @email, @pwd);
            ";
            cmd.Parameters.AddWithValue("@username", user.Username);
            cmd.Parameters.AddWithValue("@email", user.Email);
            cmd.Parameters.AddWithValue("@pwd", user.Password);
            await cmd.ExecuteNonQueryAsync();
        }
        
        //untuk login
          public async Task<User?> GetByEmailAsync(string email)
        {
            await using var conn = new MySqlConnection(_connString);
            await conn.OpenAsync();
            await using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                SELECT id, username, email, password, created_at
                FROM users
                WHERE email = @email
                LIMIT 1;
            ";
            cmd.Parameters.AddWithValue("@email", email);
            await using var reader = await cmd.ExecuteReaderAsync();
            if (!reader.Read()) return null;

            return new User
            {
                Id = reader.GetInt32("id"),
                Username = reader.GetString("username"),
                Email = reader.GetString("email"),
                Password = reader.GetString("password"),
                CreatedAt = reader.GetDateTime("created_at")
            };
        }
    }
}