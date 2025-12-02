using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace exp.Models.ViewModels
{
    public class CopilotAgentConversationViewModel
    {
        public string Token { get; set; }
        public string ConversationId { get; set; }
        public string StreamUrl { get; set; }
    }

    public class CopilotAgentResponseViewmodel
    {
        public List<CopilotAgentResponseActivitiesViewmodel> Activities { get; set; }
        public string Watermark { get; set; }
    }
    public class PromtForAgentResponseViewmodel
    {
        public string Prompt { get; set; } = default!;
    }
    public class PromtForAgentResponseAdmViewmodel
    {
        public string Prompt { get; set; } = default!;
        public required int OrganisationId { get; set; } = default!;
    }
    public class CopilotAgentResponseActivitiesViewmodel
    {
        public DateTime Timestamp { get; set; }
        public string Text { get; set; }
    }

}
