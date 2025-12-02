using System.Security.Cryptography;
using System.Text;

namespace exp.Backend.Helpers
{
    public class GenerateAccessCode
    {
        public static string GeneratePassword()
        {
            int[] segments = { 8, 4, 4, 4, 12 };
            var sb = new StringBuilder();

            foreach (var seg in segments)
            {
                sb.Append(RandomString(seg));
                sb.Append('-');
            }

            sb.Length--;
            return sb.ToString();
        }

        private static string RandomString(int length)
        {
            var Charset = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

            var bytes = new byte[length];
            var result = new char[length];

            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(bytes);
            }

            for (int i = 0; i < length; i++)
            {
                result[i] = Charset[bytes[i] % Charset.Length];
            }

            return new string(result);
        }
    }
}
