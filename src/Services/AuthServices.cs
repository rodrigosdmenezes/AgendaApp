using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AgendaApp.src.Dtos;
using AgendaApp.src.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Identity;
using AgendaApp.Infra.Data;
using AgendaApp.Entities;

namespace AgendaApp.src.Services
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly PasswordHasher<object> _passwordHasher;

        public AuthService(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
            _passwordHasher = new PasswordHasher<object>();
        }

        public async Task<AuthResult> RegisterPacienteAsync(RegisterPacienteRequest request)
        {
            // Verificar se email já está em uso
            if (await _context.Pacientes.AnyAsync(p => p.Email == request.Email))
                return new AuthResult { Success = false, Message  =  "Email já cadastrado." };

            var paciente = new Paciente
            {
                Nome = request.Nome,
                Email = request.Email,
                // outros campos...
            };

            paciente.SenhaHash = _passwordHasher.HashPassword(null, request.Senha);

            _context.Pacientes.Add(paciente);
            await _context.SaveChangesAsync();

            var token = GenerateJwtToken(paciente.Id.ToString(), "Paciente");

            return new AuthResult { Success = true, Token = token };
        }

        public async Task<AuthResult> RegisterMedicoAsync(RegisterMedicoRequest request)
        {
            // Verificar se email ou CRM já estão em uso
            if (await _context.Medicos.AnyAsync(m => m.Email == request.Email))
                return new AuthResult { Success = false, Message  =  "Email já cadastrado." };

            if (await _context.Medicos.AnyAsync(m => m.Crm == request.Crm))
                return new AuthResult { Success = false, Message  = "CRM já cadastrado." };

            var medico = new Medico
            {
                Nome = request.Nome,
                Email = request.Email,
                Crm = request.Crm,
                // outros campos...
            };

            medico.SenhaHash = _passwordHasher.HashPassword(null, request.Senha);

            _context.Medicos.Add(medico);
            await _context.SaveChangesAsync();

            var token = GenerateJwtToken(medico.Id.ToString(), "Medico");

            return new AuthResult { Success = true, Token = token };
        }

        public async Task<AuthResult> LoginAsync(LoginRequest request)
        {
            // Procurar usuário (medico ou paciente) pelo email
            var medico = await _context.Medicos.FirstOrDefaultAsync(m => m.Email == request.Email);
            if (medico != null)
            {
                var result = _passwordHasher.VerifyHashedPassword(null, medico.SenhaHash, request.Senha);
                if (result == PasswordVerificationResult.Success)
                {
                    var token = GenerateJwtToken(medico.Id.ToString(), "Medico");
                    return new AuthResult { Success = true, Token = token };
                }
            }

            var paciente = await _context.Pacientes.FirstOrDefaultAsync(p => p.Email == request.Email);
            if (paciente != null)
            {
                var result = _passwordHasher.VerifyHashedPassword(null, paciente.SenhaHash, request.Senha);
                if (result == PasswordVerificationResult.Success)
                {
                    var token = GenerateJwtToken(paciente.Id.ToString(), "Paciente");
                    return new AuthResult { Success = true, Token = token };
                }
            }

            return new AuthResult { Success = false, Errors = new[] { "Email ou senha inválidos." } };
        }

        // Método privado para gerar token JWT
        private string GenerateJwtToken(string userId, string role)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId),
                new Claim(ClaimTypes.Role, role),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(2),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
