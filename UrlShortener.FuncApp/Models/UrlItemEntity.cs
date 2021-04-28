using System;

using Azure;
using Azure.Data.Tables;

namespace UrlShortener.FuncApp.Models
{
    public class UrlItem
    {
        public UrlItem(UrlItemEntity entity)
        {
            this.OriginalUrl = entity.OriginalUrl;
            this.ShortUrl = entity.ShortUrl;
        }

        public string OriginalUrl { get; set; }
        public string ShortUrl { get; set; }
    }

    public class UrlItemEntity : ITableEntity
    {
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }
        public string OriginalUrl { get; set; }
        public string ShortUrl { get; set; }

        public static implicit operator UrlItem(UrlItemEntity instance)
        {
            return new UrlItem(instance);
        }
    }
}
