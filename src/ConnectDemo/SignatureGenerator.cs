namespace ConnectDemo
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;

    public static class SignatureGenerator
    {
        public static string Generate(Dictionary<string, string> parameters, string secretKey)
        {
            /*
             - Percent encode every key and value that will be signed.
             - Sort the list of parameters alphabetically by encoded key
             - For each key/value pair:
                - Append the encoded key to the output string.
                - Append the ‘=’ character to the output string.
                - Append the encoded value to the output string.
                - If there are more key/value pairs remaining, append a ‘&’ character to the output string.
             */

            var pairs = parameters
                .Select(kvp => $"{Uri.EscapeDataString(kvp.Key)}={Uri.EscapeDataString(kvp.Value)}")
                .OrderBy(s => s);

            var signatureString = string.Join("&", pairs);

            using var hmac = new HMACSHA1(Encoding.ASCII.GetBytes(secretKey));

            return Convert.ToBase64String(hmac.ComputeHash(Encoding.ASCII.GetBytes(signatureString)));
        }
    }
}
