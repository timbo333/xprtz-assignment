using Domain.Models;

namespace Domain.Services
{
    public class CachedShowService(IShowService showService, ICachingService cachingService) : IShowService
    {
        private readonly ICachingService _cachingService = cachingService;
        private readonly IShowService _showService = showService;

        private const string _allShowsKey = "AllShows";

        public Task CreateAsync(Show show)
        {
            _cachingService.Remove(_allShowsKey);

            return _showService.CreateAsync(show);
        } 

        public Task<int> DeleteAsync(int id)
        {
            // Reset cache as we know that we are changing the item.
            if (_cachingService.TryGetValue(id, out Show? cachedShow))
            {
                _cachingService.Remove(cachedShow!.Name);
                _cachingService.Remove(_allShowsKey);
            }

            return _showService.DeleteAsync(id);
        }

        public async Task<List<Show>> GetAsync()
        {
            if (_cachingService.TryGetValue(_allShowsKey, out List<Show>? cachedShows))
                return cachedShows!;

            var allShows = await _showService.GetAsync();
            _cachingService.SetValue(_allShowsKey, allShows);

            return allShows;
        }

        public async Task<Show?> GetByNameAsync(string name)
        {
            if (_cachingService.TryGetValue(name, out Show? cachedShow)) 
                return cachedShow;

            if (_cachingService.TryGetValue(_allShowsKey, out List<Show>? allCachedShows))
            {
                cachedShow = allCachedShows!.FirstOrDefault(x => x.Name == name);
                if (cachedShow is not null)
                    return cachedShow;
            }

            var show = await _showService.GetByNameAsync(name);
            if (show is not null)
                _cachingService.SetValue(show.Name, show);

            return show;
        }

        public Task<int?> UpdateAsync(int id, Show show)
        {
            // Reset cache as we know that we are changing the item.
            if (_cachingService.TryGetValue(id, out Show? cachedShow))
            {
                _cachingService.Remove(cachedShow!.Name);
                _cachingService.Remove(_allShowsKey);
            }

            return _showService.UpdateAsync(id, show);
        }
    }
}
