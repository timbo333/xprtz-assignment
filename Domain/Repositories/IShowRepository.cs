using Domain.Models;

namespace Domain.Repositories
{
    public interface IShowRepository
    {
        Task CreateAsync(Show show);
        Task<int> DeleteAsync(int id);
        Task<Show?> GetByNameAsync(string name);
        Task<List<Show>> GetAsync();
        Task<int?> UpdateAsync(int id, Show show);
    }
}