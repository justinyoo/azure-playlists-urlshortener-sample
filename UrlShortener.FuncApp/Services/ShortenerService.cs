using System.Threading.Tasks;

namespace UrlShortener.FuncApp.Services
{
    public interface IShortenerService
    {
        Task<string> ShortenAsync(string originalUrl);
    }

    public class ShortenerService : IShortenerService
    {
        public async Task<string> ShortenAsync(string originalUrl)
        {
            return await Task.FromResult("shortened!");
        }
    }
}
