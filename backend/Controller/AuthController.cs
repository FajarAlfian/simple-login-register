using Microsoft.AspNetCore.Mvc;
using TesApi.Data;
using TesApi.Models;
using BCrypt.Net;

namespace TesApi.Controllers {
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _repo;
        public AuthController(IUserRepository repo)
        {
            _repo = repo;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequestDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Username) ||
                string.IsNullOrWhiteSpace(dto.Email) ||
                string.IsNullOrWhiteSpace(dto.Password))
            {
                return BadRequest(new { message = "Semua field harus diisi" });
            }

            if (await _repo.ExistsByEmailAsync(dto.Email))
            {
                return Conflict(new { message = "Email sudah terdaftar" });
            }
            var hash = BCrypt.Net.BCrypt.HashPassword(dto.Password);
            var newUser = new RegisterRequestDto
            {
                Username = dto.Username,
                Email = dto.Email,
                Password = hash
            };

            await _repo.AddUserAsync(newUser);
            return Created(string.Empty, new { message = "User registered successfully" });
        }
        
        //Untuk login
          [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto dto) {
            if (string.IsNullOrWhiteSpace(dto.Email) || string.IsNullOrWhiteSpace(dto.Password)) {
                return BadRequest(new { message = "Email dan password diperlukan" });
            }

            var user = await _repo.GetByEmailAsync(dto.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.Password)) {
                return Unauthorized(new { message = "Email atau password salah" });
            }

            return Ok(new { message = "Login berhasil", user = new { user.Id, user.Username, user.Email } });
        }
    }
}