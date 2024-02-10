using FirstWebApiProject_E_Commerce_.APIModels;
using Microsoft.AspNetCore.Mvc;

namespace FirstWebApiProject_E_Commerce_.Services
{
    public interface IUserService
    {
        Task<ApiResponse<SignUpModel.Response>>SignUpAsync(SignUpModel.Request model);
        Task<ApiResponse<SignInModel.Response>> SignInAsync(SignInModel.Request model);
        Task<ApiResponse<CheckEmailModel.Response>> CheckEmailAsync(CheckEmailModel.Request model);
        Task<ApiResponse> AddRoleAsync(AddRoleModel.Requset model);
        Task<ApiResponse<SignInModel.Response>> RefreshTokenAsync(string token);
        Task<bool> RevokeTokenAsync(string token);
    }   
}
