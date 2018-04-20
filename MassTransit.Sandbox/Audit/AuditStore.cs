﻿using System;
using System.Linq;
using System.Threading.Tasks;
using MassTransit.Audit;
using Newtonsoft.Json;

namespace MassTransit.Sandbox.Audit
{
    public class AuditStore : IMessageAuditStore
    {
        private readonly string _code;

        public AuditStore(string code)
        {
            _code = code;
        }

        public async Task StoreMessage<T>(T message, MessageAuditMetadata metadata) where T : class
        {
                await Console.Out.WriteLineAsync($"{_code} Payload: {JsonConvert.SerializeObject(message)}");
                await Console.Out.WriteLineAsync($"{_code} Metadata: {JsonConvert.SerializeObject(metadata)}");
        }
    }

    public class MyConsumeMetadataFactory : IConsumeMetadataFactory
    {
        public MessageAuditMetadata CreateAuditMetadata<T>(ConsumeContext<T> context) where T : class
        {
            return new MessageAuditMetadata
            {
                ContextType = "contextType",
                ConversationId = context.ConversationId,
                CorrelationId = context.CorrelationId,
                InitiatorId = context.InitiatorId,
                MessageId = context.MessageId,
                RequestId = context.RequestId,
                DestinationAddress = context.DestinationAddress?.AbsoluteUri,
                SourceAddress = context.SourceAddress?.AbsoluteUri,
                FaultAddress = context.FaultAddress?.AbsoluteUri,
                ResponseAddress = context.ResponseAddress?.AbsoluteUri,
                Headers = context.Headers?.GetAll()?.ToDictionary(k => k.Key, v => v.Value.ToString())
            };
        }
    }
}