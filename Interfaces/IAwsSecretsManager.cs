using ReadAwsSecretsUsingCognito.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReadAwsSecretsUsingCognito.Interfaces
{
    public interface IAwsSecretsManager
    {
        Task<string> GetSecretKeyValue(AwsSecretsRequest secretsRequest);
        Task<string> GetSecretKeyValueWithCredentials(AwsSecretsRequest secretsRequest);
        Task<(string Name, string Value, Exception exception)> GetSingleKey(string keyName);
        Task<Exception> LoadKeys(string[] keyNames);
        List<(string Key, string Value)> Secrets { get; }
    }
}
