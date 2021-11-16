using System;
using Microsoft.AspNetCore.Mvc;
using DomainEvents;

namespace PatientService.Controllers
{
    [ApiController]
    public class PatientController : ControllerBase
    {
        [HttpGet]
        [Route("dapr/subscribe")]
        public IActionResult Subscribe()
        {
            Console.WriteLine("\nReturning subscriptions.");

            return Ok(
                @"[{
                    ""pubsubname"": ""pubsub"",
                    ""topic"": ""gaden-idays-rx"",
                    ""route"": ""ReceivedRx""
                }]"
            );
        }

        [HttpPost]
        [Route("ReceivedRx")]
        public IActionResult ReceivedRx([FromBody] Message message)
        // public IActionResult ReceivedRx([FromBody] System.Text.Json.JsonElement entity)
        {
            // Console.WriteLine($"Input: {entity}");
            //
            // var message = JsonSerializer.Deserialize<Message>(entity.ToString());

            if (message?.data?.medication == null)
            {
                Console.WriteLine("Throwing away junk");
                return BadRequest();
            }
            
            Console.WriteLine($"\nReceived Rx with medication {message.data.medication.drugName} for {message.data.patient.lastName}, {message.data.patient.firstName}");

            return Ok();
        }

        public class Message
        {
            public string id { get; set; }
            public string source { get; set; }
            public string type { get; set; }
            public string specversion { get; set; }
            public string datacontenttype { get; set; }
            public RxPrescribedEvent data { get; set; }
            public string subject { get; set; }
            public string topic { get; set; }
            public string pubsubname { get; set; }
        }
    }
}
