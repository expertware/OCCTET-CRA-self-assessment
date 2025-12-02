using exp.Infrastructure.Entities;
using exp.Infrastructure.Repositories.Organisations;
using exp.Models.Helpers;
using exp.Models.ViewModels;
using Microsoft.AspNetCore.DataProtection;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace exp.Services.CopilotAgent
{
    public class CopilotAgentService : ICopilotAgentService
    {
        private readonly HttpClient _httpClient;
        private readonly IOrganizationRepository _organizationRepository;
        private readonly VaultSettings _vaultSettings;
        private readonly IDataProtector _dataProtector;

        public CopilotAgentService(IOrganizationRepository organizationRepository, VaultSettings vaultSettings, IDataProtectionProvider dataProtectionProvider)
        {
            _httpClient = new HttpClient();
            _organizationRepository = organizationRepository;
            _vaultSettings = vaultSettings;
            _dataProtector = dataProtectionProvider.CreateProtector("DataProtectionKey");
        }

        private async Task<CopilotAgentConversationViewModel> StartConversation()
        {
            var requestBody = new { };

            var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");

            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_vaultSettings.AgentCopilotBearer}");
            var response = await _httpClient.PostAsync("https://directline.botframework.com/v3/directline/conversations", content);
            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();

                var result = JsonSerializer.Deserialize<CopilotAgentConversationViewModel>(jsonString, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return result;
            }
            return new CopilotAgentConversationViewModel();
        }

        private async Task<CopilotAgentResponseViewmodel> GetResponse(CopilotAgentConversationViewModel conversation)
        {
            var response = await _httpClient.GetAsync("https://directline.botframework.com/v3/directline/conversations/" + conversation.ConversationId + "/activities");

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();

                var result = JsonSerializer.Deserialize<CopilotAgentResponseViewmodel>(jsonString, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return result;
            }

            return new CopilotAgentResponseViewmodel();
        }
        public async Task<string> SendMessage(PromtForAgentResponseViewmodel prompt, int organisationId)
        {
            Organisation? companyDetails = new();

           
                companyDetails = _organizationRepository.GetAllOrganizations().AsEnumerable().FirstOrDefault(x => x.Id == organisationId);
                if (companyDetails == null)
                {
                    throw new ForbiddenException("Organization not found");
                }
          
                var promptText = $"Base information about my company name is '{companyDetails.Name}', based in '{companyDetails.Country.Name}', company size {companyDetails.CompanySize}'. " +
                                 $"My company delivery in '{string.Join(", ", companyDetails.DeliveryCountries.Select(x => x.Country.Name))}'. " +
                            $"We operate in the following activity sectors: '{string.Join(", ", companyDetails.OrganisationSectors.Select(x => x.ActivitySector.Name))}'. Use this information for help me to answer questions. " +
                            $"{prompt.Prompt}";
                prompt.Prompt = promptText;
      

            return await AskOxi(prompt.Prompt, companyDetails);
        }
        public async Task<string> SendMessageAdm(PromtForAgentResponseAdmViewmodel prompt)
        {
            Organisation? companyDetails = new();

            companyDetails = _organizationRepository.GetAllOrganizations().FirstOrDefault(x => x.Id == prompt.OrganisationId);
            if (companyDetails == null)
            {
                throw new ForbiddenException("Organization not found");
            }

            if (companyDetails != null)
            {
                var promptText = $"My company name is {companyDetails.Name}, based in {companyDetails.Country.Name}." +
                            $"We operate in the following activity sectors: {string.Join(", ", companyDetails.OrganisationSectors.Select(x => x.ActivitySector.Name))}." +
                            $" Delivery in: {string.Join(", ", companyDetails.DeliveryCountries.Select(x => x.Country.Name))}." +
                       $"{prompt.Prompt}";
                prompt.Prompt = promptText;
            }

            return await AskOxi(prompt.Prompt, companyDetails);
        }
        public async Task<string> AskOxi(string prompt, Organisation companyDetails)
        {
            var conversation = await StartConversation();

            var httpClient = new HttpClient();
            var requestBody = new
            {
                type = "message",
                from = new { id = "user1" },
                text = prompt,

            };

            var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");
            httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + conversation.Token);
            //send message
            var response = await httpClient.PostAsync("https://directline.botframework.com/v3/directline/conversations/" + conversation.ConversationId + "/activities", content);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("something went wrong!");
            }

            //received message
            var responseMessage = await GetResponse(conversation);
            while (responseMessage.Watermark == "0")
            {
                responseMessage = await GetResponse(conversation);
            }
            if (responseMessage.Activities != null && responseMessage.Activities.Any())
            {
                return ClearText(responseMessage.Activities.OrderBy(x => x.Timestamp).Skip(1).FirstOrDefault(x => x.Text != null)!.Text);
            }

            return "Sorry, Oxi didn't find an answer for you.";
        }
        private string ClearText(string text)
        {
            if (text != null)
            {
                text = Regex.Replace(text, @"^```[a-zA-Z]*\s*\r?\n?", "", RegexOptions.Multiline);



                text = Regex.Replace(text, @"\r?\n?```$", "", RegexOptions.Multiline);



                text = Regex.Replace(text, @"\s*\[\d+\]:\s*cite:\d+\s*""[^""]*""", "", RegexOptions.Multiline);



                text = Regex.Replace(text, @"\[\d+\]", "");



                text = Regex.Replace(text, @"[ \t]+$", "", RegexOptions.Multiline);
                text = Regex.Replace(text, @"\n{3,}", "\n\n");



                return text.Trim();
            }
            return "Sorry, Oxi didn't find an answer for you.";
        }
    }
}
