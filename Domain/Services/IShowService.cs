using Domain.Models;

namespace Domain.Services
{
    public interface IShowService
    {
        Task CreateAsync(Show show);
        Task<int> DeleteAsync(int id);
        Task<List<Show>> GetAsync();
        Task<Show?> GetByNameAsync(string name);
        Task<int?> UpdateAsync(int id, Show show);
    }
}