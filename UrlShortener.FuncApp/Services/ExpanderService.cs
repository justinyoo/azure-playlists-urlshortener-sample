using System;
using System.Threading.Tasks;

using Azure.Data.Tables;

using UrlShortener.FuncApp.Models;

namespace UrlShortener.FuncApp.Services
{
    public interface IExpanderService
    {
        Task<UrlItem> ExpandAsync(string shortUrl);
    }

    public class ExpanderService : IExpanderService
    {
        private const string TableName = "UrlItems";
        private const string PartitionKey = "UrlItem";

        private readonly TableServiceClient _client;

        public ExpanderService(TableServiceClient client)
        {
            this._client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public async Task<UrlItem> ExpandAsync(string shortUrl)
        {
            await this._client.CreateTableIfNotExistsAsync(TableName);
            var table = this._client.GetTableClient(TableName);
            var entity = default(UrlItemEntity);

            try
            {
                var response = await table.GetEntityAsync<UrlItemEntity>(PartitionKey, shortUrl);
                entity = response.Value;
            }
            catch
            {
                entity = new UrlItemEntity();
            }

            return entity;
        }
    }
}
