using Microsoft.Extensions.Configuration;
using Service.DTOs;
using Service.Models;
using Service.Services;
using System.Text;
using System.Text.Json;

namespace BiblioTestProject
{
    public class UnitTestUsuarioService
    {
        [Fact]
        public async Task Test_GetAlAsync_ReturnListOfEntities()
        {
            //Arrange

            await LoginTest();
            //Act 
            var service = new UsuarioService();
            var result = await service.GetAllAsync();


            //Assert
            Assert.NotNull(result);
            Assert.IsType<List<Usuario>>(result);
            Assert.True(result.Count > 0);
        }

        private async Task LoginTest()
        {
            var serviceAuth = new AuthService();
            var token = await serviceAuth.Login(new LoginDTO
            {
                Username = "lautaroperesin@gmail.com",
                Password = "1234567"
            });
            Console.WriteLine($"Token>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>: " + token);
        }

        [Fact]
        public async Task Test_GetByEmailAsync_ReturnEntity()
        {
            await LoginTest();
            //Arrange
            var service = new UsuarioService();
            //Act 
            var result = await service.GetByEmailAsync("lautaroperesin@gmail.com");
            //Assert
            Assert.NotNull(result);
            Assert.IsType<Usuario>(result);
            Assert.Equal("lautaroperesin@gmail.com", result.Email);
        }
    }
}