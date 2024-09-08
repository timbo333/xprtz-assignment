using Domain.Models;
using Domain.Repositories;

namespace Domain.Services
{
    public class ShowService(IShowRepository repository) : IShowService
    {
        private readonly IShowRepository _repository = repository;

        public Task<List<Show>> GetAsync() => _repository.GetAsync();

        public Task<Show?> GetByNameAsync(string name) => _repository.GetByNameAsync(name);

        public Task CreateAsync(Show show) => _repository.CreateAsync(show);

        public Task<int?> UpdateAsync(int id, Show show) => _repository.UpdateAsync(id, show);

        public Task<int> DeleteAsync(int id) => _repository.DeleteAsync(id);
    }
}
