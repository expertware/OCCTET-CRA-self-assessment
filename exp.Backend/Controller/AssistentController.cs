using exp.backend.Auth;
using exp.Models.ViewModels;
using exp.Services.CopilotAgent;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace exp.Backend.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssistentController : ControllerBase
    {
        private readonly ICopilotAgentService _agentService;
        public AssistentController(ICopilotAgentService agentService) 
        {
            _agentService = agentService;
        }

        [EnableRateLimiting("sliding")]
        [Authorize(Roles = "Guest")]
        [HttpPost("sendMessage")]
        public async Task<IActionResult> SendMessage(PromtForAgentResponseViewmodel prompt)
        {
            var organisationId = Convert.ToInt32(User.GetOrganisationID());

            var result = await _agentService.SendMessage(prompt, organisationId);
            return Ok(result);
        }
        [EnableRateLimiting("sliding")]
        [Authorize(Roles = "Admin")]
        [HttpPost("sendMessageAdm")]
        public async Task<IActionResult> SendMessageAdm(PromtForAgentResponseAdmViewmodel prompt)
        {
            var result = await _agentService.SendMessageAdm(prompt);
            return Ok(result);
        }
    }
}
