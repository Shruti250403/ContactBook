﻿using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Text;
using ClientPhoneBook.Data.Contract;
using ClientPhoneBook.Services.Contract;

namespace ClientPhoneBook.Services.Implementation
{
    public class AuthService:IAuthService
    {
        private readonly IAuthRepository _authRepository;
        private readonly IConfiguration _configuration;

        public AuthService(IAuthRepository authRepository, IConfiguration configuration)
        {
            _authRepository = authRepository;
            _configuration = configuration;
        }
        public string RegisterUserService(RegisterViewModel register)
        {
            var message = string.Empty;
            if (register != null)
            {
                message = CheckPasswordStrength(register.Password);
                if (!string.IsNullOrWhiteSpace(message))
                {
                    return message;
                }
                else if (_authRepository.UserExists(register.LoginId, register.Email))
                {
                    message = "Contact already exists.";
                    return message;
                }
                else
                {
                    // Save user
                    User user = new User()
                    {
                        FirstName = register.FirstName,
                        LastName = register.LastName,
                        Email = register.Email,
                        LoginId = register.LoginId,
                        Phone = register.ContactNumber,
                        Company = register.Company,
                    };

                    CreatePasswordHash(register.Password, out byte[] passwordHash, out byte[] passwordSalt);
                    user.PasswordHash = passwordHash;
                    user.PasswordSalt = passwordSalt;
                    var result = _authRepository.RegisterUser(user);
                    message = result ? string.Empty : "Something went wrong.Please try again later.";
                }
            }
            return message;

        }

        public string LoginUserService(LoginViewModel login)
        {
            string message = "Invalid username or password";
            if (login != null)
            {

                var user = _authRepository.ValidateUser(login.Username);
                if (user == null)
                {
                    return message;
                }
                else if (!VerifyPasswordHash(login.Password, user.PasswordHash, user.PasswordSalt))
                {

                    return message;
                }
                string token = CreateToken(user);
                return token;
            }
            message = "Something went wrong.Try again later.";
            return message;

        }

        private string CheckPasswordStrength(string password)
        {
            StringBuilder stringBuilder = new StringBuilder();
            if (password.Length < 8)
            {
                stringBuilder.Append("Mininum password length should be 8" + Environment.NewLine);
            }
            if (!(Regex.IsMatch(password, "[a-z]") && Regex.IsMatch(password, "[A-Z]") && Regex.IsMatch(password, "[0-9]")))
            {
                stringBuilder.Append("Password should be apphanumeric" + Environment.NewLine);
            }
            if (!Regex.IsMatch(password, "[<,>,@,!,#,$,%,^,&,*,*,(,),_,]"))
            {
                stringBuilder.Append("Password should contain special characters" + Environment.NewLine);
            }

            return stringBuilder.ToString();
        }
        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        public bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computeHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computeHash.SequenceEqual(passwordHash);

            }
        }
        public string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier,user.ContactId.ToString()),
                 new Claim(ClaimTypes.Name,user.LoginId.ToString()),

            };
            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value));
            SigningCredentials signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = signingCredentials
            };
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
