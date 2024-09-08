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
    public class CachedShowServiceTest
    {
        private readonly IShowService _showServiceMock;
        private readonly ICachingService _cachingServiceMock;
        private readonly IShowService _sut;

        public CachedShowServiceTest()
        {
            _cachingServiceMock = Substitute.For<ICachingService>();
            _showServiceMock = Substitute.For<IShowService>();
            _sut = new CachedShowService(_showServiceMock, _cachingServiceMock);
        }

        [Fact]
        public async Task CreateAsync()
        {
            // Given
            var show = new Show { Name = "Test" };

            // When
            await _sut.CreateAsync(show);

            // Then
            _cachingServiceMock.Received().Remove("AllShows");
            await _showServiceMock.Received().CreateAsync(show);
        }

        [Fact]
        public async Task DeleteAsync()
        {
            // Given
            var id = 1;
            var show = new Show { Id = 1, Name = "test" };

            _cachingServiceMock.TryGetValue(id, out Arg.Any<Show>()).Returns(false);

            // When
            await _sut.DeleteAsync(id);

            // Then
            await _showServiceMock.Received().DeleteAsync(id);
            _cachingServiceMock.DidNotReceive().Remove(show.Name);
            _cachingServiceMock.DidNotReceive().Remove("AllShows");
        }

        [Fact]
        public async Task DeleteAsync_InCache()
        {
            // Given
            var id = 1;
            var show = new Show { Id = 1, Name = "test" };

            _cachingServiceMock.TryGetValue(id, out Arg.Any<Show>()).Returns(x =>
            {
                x[1] = show;
                return true;
            });

            // When
            await _sut.DeleteAsync(id);

            // Then
            await _showServiceMock.Received().DeleteAsync(id);
            _cachingServiceMock.Received().Remove(show.Name);
            _cachingServiceMock.Received().Remove("AllShows");
        }

        [Fact]
        public async Task UpdateAsync()
        {
            // Given
            var id = 1;
            var show = new Show { Id = 1, Name = "test" };

            _cachingServiceMock.TryGetValue(id, out Arg.Any<Show>()).Returns(false);

            // When
            await _sut.UpdateAsync(id, show);

            // Then
            await _showServiceMock.Received().UpdateAsync(id, show);
            _cachingServiceMock.DidNotReceive().Remove(show.Name);
            _cachingServiceMock.DidNotReceive().Remove("AllShows");
        }

        [Fact]
        public async Task UpdateAsync_InCache()
        {
            // Given
            var id = 1;
            var show = new Show { Id = id };

            _cachingServiceMock.TryGetValue(id, out Arg.Any<Show>()).Returns(x =>
            {
                x[1] = show;
                return true;
            });

            // When
            await _sut.UpdateAsync(id, show);

            // Then
            await _showServiceMock.Received().UpdateAsync(id, show);
            _cachingServiceMock.Received().Remove(show.Name);
            _cachingServiceMock.Received().Remove("AllShows");
        }

        [Fact]
        public async Task GetAsync()
        {
            // Given
            _cachingServiceMock.TryGetValue("AllShows", out Arg.Any<List<Show>>()).Returns(false);

            // When
            await _sut.GetAsync();

            // Then
            await _showServiceMock.Received().GetAsync();
        }

        [Fact]
        public async Task GetAsync_InCache()
        {
            // Given
            _cachingServiceMock.TryGetValue("AllShows", out Arg.Any<List<Show>>()).Returns(true);

            // When
            await _sut.GetAsync();

            // Then
            await _showServiceMock.DidNotReceive().GetAsync();
        }

        [Fact]
        public async Task GetByNameAsync()
        {
            // Given
            var name = "name";
            var show = new Show { Id = 1, Name = name };

            _cachingServiceMock.TryGetValue(name, out Arg.Any<string>()).Returns(false);
            _cachingServiceMock.TryGetValue("AllShows", out Arg.Any<List<Show>>()).Returns(false);

            _showServiceMock.GetByNameAsync(name).Returns(show);

            // When
            await _sut.GetByNameAsync(name);

            // Then
            await _showServiceMock.Received().GetByNameAsync(name);
            _cachingServiceMock.Received().SetValue(name, show);
        }

        [Fact]
        public async Task GetByNameAsync_InByNameCache()
        {
            // Given
            var name = "name";
            var show = new Show { Id = 1, Name = name };

            _cachingServiceMock.TryGetValue(name, out Arg.Any<Show?>()).Returns(true);

            // When
            await _sut.GetByNameAsync(name);

            // Then
            await _showServiceMock.DidNotReceive().GetByNameAsync(name);
            _cachingServiceMock.DidNotReceive().TryGetValue("AllShows", out Arg.Any<List<Show>?>());
        }

        [Fact]
        public async Task GetByNameAsync_InAllShowsCache()
        {
            // Given
            var name = "name";
            var show = new Show { Id = 1, Name = name };

            _cachingServiceMock.TryGetValue(name, out Arg.Any<string>()).Returns(false);
            _cachingServiceMock.TryGetValue("AllShows", out Arg.Any<List<Show>>()).Returns(x =>
            {
                x[1] = new List<Show> { show };
                return true;
            });

            // When
            await _sut.GetByNameAsync(name);

            // Then
            await _showServiceMock.DidNotReceive().GetByNameAsync(name);
        }
    }
}
