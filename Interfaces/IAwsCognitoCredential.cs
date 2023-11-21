using Amazon.Runtime;
using ReadAwsSecretsUsingCognito.Models;
using System.Threading.Tasks;

namespace ReadAwsSecretsUsingCognito.Interfaces
{
    public interface IAwsCognitoCredential
    {
        Task<AWSCredentials> GetAwsCredentials(AwsSecretsRequest secretsRequest);
    }
}
