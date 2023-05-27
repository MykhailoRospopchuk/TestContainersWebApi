namespace TestContainerWebApi.Services
{
    public static class AccessTokenUrl
    {
        public static Guid GenerateAccessToken()
        {
            Guid uuid = Guid.NewGuid();
            return uuid;
        }
    }
}
