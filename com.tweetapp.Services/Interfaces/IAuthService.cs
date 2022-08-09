using com.tweetapp.Models.Models;
using com.tweetapp.Services.DTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.tweetapp.Services.Interfaces
{
    public interface IAuthService
    {
        /// <summary>
        /// Register new user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<ServiceResponse<int>> RegisterUser(RegisterUserDto user);

        /// <summary>
        /// Authenticate and login user
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Task<ServiceResponse<string>> Login(string userName, string password);

        /// <summary>
        /// reset password
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Task<ServiceResponse<string>> ForgotPassword(string userName, string password);

        /// <summary>
        /// Get user by username
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        Task<ServiceResponse<UserDto>> GetByName(string userName);
    }
}
