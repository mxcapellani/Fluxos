namespace Fluxos.Domain.Wpp
{
    public class ReceberMensagem
    {
        public MessageId Id { get; set; } // Campo obrigatório
        public string From { get; set; } // Campo obrigatório
        public string To { get; set; } // Campo obrigatório
        public string Body { get; set; } // Campo obrigatório
        public string Type { get; set; } // Campo obrigatório
        public long Timestamp { get; set; }
        public string DeviceType { get; set; } // Campo obrigatório
        public bool IsForwarded { get; set; }
        public bool Viewed { get; set; }
        public string NotifyName { get; set; } // Campo obrigatório
    }

    public class MessageId
    {
        public bool FromMe { get; set; }
        public string Remote { get; set; }
        public string Id { get; set; }
        public string Serialized { get; set; }
    }
}
