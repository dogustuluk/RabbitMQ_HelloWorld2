using RabbitMQ.Client;
using System;
using System.Text;

namespace RabbitMQ.publisher
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory();
            factory.Uri = new Uri("amqps://nxdranwu:n_3nr-xZlXx0NoCWuFP05gTqZfp7_hwK@sparrow.rmq.cloudamqp.com/nxdranwu ");
            using var connection = factory.CreateConnection();
            var channel = connection.CreateModel();
            channel.QueueDeclare("hello-queue2",true,false,false);
            var message = "hello world 2";
            var messageBody = Encoding.UTF8.GetBytes(message);
            channel.BasicPublish(string.Empty, "hello-queue2", null, messageBody);
            Console.WriteLine("Mesaj Gönderilmiştir");
            Console.ReadLine();

        }
    }
}
