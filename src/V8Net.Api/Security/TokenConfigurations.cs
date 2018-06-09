namespace V8Net.Api.Security
{
    public class TokenConfigurations
    {
        public string ISSUER { get; }
        public string AUDIENCE { get; }
        public string SECRET_KEY { get; }
        public int SECONDS { get; }

        public TokenConfigurations()
        {
            ISSUER = "480602b3";
            AUDIENCE = "c2d13d8a31db";
            SECRET_KEY = "480602b3-2404-4096-85c3-c2d13d8a31db";
            SECONDS = 86400;
        }
    }
}
