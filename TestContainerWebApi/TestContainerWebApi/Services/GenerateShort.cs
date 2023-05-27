using System;

namespace TestContainerWebApi.Services
{
    public static class GenerateShort
    {
        private static readonly string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        private static Random random = new Random();
        public static string GenerateShortUrl()
        {
            string result = new(Enumerable.Repeat(chars, 6).Select(s => s[random.Next(s.Length)]).ToArray());
            return result;
        }
    }
}
