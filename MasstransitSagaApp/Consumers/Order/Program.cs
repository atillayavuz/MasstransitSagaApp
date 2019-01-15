using GreenPipes;
using MassTransit;
using System;

namespace Order
{
    class Program
    {
        static void Main(string[] args)
        {
            var busControl = Bus.Factory.CreateUsingRabbitMq(config =>
            {
                var host = config.Host(new Uri("rabbitmq://localhost"), h =>
                {
                    h.Username("user");
                    h.Password("bitnami");
                });

                config.ReceiveEndpoint(host, "order-queue", ep =>
                {
                    ep.UseRetry(r => r.Immediate(3));
                    ep.Consumer<OrderConsumer>();
                });
            });
            busControl.Start();

            Console.WriteLine("Hello World!");
        }
    }
}
