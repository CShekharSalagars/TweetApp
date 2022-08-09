using AutoMapper;
using com.tweetapp.Entity;
using com.tweetapp.Models.Models;
using com.tweetapp.Services.DTOS;
using com.tweetapp.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace com.tweetapp.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly TweetAppContext dbContext;
        private readonly IMapper mapper;

        public readonly IConfiguration config;

        public AuthService(TweetAppContext _dbContext, IMapper _mapper,IConfiguration _config)
        {
            dbContext = _dbContext;
            mapper = _mapper;
            config = _config;
        }

        /// <summary>
        /// Function will register user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<ServiceResponse<int>> RegisterUser(RegisterUserDto user)
        {
            var serviceResponse = new ServiceResponse<int>();
            try
            {
                if (await UserExists(user.UserName))
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = $"User with username {user.UserName} is already exist";
                    return serviceResponse;
                }
                UserModel userEntity = mapper.Map<UserModel>(user);
                CreatePasswordHash(user.Password, out byte[] passwordHash, out byte[] passwordSalt);
                userEntity.PasswordSalt = passwordSalt;
                userEntity.PasswordHash = passwordHash;
                dbContext.User.Add(userEntity);
                await dbContext.SaveChangesAsync();
                serviceResponse.Data = userEntity.UserId;
                serviceResponse.Message = "User Created";
                return serviceResponse;
            }
            catch (Exception ex)
            {
                serviceResponse.Data = 0;
                serviceResponse.Message = "User creation failed";
                serviceResponse.Success = false;
                return serviceResponse;
            }
        }

        /// <summary>
        /// Funtion will authenticate user
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ServiceResponse<string>> Login(string userName, string password)
        {
            var serviceResponse = new ServiceResponse<string>();
            try
            {
                var user = await dbContext.User.FirstOrDefaultAsync(u => userName.ToLower().Equals(u.UserName.ToLower()));
                if (user == null)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "User not found";
                    return serviceResponse;
                }
                else if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "Username or password is wrong";
                    return serviceResponse;
                }
                serviceResponse.Data =  CreateToken(user);
                return serviceResponse;

            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = "Something went wrong";
                return serviceResponse;
            }
        }

        /// <summary>
        /// Reset user password
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ServiceResponse<string>> ForgotPassword(string userName, string password)
        {
            var serviceResponse = new ServiceResponse<string>();
            var userEntity = await dbContext.User.FirstOrDefaultAsync(u => userName.ToLower().Equals(u.UserName.ToLower()));
            if (userEntity == null)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = "User not found";
                return serviceResponse;
            }

            CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);
            userEntity.PasswordSalt = passwordSalt;
            userEntity.PasswordHash = passwordHash;
            await dbContext.SaveChangesAsync();
            serviceResponse.Message = "Password changed successfully";
            return serviceResponse;
        }



        public async Task<ServiceResponse<UserDto>> GetByName(string userName)
        {
            var servicesResponse = new ServiceResponse<UserDto>();
            var userEntiries = await dbContext.User.Where(x => x.UserName == userName).FirstOrDefaultAsync();
            servicesResponse.Success = userEntiries != null;
            servicesResponse.Data = mapper.Map<UserDto>(userEntiries);
            return servicesResponse;
        }


        #region[Private functions]
        /// <summary>
        /// Create Hash password
        /// </summary>
        /// <param name="password"></param>
        /// <param name="passwordHash"></param>
        /// <param name="passwordSalt"></param>
        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        /// <summary>
        /// Check if user name is exist
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public async Task<bool> UserExists(string userName)
        {
            return await dbContext.User.AnyAsync(x => x.UserName == userName);
        }

        /// <summary>
        /// Function will verify the hash
        /// </summary>
        /// <param name="password"></param>
        /// <param name="passwordHash"></param>
        /// <param name="passwordSalt"></param>
        /// <returns></returns>
        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computeHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computeHash.SequenceEqual(passwordHash);
            }
        }

        /// <summary>
        /// Function will create Auth token
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private string CreateToken(UserModel user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Name, user.UserName)
            };

            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes(config.GetSection("AppSettings:SecretKey").Value));

            SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        #endregion
    }
}
