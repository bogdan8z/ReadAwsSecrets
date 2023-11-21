using Amazon;
using Amazon.SecretsManager;
using ReadAwsSecretsUsingCognito.Interfaces;
using ReadAwsSecretsUsingCognito.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReadAwsSecretsUsingCognito
{
    public class AwsSecretsReader
    {
        private readonly Settings _settings;

        public AwsSecretsReader(Settings settings)
        {
            _settings = settings;
        }

        public async Task<(Exception Exception, string KeyValue)> ReadMyKey(string keyName)
        {
            try
            {
                var credentialsProvider = GetCongnitoProvider();
                var credentials = await credentialsProvider.GetAwsCredentialsAsync();
                var secretsClient = new AmazonSecretsManagerClient(credentials, RegionEndpoint.GetBySystemName(_settings.Region));
                var awsSecretsManager = new AwsSecretsManager(credentialsProvider, secretsClient, _settings.Region);
                                
                var error = await awsSecretsManager.LoadKeys(new string[] { keyName });
                if (error != null)
                {
                    return (error, null);
                }

                var keyValue = FormatKey(awsSecretsManager.Secrets, keyName);
                return (null, keyValue);
            }
            catch (Exception ex)
            {
                return (ex, null);
            }
        }

        private ICognitoAwsCredentialsProvider GetCongnitoProvider()
        {
            return new CognitoAwsCredentialsProvider(
                _settings.ClientId,
                _settings.UserPoolId,
                _settings.Username,
                _settings.Password,
                _settings.IdentityPoolId,
                _settings.IdentityProviderName,
                RegionEndpoint.GetBySystemName(_settings.Region));
        }

        private string FormatKey(List<(string Key, string Value)> secrets, string appIdName)
        {
            string appId = null;
            var secret = secrets.FirstOrDefault(se => se.Key == appIdName);
            if (!secret.Equals(default))
            {
                appId = secret.Value;
            }
           
            return appId;
        }
    }
}
