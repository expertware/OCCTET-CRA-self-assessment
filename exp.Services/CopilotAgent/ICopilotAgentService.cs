using exp.Models.ViewModels;

namespace exp.Services.CopilotAgent
{
    public interface ICopilotAgentService
    {
        Task<string> SendMessage(PromtForAgentResponseViewmodel prompt, int organisationId);
        Task<string> SendMessageAdm(PromtForAgentResponseAdmViewmodel prompt);
    }
}
