using System;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using PlatformService.Dtos;
using RabbitMQ.Client;

namespace PlatformService.AsyncDataServices
{
    public class MessageBusClient : IMessageBusClient
    {
        private readonly IConfiguration _configuration;
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public MessageBusClient(IConfiguration configuration)
        {
            _configuration = configuration;
            var factory = new ConnectionFactory()
            {
                HostName = _configuration["RabbitMQHost"],
                Port = int.Parse(_configuration["RabbitMQPort"])
            };

            try
            {
                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();

                _channel.ExchangeDeclare("trigger", ExchangeType.Fanout);

                _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;

                System.Console.WriteLine($"--> Connected to Rabbit MQ");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"COuld not connect to the MQ : {ex.Message}");
            }
        }

        private void RabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs e)
        {
            System.Console.WriteLine($"--> RabbitMQ connection shutdown fired.");
        }

        public void PublishNewPlatform(PlatformPublishDto platformPublishDto)
        {
            var message = JsonSerializer.Serialize(platformPublishDto);

            if (_connection.IsOpen)
            {
                System.Console.WriteLine("--> RabbitMQ connection is Open, sending message");
                SendMessage(message);
            }
            else
            {
                System.Console.WriteLine("--> RabbitMQ connection is CLOSED!");
            }
        }

        public void Dispose()
        {
            System.Console.WriteLine("--> Inside Dispose");
            if (_channel.IsOpen)
            {
                _channel.Close();
                _connection.Close();
            }
        }
        private void SendMessage(string messageToSend)
        {
            var body = Encoding.UTF8.GetBytes(messageToSend);
 
            _channel.BasicPublish(exchange: "trigger",
                routingKey: "",
                basicProperties: null,
                body: body);

            System.Console.WriteLine($"{messageToSend} was sent");
        }
    }
}