using Amazon.Runtime;
using System.Threading.Tasks;

namespace ReadAwsSecretsUsingCognito.Interfaces
{
    public interface ICognitoAwsCredentialsProvider
    {
        Task<AWSCredentials> GetAwsCredentialsAsync();
    }
}
