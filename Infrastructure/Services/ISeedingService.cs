using Domain.Models;

namespace Infrastructure.Services
{
    public interface ISeedingService
    {
        IAsyncEnumerable<Show> GetShowsAsync();
    }
}