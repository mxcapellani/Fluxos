namespace Fluxos.Domain.Wpp
{
    public class RequestModel
    {
        public string DataType { get; set; }
        public WhatsAppMessage Data { get; set; }
        public string SessionId { get; set; }
    }

    public class WhatsAppMessage
    {
        public string MessageId { get; set; }
        public string Body { get; set; }
        public long Timestamp { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public bool FromMe { get; set; }
        public string Type { get; set; }
        public bool HasMedia { get; set; }
        public bool IsForwarded { get; set; }
        public string NotifyName { get; set; }
        public bool IsNewMsg { get; set; }
    }

}
