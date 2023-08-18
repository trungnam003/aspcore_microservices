using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

var factory = new ConnectionFactory() { HostName = "localhost" };

using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

channel.QueueDeclare("orders", exclusive: false);

var consumer = new EventingBasicConsumer(channel);

consumer.Received += (_, eventArgs) =>
{
    var body = eventArgs.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    Console.BackgroundColor = ConsoleColor.DarkGreen;
    Console.ForegroundColor = ConsoleColor.Black;
    Console.WriteLine($"MESSAGE: {message}");
    Console.WriteLine();
};

channel.BasicConsume("orders", true, consumer);

Console.ReadKey();