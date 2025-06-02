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
                return new AuthResult { Success = false, Message = "Email já cadastrado." };
            if (await _context.Medicos.AnyAsync(m => m.Cpf == request.Cpf))
                return new AuthResult { Success = false, Message = "CPF já cadastrado." };

            var paciente = new Paciente
            {
                Nome = request.Nome,
                Cpf = request.Cpf,
                Email = request.Email,
                Senha = request.Senha
            };

            paciente.Senha = _passwordHasher.HashPassword(null, request.Senha);

            _context.Pacientes.Add(paciente);
            await _context.SaveChangesAsync();

            var token = GenerateJwtToken(paciente.Id, paciente.Email, "Paciente");

            return new AuthResult { Success = true, Token = token };
        }

        public async Task<AuthResult> RegisterMedicoAsync(RegisterMedicoRequest request)
        {
            if (await _context.Medicos.AnyAsync(m => m.Email == request.Email))
                return new AuthResult { Success = false, Message = "Email já cadastrado." };

            if (await _context.Medicos.AnyAsync(m => m.Crm == request.Crm))
                return new AuthResult { Success = false, Message = "CRM já cadastrado." };
            if (await _context.Medicos.AnyAsync(m => m.Cpf == request.Cpf))
                return new AuthResult { Success = false, Message = "CPF já cadastrado." };

            var medico = new Medico
            {
                Nome = request.Nome,
                Cpf = request.Cpf,
                Email = request.Email,
                Crm = request.Crm,
                Especialidade = request.Especialidade
            };

            medico.Senha = _passwordHasher.HashPassword(null, request.Senha);

            _context.Medicos.Add(medico);
            await _context.SaveChangesAsync();

            var token = GenerateJwtToken(medico.Id, medico.Email, "Medico");

            return new AuthResult { Success = true, Token = token };
        }

        public async Task<AuthResult> LoginAsync(LoginRequest request)
        {
            // Procurar usuário (medico ou paciente) pelo email
            var medico = await _context.Medicos.FirstOrDefaultAsync(m => m.Email == request.Email);
            if (medico != null)
            {
                var result = _passwordHasher.VerifyHashedPassword(null, medico.Senha, request.Senha);
                if (result == PasswordVerificationResult.Success)
                {
                    var token = GenerateJwtToken(medico.Id, medico.Email, "Medico");
                    return new AuthResult { Success = true, Token = token };
                }
            }

            var paciente = await _context.Pacientes.FirstOrDefaultAsync(p => p.Email == request.Email);
            if (paciente != null)
            {
                var result = _passwordHasher.VerifyHashedPassword(null, paciente.Senha, request.Senha);
                if (result == PasswordVerificationResult.Success)
                {
                    var token = GenerateJwtToken(paciente.Id, paciente.Email, "Paciente");
                    return new AuthResult { Success = true, Token = token };
                }
            }

            return new AuthResult { Success = false, Message = "Email ou senha inválidos." };
        }

        // Método privado para gerar token JWT
        private string GenerateJwtToken(Guid userId, string email, string role)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
        new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
        new Claim(JwtRegisteredClaimNames.Email, email),
        new Claim(ClaimTypes.Role, role),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
    };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
