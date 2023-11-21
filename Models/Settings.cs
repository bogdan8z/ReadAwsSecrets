
namespace ReadAwsSecretsUsingCognito.Models
{
    public class Settings
    {
        public string Region { get; set; }
        public string ClientId { get; set; }
        public string Username { get; set; }
        public string UserPoolId { get; set; }
        public string Password { get; set; }
        public string IdentityPoolId { get; set; }
        public string IdentityProviderName { get; set; }
    }
}
