using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Service.DTOs;
using Service.Models;
using Service.Services;

namespace BiblioTest
{
    public class UnitTestGenericService
    {
        // Test GetAllAsync method of GenericService<T>
        [Fact]
        public async Task GetAllAsync_ReturnsListOfEntities()
        {
            // Arrange
            await LoginTest();

            var service = new GenericService<Libro>();

            var result = await service.GetAllAsync();

            Assert.NotNull(result);
            Assert.IsType<List<Libro>>(result);
            Assert.NotEmpty(result);
        }

        [Fact]
        public async Task GetAllAsync_WithFilter()
        {
            // Arrange
            await LoginTest();
            var service = new GenericService<Libro>();

            var result = await service.GetAllAsync("soledad");

            Assert.NotNull(result);
            Assert.IsType<List<Libro>>(result);
            Assert.True(result.Count == 1);
            Assert.Equal("Cien Años de Soledad", result[0].Titulo);
        }

        // Agregar un libro
        [Fact]
        public async Task Test_AddAsync_AddsEntity()
        {
            // Arrange
            await LoginTest();
            var service = new GenericService<Libro>();
            var newLibro = new Libro
            {
                Titulo = "Test Libro",
                Descripcion = "Descripcion del libro de prueba",
                EditorialId = 1,
                Paginas = 100,
                AnioPublicacion = 2024,
                Portada = "portada.jpg",
                Sinopsis = "Sinopsis del libro de prueba"
            };
            // Act
            var result = await service.AddAsync(newLibro);
            // Assert
            Assert.NotNull(result);
            Assert.IsType<Libro>(result);
            Assert.Equal("Test Libro", result.Titulo);
        }

        // test delete
        [Fact]
        public async Task Test_DeleteAsync_DeletesEntity()
        {
            // Arrange
            await LoginTest();
            var service = new GenericService<Libro>();
            // Primero agregamos un libro para luego eliminarlo
            var newLibro = new Libro
            {
                Titulo = "Libro a eliminar",
                Descripcion = "Descripcion del libro a eliminar",
                EditorialId = 1,
                Paginas = 150,
                AnioPublicacion = 2023,
                Portada = "portada_eliminar.jpg",
                Sinopsis = "Sinopsis del libro a eliminar"
            };
            var addedLibro = await service.AddAsync(newLibro);
            Assert.NotNull(addedLibro);
            // Act
            var result = await service.DeleteAsync(addedLibro.Id);
            // Assert
            Assert.True(result);
        }

        // test get deleteds
        [Fact]
        public async Task Test_GetAllDeletedsAsync_ReturnsDeletedEntities()
        {
            // Arrange
            await LoginTest();
            var service = new GenericService<Libro>();
            // Act
            var result = await service.GetAllDeletedsAsync();
            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<Libro>>(result);
            Assert.NotEmpty(result);
        }

        // test update
        [Fact]
        public async Task Test_UpdateAsync_UpdatesEntity()
        {
            // Arrange
            await LoginTest();
            var service = new GenericService<Libro>();
            // Primero agregamos un libro para luego actualizarlo
            var newLibro = new Libro
            {
                Titulo = "Libro a actualizar",
                Descripcion = "Descripcion del libro a actualizar",
                EditorialId = 1,
                Paginas = 200,
                AnioPublicacion = 2022,
                Portada = "portada_actualizar.jpg",
                Sinopsis = "Sinopsis del libro a actualizar"
            };
            var addedLibro = await service.AddAsync(newLibro);
            Assert.NotNull(addedLibro);
            // Modificamos el título del libro
            addedLibro.Titulo = "Libro actualizado";
            // Act
            var result = await service.UpdateAsync(addedLibro);
            // Assert
            Assert.True(result);
        }

        // test get by id
        [Fact]
        public async Task Test_GetByIdAsync_ReturnsEntity()
        {
            // Arrange
            await LoginTest();
            var service = new GenericService<Libro>();
            // Act
            var result = await service.GetByIdAsync(1); // Suponiendo que el libro con ID 1 existe
            // Assert
            Assert.NotNull(result);
            Assert.IsType<Libro>(result);
            Assert.Equal(1, result.Id);
        }

        // test restore
        [Fact]
        public async Task Test_RestoreAsync_RestoresEntity()
        {
            // Arrange
            await LoginTest();
            var service = new GenericService<Libro>();
            // Primero agregamos un libro para luego eliminarlo y restaurarlo
            var newLibro = new Libro
            {
                Titulo = "Libro a restaurar",
                Descripcion = "Descripcion del libro a restaurar",
                EditorialId = 1,
                Paginas = 250,
                AnioPublicacion = 2021,
                Portada = "portada_restaruar.jpg",
                Sinopsis = "Sinopsis del libro a restaurar"
            };
            var addedLibro = await service.AddAsync(newLibro);
            Assert.NotNull(addedLibro);
            // Lo eliminamos
            var deleteResult = await service.DeleteAsync(addedLibro.Id);
            Assert.True(deleteResult);
            // Act
            var result = await service.RestoreAsync(addedLibro.Id);
            // Assert
            Assert.True(result);
        }

        private async Task LoginTest()
        {
            // Primero nos autenticamos para obtener el token
            var serviceAuth = new AuthService();
            var token = await serviceAuth.Login(new LoginDTO
            {
                Username = "lautiperesin@gmail.com",
                Password = "1234lauti"
            });

            GenericService<object>.token = token;
        }
    }
}
