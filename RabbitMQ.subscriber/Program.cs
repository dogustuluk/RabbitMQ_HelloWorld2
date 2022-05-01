using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading;

namespace RabbitMQ.subscriber
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory();
            factory.Uri = new Uri("amqps://nxdranwu:n_3nr-xZlXx0NoCWuFP05gTqZfp7_hwK@sparrow.rmq.cloudamqp.com/nxdranwu ");

            using var connection = factory.CreateConnection();
            
            var channel = connection.CreateModel();

            //RabbitMQ'dan mesajları kaçar kaçar alacağımızı belirtmek için BasicQos kullanılır. her bir subscriber'a kaç mesaj geleceğini belirtir.
            channel.BasicQos(0,1,false);
            //basicQos yapısı >>> (uint prefetchSize, ushort prefetchCount, bool global)
            //prefetchSize >>>> herhangi bir boyuttaki mesajı gönderebilme durumu (0 ise hepsi olur)
            //prefetchCount >>>> kaçar kaçar mesaj geleceği bilgisi
            //bool global >>>> eğer false olursa prefetchCount'ta belirtilen sayı her bir subscriber için gönderilir
                    //true olursa prefetchCount'ta verilen değeri her bir subscriber'a paylaştırır. Yani 6 prefetchCount girersek ve 3 subscirber olursa 2'şer tane mesaj
                    //yollar subscriber'lara.
                    
            var consumer = new EventingBasicConsumer(channel);
            
            channel.BasicConsume("hello-queue2", false, consumer); //false ile mesajı rabbitMQ hemen silmiyor, mesajı işledikten sonra bizim komutumuzla beraber siliyor (received kısmında kodlar var)
            
            consumer.Received += (object sender, BasicDeliverEventArgs e) =>
            {
                var message = Encoding.UTF8.GetString(e.Body.ToArray());

                Thread.Sleep(1500); //her bir mesajın 1.5sn'de işlenmesini sağlar. geciktirme uyguluyoruz daha net anlaşılması için.

                Console.WriteLine("Gelen Mesaj: " + message);

                channel.BasicAck(e.DeliveryTag, false); //rabbitMQ'nun mesajı silmesini sağlar
                //BasicAck yapısı >> (deliveryTag, bool multiple)
                //deliveryTag >>> parametreden gelen tag'ı belirtir. bulduğu tag'a ait mesajı siler. örnekte parametre "e"dir.
                //bool multiple >>> eğer true dersek o anda memory'de işlenmiş ama rabbitMQ'ya gitmemiş başka mesajlar da varsa onun bilgilerini de rabbitMQ'ya haber verir.
                            //örnekte tek bir mesaj işlediğimiz için false olarak verdik
            };
            
            Console.ReadLine();
        }
    }
}
