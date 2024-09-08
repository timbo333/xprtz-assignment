using Microsoft.Extensions.Caching.Memory;
using System.Diagnostics.CodeAnalysis;

namespace Domain.Services
{
    public class CachingService(IMemoryCache cache) : ICachingService
    {
        private readonly IMemoryCache _cache = cache;
        private readonly MemoryCacheEntryOptions _options = new()
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
        };

        public T SetValue<T>(object key, T value, MemoryCacheEntryOptions? options = null) => _cache.Set(key, value, options ?? _options);
        public bool TryGetValue<T>(object key, [NotNullWhen(true)] out T? result) => _cache.TryGetValue(key, out result);
        public void Remove(object key) => _cache.Remove(key);
    }
}
