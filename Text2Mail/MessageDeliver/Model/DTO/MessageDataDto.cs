using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MessageDeliver.Model.DTO
{
    public class MessageDataDto
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("publisherId")]
        public string PublisherId { get; set; }
        [JsonProperty("publisherDisplayName")]
        public string PublisherDisplayName { get; set; }
        [JsonProperty("messageId")]
        public string MessageId { get; set; }
        [JsonProperty("sender")]
        public string Sender { get; set; }
        [JsonProperty("body")]
        public string Body { get; set; }
        [JsonProperty("timestamp")]
        public string Timestamp { get; set; }
        [JsonProperty("forwarded")]
        public bool Forwarded { get; set; }

        public static MessageDataDto FromEntity(MessageData entity)
        {
            return new MessageDataDto()
            {
                Id = entity.Id,
                PublisherId = entity.PublisherId.ToString(),
                PublisherDisplayName = entity.PublisherDisplayName,
                MessageId = entity.MessageId.ToString(),
                Sender = entity.Sender,
                Body = entity.Body,
                Timestamp = entity.Timestamp.ToString("yyyy-MM-dd'T'HH:mm:ss"),
                Forwarded = entity.Forwarded
            };
        }
    }
}
