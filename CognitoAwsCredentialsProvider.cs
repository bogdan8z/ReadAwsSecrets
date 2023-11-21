using Amazon;
using Amazon.Runtime;
using ReadAwsSecretsUsingCognito.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ReadAwsSecretsUsingCognito
{
    public class CognitoAwsCredentialsProvider : ICognitoAwsCredentialsProvider
    {
        private string clientId;
        private string userPoolId;
        private string username;
        private string password;
        private string identityPoolId;
        private string identityProviderName;
        private RegionEndpoint regionEndpoint;

        public CognitoAwsCredentialsProvider(string clientId, string userPoolId, string username, string password, string identityPoolId, string identityProviderName, RegionEndpoint regionEndpoint)
        {
            this.clientId = clientId;
            this.userPoolId = userPoolId;
            this.username = username;
            this.password = password;
            this.identityPoolId = identityPoolId;
            this.identityProviderName = identityProviderName;
            this.regionEndpoint = regionEndpoint;
        }

        public Task<AWSCredentials> GetAwsCredentialsAsync()
        {
            //todo: add your code to read credentials from AWS Cognito
            throw new NotImplementedException();
        }
    }
}
