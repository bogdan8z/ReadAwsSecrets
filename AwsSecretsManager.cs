using Amazon.Runtime;
using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;
using Newtonsoft.Json;
using ReadAwsSecretsUsingCognito.Interfaces;
using ReadAwsSecretsUsingCognito.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadAwsSecretsUsingCognito
{
    public class AwsSecretsManager : IAwsSecretsManager
    {
        private const string SecretRequestVersionStage = "AWSCURRENT";
        private const string ErrorReadingKeyWithinSecretFormat = "Error reading key within secret {0}";
        private readonly ICognitoAwsCredentialsProvider _cognitoAwsCredentialsProvider;
        private readonly string _region;
        private AWSCredentials _credentials;
        private List<(string Key, string Value)> _secrets = null;
        private readonly IAmazonSecretsManager _smClient;

        private AwsSecretsManager()
        {

        }

        public AwsSecretsManager(
            ICognitoAwsCredentialsProvider credentialsProvider,
            IAmazonSecretsManager smClient,
            string region)
        {
            _cognitoAwsCredentialsProvider = credentialsProvider;
            _smClient = smClient;
            _region = region;
        }

        public List<(string Key, string Value)> Secrets
        {
            get
            {
                return _secrets;
            }
        }


        public async Task<Exception> LoadKeys(string[] keyNames)
        {
            _secrets = new List<(string Key, string Value)>();

            if (keyNames == null)
            {
                return null;
            }
            var tasks = new List<Task<(string KeyName, string KeyValue, Exception Exception)>>();

            foreach (var keyName in keyNames)
            {
                tasks.Add(GetSingleKey(keyName));
            }

            await Task.WhenAll(tasks);

            foreach (var kvp in tasks.Select(x => x.Result))
            {
                if (kvp.Exception != null)
                {
                    return kvp.Exception;
                }
                _secrets.Add((kvp.KeyName, kvp.KeyValue));
            }

            return null;
        }

        public async Task<(string Name, string Value, Exception exception)> GetSingleKey(string keyName)
        {
            try
            {
                var awsSecret = await GetSecretKeyValue(new AwsSecretsRequest
                {
                    SecretName = keyName,
                    Region = _region
                });

                var secret = JsonConvert.DeserializeObject<Dictionary<string, string>>(awsSecret);
                if (secret.ContainsKey(keyName))
                {
                    return (keyName, secret[keyName], null);
                }
                var errorMessage = string.Format(ErrorReadingKeyWithinSecretFormat, keyName);
                return (null, null, new Exception(errorMessage));
            }
            catch (Exception ex)
            {
                return (null, null, ex);
            }
        }

        public async Task<string> GetSecretKeyValue(AwsSecretsRequest secretsRequest)
        {
            await GetCredentials();

            return await GetSecretKeyValueWithCredentials(secretsRequest);
        }

        private async Task GetCredentials()
        {
            if (_credentials != null)
            {
                return;
            }
            _credentials = await _cognitoAwsCredentialsProvider.GetAwsCredentialsAsync();
        }

        public async Task<string> GetSecretKeyValueWithCredentials(AwsSecretsRequest secretsRequest)
        {
            var request = new GetSecretValueRequest
            {
                SecretId = secretsRequest.SecretName,
                VersionStage = SecretRequestVersionStage, // VersionStage defaults to AWSCURRENT if unspecified.
            };

            var response = await _smClient.GetSecretValueAsync(request);

            if (response == null)
            {
                //No secret value was returned
                return null;
            }

            return response.SecretString;
        }
    }
}
