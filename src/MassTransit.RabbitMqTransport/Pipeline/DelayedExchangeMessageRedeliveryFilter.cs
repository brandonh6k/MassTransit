﻿// Copyright 2007-2015 Chris Patterson, Dru Sellers, Travis Smith, et. al.
//  
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use
// this file except in compliance with the License. You may obtain a copy of the 
// License at 
// 
//     http://www.apache.org/licenses/LICENSE-2.0 
// 
// Unless required by applicable law or agreed to in writing, software distributed
// under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR 
// CONDITIONS OF ANY KIND, either express or implied. See the License for the 
// specific language governing permissions and limitations under the License.
namespace MassTransit.RabbitMqTransport.Pipeline
{
    using System.Diagnostics;
    using System.Threading.Tasks;
    using Contexts;
    using MassTransit.Pipeline;
    using Monitoring.Introspection;


    /// <summary>
    /// Uses a delayed exchange in RabbitMQ to delay a message retry
    /// </summary>
    /// <typeparam name="TMessage"></typeparam>
    public class DelayedExchangeMessageRedeliveryFilter<TMessage> :
        IFilter<ConsumeContext<TMessage>>
        where TMessage : class
    {
        async Task IProbeSite.Probe(ProbeContext context)
        {
        }

        [DebuggerNonUserCode]
        Task IFilter<ConsumeContext<TMessage>>.Send(ConsumeContext<TMessage> context, IPipe<ConsumeContext<TMessage>> next)
        {
            context.GetOrAddPayload<MessageRedeliveryContext>(() => new DelayedExchangeMessageRedeliveryContext<TMessage>(context));

            return next.Send(context);
        }

        bool IFilter<ConsumeContext<TMessage>>.Visit(IPipelineVisitor visitor)
        {
            return visitor.Visit(this);
        }
    }
}