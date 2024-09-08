using Domain.Models;
using Domain.Repositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class ShowRepository : IShowRepository
    {
        private readonly ShowDbContext _context;

        public ShowRepository(ShowDbContext context)
        {
            _context = context;
        }

        public Task<List<Show>> GetAsync() => _context.Show.ToListAsync();
        public Task<Show?> GetByNameAsync(string name) => _context.Show.FirstOrDefaultAsync(x => x.Name == name);

        public async Task CreateAsync(Show show)
        {
            _context.Add(show);
            await _context.SaveChangesAsync();
        }

        public async Task<int?> UpdateAsync(int id, Show show)
        {
            show.Id = id;

            try
            {
                _context.Update(show);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                // TODO: make something nice out of this.
                throw;
            }

            return show.Id;
        }

        public async Task<int> DeleteAsync(int id)
        {
            var show = await _context.Show.FindAsync(id);
            if (show != null)
                _context.Show.Remove(show);

            await _context.SaveChangesAsync();
            return id;
        }
    }
}
