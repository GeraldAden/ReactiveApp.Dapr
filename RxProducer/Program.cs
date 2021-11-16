using System;
using System.Text.Json;
using System.Threading;
using System.Net.Http;
using System.Threading.Tasks;
using DomainEvents;

namespace RxProducer
{
	class Program
	{
		private static string dapr_port = "3500";
		private static string dapr_pubsub = "pubsub";
		private static string topic = "gaden-idays-rx";

		private static string dapr_url = $"http://localhost:{dapr_port}/v1.0/publish/{dapr_pubsub}/{topic}";

		private static int rxId = 1;
		private static string instanceId = Guid.NewGuid().ToString();

		static async Task Main(string[] args)
		{
			Console.WriteLine($"RxProducer({instanceId}) -  starting");

			var jsonOptions = new JsonSerializerOptions
			{
				PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
			};

            var httpClient = new HttpClient();

            while (true)
            {
                var rxEvent = GenerateRxPrescribedEvent();

                try
                {
                    var content = new StringContent(JsonSerializer.Serialize(rxEvent, jsonOptions));

					Console.WriteLine($"HTTP Client Base URL: {httpClient.BaseAddress}");
                    Console.WriteLine($"DAPR URL: {dapr_url}");

					var response = await httpClient.PostAsync(dapr_url, content);

                    Console.WriteLine(response.StatusCode == System.Net.HttpStatusCode.OK
                        ? $"RxProducer({instanceId}) - PUBLISHED RxPrescribedEvent '{rxEvent}' to topic '{topic}'\n"
                        : $"RxProducer({instanceId}) - FAILED to publish RxPrescribedEvent '{rxEvent}' to topic '{topic}'\n");
                }
                catch (Exception e)
                {
                    Console.WriteLine($"RxProducer({instanceId}) - Publishing RxPrescribedEvent failed: { e.Message }");
                }

                Thread.Sleep(3000);
            }
        }

		static RxPrescribedEvent GenerateRxPrescribedEvent()
		{
			var rxPrescribedEvent = new RxPrescribedEvent
			{
				// Timestamp = DateTime.Now,
				patient = new Patient
				{
					firstName = $"PatientFirstName{rxId}",
					lastName = $"PatientLastName{rxId}"
				},
				medication = new Medication
				{
					drugName = $"Drug{rxId}"
				}
			};

			rxId++;

			return rxPrescribedEvent;
		}
	}
}
