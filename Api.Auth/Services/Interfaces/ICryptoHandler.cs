namespace Api.Auth.Services.Interfaces
{
    public interface ICryptoHandler
    {
        string EncryptString(string text);
        string DecryptString(string cipherText);
    }
}