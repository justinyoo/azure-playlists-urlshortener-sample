using System;
using System.Text;
using System.Threading.Tasks;

using Azure.Data.Tables;

using UrlShortener.FuncApp.Models;

namespace UrlShortener.FuncApp.Services
{
    public interface IShortenerService
    {
        Task<UrlItem> ShortenAsync(string originalUrl);
    }

    public class ShortenerService : IShortenerService
    {
        private const string CharacterPool = "abcdefghijklmnopqrstuvwxyz0123456789";
        private const int LengthOfShortUrl = 10;
        private const string TableName = "UrlItems";
        private const string PartitionKey = "UrlItem";

        private static Random random = new Random();

        private readonly TableServiceClient _client;

        public ShortenerService(TableServiceClient client)
        {
            this._client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public async Task<UrlItem> ShortenAsync(string originalUrl)
        {
            // Get shortened URL.
            var sb = new StringBuilder();
            for (var i = 0; i < LengthOfShortUrl; i++)
            {
                var index = random.Next(LengthOfShortUrl);
                sb.Append(CharacterPool[index]);
            }
            var shortUrl = sb.ToString();

            // Store shortened URL to Table Storage.
            await this._client.CreateTableIfNotExistsAsync(TableName);
            var table = this._client.GetTableClient(TableName);
            var entity = new UrlItemEntity()
            {
                PartitionKey = PartitionKey,
                RowKey = shortUrl,
                Timestamp = DateTimeOffset.UtcNow,
                OriginalUrl = originalUrl,
                ShortUrl = shortUrl,
            };
            await table.UpsertEntityAsync(entity);

            return entity;
        }
    }
}
