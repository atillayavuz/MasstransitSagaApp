using Contracts;
using MassTransit;
using System;
using System.Threading.Tasks;

namespace Order
{
    public class OrderConsumer : IConsumer<SendOrder>
    {
        public Task Consume(ConsumeContext<SendOrder> context)
        {
            Console.WriteLine($"Order with {context.Message.Id} accepted."); 
            return Task.FromResult(0);
        }
    }
}
