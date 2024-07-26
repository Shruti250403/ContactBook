using APIPhoneBook.Data.Contract;
using APIPhoneBook.Dto;
using APIPhoneBook.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Text;
using APIPhoneBook.Service.Contract;

namespace APIPhoneBook.Service.Implementation
{
    public class AuthService:IAuthService
    {
        private readonly IAuthRepository _authRepository;
        private readonly IPasswordService _passwordService;

        public AuthService(IAuthRepository authRepository, IPasswordService passwordService)
        {
            _authRepository = authRepository;
            _passwordService = passwordService;
        }
        public ServiceResponse<string> ForgetPasswordService(ForgetDto forgetDto)
        {
            var response = new ServiceResponse<string>();

            if (forgetDto != null)
            {
                var user = _authRepository.ValidateUser(forgetDto.Username);
                if (user == null)
                {
                    response.Success = false;
                    response.Message = "Invalid username!";
                    return response;
                }

                var message = CheckPasswordStrength(forgetDto.Password);
                if (!string.IsNullOrWhiteSpace(message))
                {
                    response.Success = false;
                    response.Message = message;
                    return response;
                }

                if (forgetDto.Password != forgetDto.ConfirmPassword)
                {
                    response.Success = false;
                    response.Message = "Password and confirmation password do not match!";
                    return response;
                }

                // Create password hash and salt
                CreatePasswordHash(forgetDto.Password, out byte[] passwordHash, out byte[] passwordSalt);

                // Update user's password hash and salt
                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;

                _authRepository.UpdateUser(user); // Update the user with the new password hash and salt

                response.Success = true;
                response.Message = "Password reset successfully!";
                return response;
            }

            response.Success = false;
            response.Message = "Something went wrong, please try again later.";
            return response;
        }
        public ServiceResponse<string> RegisterUserService(RegisterDto register)
        {
            var response = new ServiceResponse<string>();

            if (register != null)
            {
                var message = CheckPasswordStrength(register.Password);
                if (!string.IsNullOrWhiteSpace(message))
                {
                    response.Success = false;

                    response.Message = message;
                    return response;
                }
                else if (_authRepository.UserExists(register.LoginId, register.Email))
                {
                    response.Success = false;

                    response.Message = "User already exists.";
                    return response;

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
                        ContactNumber = register.ContactNumber,
                        FileName = register.FileName,
                        ImageByte = register.ImageByte
                    };

                    CreatePasswordHash(register.Password, out byte[] passwordHash, out byte[] passwordSalt);
                    user.PasswordHash = passwordHash;
                    user.PasswordSalt = passwordSalt;
                    var result = _authRepository.RegisterUser(user);
                    response.Success = result;
                    response.Message = result ? "User registered successfully." : "Something went wrong.Please try again later.";
                }
            }
            return response;

        }
        public ServiceResponse<UserDto> GetUser(string userId)
        {
            var response = new ServiceResponse<UserDto>();
            var existingContact = _authRepository.GetUser(userId);
            if (existingContact != null)
            {
                var user = new UserDto()
                {
                    userId = existingContact.userId,
                    FirstName = existingContact.FirstName,
                    LastName = existingContact.LastName,
                    LoginId = userId,
                    ContactNumber = existingContact.ContactNumber,
                    Email = existingContact.Email,
                    FileName = existingContact.FileName,
                    ImageByte = existingContact.ImageByte

                };
                response.Data = user;
            }

            else
            {
                response.Success = false;
                response.Message = "Something went wrong,try after sometime";
            }
            return response;
        }
        public ServiceResponse<string> ModifyUser(User user, int userId, string email)
        {
            var response = new ServiceResponse<string>();
            var message = string.Empty;
            if (_authRepository.UserExist(userId, user.LoginId,email))
            {
                response.Success = false;
                response.Message = "User already exists.";
                return response;
            }
            var existingContact = _authRepository.GetUser(user.LoginId);
            var result = false;
            if (existingContact != null)
            {
                existingContact.FirstName = user.FirstName;
                existingContact.LastName = user.LastName;
                existingContact.ContactNumber = user.ContactNumber;
                existingContact.FileName = user.FileName;
                existingContact.ImageByte = user.ImageByte;
                result = _authRepository.UpdateUser(existingContact);
            }
            if (result)
            {
                response.Message = "User updated successfully.";
            }
            else
            {
                response.Success = false;
                response.Message = "Something went wrong,try after sometime";
            }
            return response;
        }

            public ServiceResponse<string> LoginUserService(LoginDto login)
        {
            var response = new ServiceResponse<string>();

            if (login != null)
            {

                var user = _authRepository.ValidateUser(login.Username);
                if (user == null)
                {
                    response.Success = false;
                    response.Message = "Invalid username or password!";
                    return response;
                }
                else if (!_passwordService.VerifyPasswordHash(login.Password, user.PasswordHash, user.PasswordSalt))
                {
                    response.Success = false;

                    response.Message = "Invalid username or password!";
                    return response;
                }

                string token = _passwordService.CreateToken(user);
                response.Data = token;
                response.Message = "Loged In Successfully!";
                return response;
            }
            response.Success = false;
            response.Message = "Something went wrong, please try after sometime.";

            return response;
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

        //public bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        //{
        //    using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
        //    {
        //        var computeHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        //        return computeHash.SequenceEqual(passwordHash);

        //    }
        //}
        //public string CreateToken(User user)
        //{
        //    List<Claim> claims = new List<Claim>()
        //    {
        //        new Claim(ClaimTypes.NameIdentifier,user.LoginId.ToString()),
        //         new Claim(ClaimTypes.Name,user.LoginId.ToString()),

        //    };
        //    SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_passwordService.GetSection("AppSettings:Token").Value));
        //    SigningCredentials signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
        //    SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
        //    {
        //        Subject = new ClaimsIdentity(claims),
        //        Expires = DateTime.Now.AddDays(1),
        //        SigningCredentials = signingCredentials
        //    };
        //    JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
        //    SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
        //    return tokenHandler.WriteToken(token);
        //}
    }
}
