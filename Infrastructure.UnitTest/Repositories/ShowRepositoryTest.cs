using Domain.Models;
using Domain.Repositories;
using EntityFrameworkCore.Testing.NSubstitute;
using FluentAssertions;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using System.Linq.Expressions;

namespace Infrastructure.UnitTest.Repositories
{
    public class ShowRepositoryTest
    {
        private readonly IShowRepository _sut;
        private readonly ShowDbContext _dbContextMock;

        public ShowRepositoryTest()
        {
            var options = new DbContextOptionsBuilder<ShowDbContext>()
                .UseInMemoryDatabase("fakeDb")
                .Options;

            _dbContextMock = Substitute.For<ShowDbContext>(options);

            _sut = new ShowRepository(_dbContextMock);
        }

        [Fact]
        public async Task CreateAsync()
        {
            // Given
            var show = new Show { Name = "Tim" };

            // When
             await _sut.CreateAsync(show);

            // Then
            _dbContextMock.Received().Add(show);
            await _dbContextMock.Received().SaveChangesAsync();
        }

        [Fact]
        public async Task DeleteAsync()
        {
            var dbSetMock = Substitute.For<DbSet<Show>>();
            _dbContextMock.Show = dbSetMock;

            // Given
            var id = 1;
            var show = new Show { Id = id };

            dbSetMock.FindAsync(id).Returns(show);

            // When
            await _sut.DeleteAsync(id);

            // Then
            dbSetMock.Received().Remove(show);
            await _dbContextMock.Received().SaveChangesAsync();
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
            _dbContextMock.Received().Update(show);
            await _dbContextMock.Received().SaveChangesAsync();
        }

        [Fact]
        public async Task UpdateAsync_IdAndShowId_DoNotMatch_Fails()
        {
            // Given
            var id = 1;
            var show = new Show { Id = 2 };

            // When
            var result = await _sut.UpdateAsync(id, show);

            // Then
            result.Should().BeNull();
            _dbContextMock.DidNotReceive().Update(show);
            await _dbContextMock.DidNotReceive().SaveChangesAsync();
        }
    }
}
