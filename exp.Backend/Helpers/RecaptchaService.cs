using exp.Models.Helpers;
using System.Text.Json;

namespace exp.Backend.Helpers
{

    public interface IRecaptchaService
    {
        Task<bool> VerifyTokenAsync(string token);

    }
    public class RecaptchaService : IRecaptchaService
    {
        private readonly HttpClient _httpClient;
        private readonly VaultSettings _vaultSettings;

        public RecaptchaService(HttpClient httpClient, IConfiguration config, VaultSettings vaultSettings)
        {
            _httpClient = httpClient;
            _vaultSettings = vaultSettings;
        }

        public async Task<bool> VerifyTokenAsync(string token)
        {
            var response = await _httpClient.PostAsync(
                $"https://www.google.com/recaptcha/api/siteverify?secret={_vaultSettings.RecaptchaSecretKey}&response={token}",
                null
            );

            var json = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<RecaptchaResponse>(json);
            return result.success && result.score >= 0.5;
        }
    }

    public class RecaptchaResponse
    {
        public bool success { get; set; }
        public float score { get; set; }
    }
}
