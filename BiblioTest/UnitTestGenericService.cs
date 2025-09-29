using Microsoft.Extensions.Configuration;
using Service.DTOs;
using Service.Models;
using Service.Services;
using System.Text;
using System.Text.Json;

namespace BiblioTestProject
{
    public class UnitTestAuthService
    {
        [Fact]
        private async Task LoginTest()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            var serviceAuth = new AuthService();
            var token = await serviceAuth.Login(new LoginDTO
            {
                Username = "sofiimellano@gmail.com",
                Password = "123457"
            });
        }

        [Fact]
        public async Task ResetPasswordTest()
        {
            await LoginTest();
            var serviceAuth = new AuthService();
            var loginDTO = new LoginDTO
            {
                Username = "sofiimellano@gmail.com",
                Password = "no hace falta"

            };
            var result = await serviceAuth.ResetPassword(loginDTO);
            Assert.True(result);
        }
    }
}