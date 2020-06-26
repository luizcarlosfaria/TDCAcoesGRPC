using System;
using System.Text;
using System.Text.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Confluent.Kafka;

namespace AcoesGRPC
{
    public class StockService : Stock.StockBase
    {
        private readonly ILogger<StockService> _logger;
        private readonly IConfiguration config;

        public StockService(ILogger<StockService> logger, IConfiguration config)
        {
            _logger = logger;
            this.config = config;
        }

        public override Task<StockResponse> Update(StockRequest request, ServerCallContext context)
        {
            var conteudoAcao = JsonSerializer.Serialize(new { Codigo = request.Name, Valor = request.Value });
            _logger.LogInformation($"Dados: {conteudoAcao}");

            try
            {
                var body = Encoding.UTF8.GetBytes(conteudoAcao);
                string topic = config["Kafka_Topic"];
                var configKafka = new ProducerConfig
                {
                    BootstrapServers = config["Kafka_Broker"]
                };

                using (var producer = new ProducerBuilder<Null, string>(configKafka).Build())
                {
                    var result = producer.ProduceAsync(
                        topic,
                        new Message<Null, string>
                        { Value = conteudoAcao }).Result;

                    _logger.LogInformation(
                        $"Apache Kafka - Envio para o tópico {topic} do Apache Kafka concluído | " +
                        $"{conteudoAcao} | Status: { result.Status.ToString()}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exceção: {ex.GetType().FullName} | Mensagem: {ex.Message}");
            }


            return Task.FromResult(new StockResponse() { Message = $"Mensagem enviada com sucesso! Codigo = {request.Name} | Valor = {request.Value}" });

        }

    }
}
