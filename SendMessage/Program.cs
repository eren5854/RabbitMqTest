using RabbitMQ.Client;
using System.Text;

namespace SendMessage
{
    internal class Program
    {
        static void Main(string[] args)
        {
            
            var factory = new ConnectionFactory();
            factory.HostName = "192.168.1.154";//RabbitMq çalıştığı sunucunun ip adresi
            factory.Port = 5672;//RabbitMq ayağa kalktığı port.
            factory.UserName = "admin";//RabbitMq kullanıcı adı.
            factory.Password = "password";//RabbitMq şifresi.

            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.QueueDeclare(
                queue: "hello", //Kuyruk adı.
                durable: false, //Kuyruk muhafaza edilmesi için false yapılır. True yapılırsa sunucu yeniden başlama durumunda kuyruk yok olur.
                exclusive: false,// Kuyruğun birden fazla bağlantıya izin verilip verilmemesi.
                autoDelete: false,//Kuyruk boş ise (herhangi bir bağlantı durumu yoksa) kuyruğun otomatik olarak silinip silinmemesi.
                arguments: null//Kuyruğun özelliştirilmiş argumanları belirtilir.
                );

            while (true)
            {
                Console.WriteLine("Mesajınızı yazınız: ");
                string? message = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(message))
                {
                    Console.WriteLine("Mesaj alanı boş olamaz");
                    continue;
                }

                var body = Encoding.UTF8.GetBytes(message); // Mesajı byte'a dönüştür

                channel.BasicPublish(
                    exchange: string.Empty,
                    routingKey: "hello",
                    basicProperties: null,
                    body: body
                );

                Console.WriteLine($"[*] Gönderilen mesaj: {message}");
                Console.WriteLine("Çıkmak için 'exit' yazıp enter tuşuna basın, devam etmek için başka bir tuşa basın.");

                // Çıkış kontrolü
                if (Console.ReadLine()?.ToLower() == "exit")
                {
                    break;
                }
            }
        }
    }
}
