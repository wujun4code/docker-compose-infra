using BetterCoding.MessagePubSubCenter.Contracts.Request;
using BetterCoding.MessagePubSubCenter.Services;
using BetterCoding.Strapi.SDK.Core;
using BetterCoding.Strapi.SDK.Core.Webhook;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BetterCoding.MessagePubSubCenter.API.Controllers
{
    [ApiController]
    [Route("strapi")]
    public class StrapiWebhookController : ControllerBase
    {
        private readonly IStrapiWebhookService _strapiWebhookService;
        private readonly ILogger<StrapiWebhookController> _logger;
        private readonly StrapiClient _strapiClient;
        public StrapiWebhookController(IStrapiWebhookService strapiWebhookService,
            ILogger<StrapiWebhookController> logger,
            StrapiClient strapiClient)
        {
            _strapiWebhookService = strapiWebhookService;
            _logger = logger;
            _strapiClient = strapiClient;
        }

        [HttpPost("webhook")]
        public async Task<IActionResult> InvokeWebhook([FromBody] WebhookPayload payload)
        {
            if (payload.Event == null) return BadRequest();
            try
            {
                var json = JsonConvert.SerializeObject(payload);
                _logger.LogInformation($"received strapi webhook: {json}");
                await _strapiWebhookService.PublishMessageAsync(payload);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(500, new { message = "Unable to receive strapi webhook payload" });
            }
        }

        [HttpPost("forward/login")]
        public async Task<IActionResult> LogIn([FromForm] LogInRequest request)
        {
            await _strapiClient.GetREST().LogInAsync(request.Username, request.Password);
            return Ok();
        }
    }
}