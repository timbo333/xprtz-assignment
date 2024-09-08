using Microsoft.Extensions.Caching.Memory;
using System.Diagnostics.CodeAnalysis;

namespace Domain.Services
{
    public interface ICachingService
    {
        void Remove(object key);
        T SetValue<T>(object key, T value, MemoryCacheEntryOptions? options = null);
        bool TryGetValue<T>(object key, [NotNullWhen(true)] out T? result);
    }
}