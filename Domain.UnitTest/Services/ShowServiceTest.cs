using Domain.Models;
using Domain.Repositories;
using Domain.Services;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.UnitTest.Services
{
    public class ShowServiceTest
    {
        private readonly IShowRepository _repositoryMock;
        private readonly IShowService _sut;

        public ShowServiceTest()
        {

            _repositoryMock = Substitute.For<IShowRepository>();
            _sut = new ShowService(_repositoryMock);
        }

        [Fact]
        public async Task CreateAsync()
        {
            // Given
            var show = new Show { Name = "Test" };

            // When
            await _sut.CreateAsync(show);

            // Then
            await _repositoryMock.Received().CreateAsync(show);
        }

        [Fact]
        public async Task DeleteAsync()
        {
            // Given
            var id = 1;

            // When
            await _sut.DeleteAsync(id);

            // Then
            await _repositoryMock.Received().DeleteAsync(id);
        }

        [Fact]
        public async Task UpdateAsync()
        {
            // Given
            var id = 1;
            var show = new Show { Id = id };

            // When
            await _sut.UpdateAsync(id, show);

            // Then
            await _repositoryMock.Received().UpdateAsync(id, show);
        }

        [Fact]
        public async Task GetAsync()
        {
            // Given

            // When
            await _sut.GetAsync();

            // Then
            await _repositoryMock.Received().GetAsync();
        }

        [Fact]
        public async Task GetByNameAsync()
        {
            // Given
            var name = "name";

            // When
            await _sut.GetByNameAsync(name);

            // Then
            await _repositoryMock.Received().GetByNameAsync(name);
        }
    }
}
