using System;
using System.Security.Cryptography;
using System.Text;

namespace com.gmail.sharma.vishal.InfoStore
{
    /// <summary>
    /// This class can be used to access single piece of secret information. e.g. User password
    /// If user password is never used before, it asks for user password to be entered in console window and masks the password as
    /// it is entered. The user password is then encrypted and stored in an User environment variable
    /// 
    /// If user password has been used before, it can access the encrypted password from User environment variable
    /// and return the decrypted password as plain text.
    /// 
    /// </summary>
    public class SecureInfoStore
    {

        public string GetSecureInfo(string envVariableName)
        {
            
            var encryptedInfo = Environment.GetEnvironmentVariable(envVariableName, EnvironmentVariableTarget.User);
            if (encryptedInfo != null)
            {
                return DecryptString(encryptedInfo);
            }
            else
            {
                var unencryptedInfo = ReadConsolePassword(envVariableName);
                encryptedInfo = EncryptString(unencryptedInfo);
                Environment.SetEnvironmentVariable(envVariableName, encryptedInfo, EnvironmentVariableTarget.User);
                return unencryptedInfo;
            }
        }

        static byte[] entropy = Encoding.Unicode.GetBytes("To generate unique key for this app");

        public static string EncryptString(string input)
        {
            byte[] encryptedData = ProtectedData.Protect(
                Encoding.Unicode.GetBytes(input),
                entropy,
                DataProtectionScope.CurrentUser);
            return Convert.ToBase64String(encryptedData);
        }

        private static string DecryptString(string encryptedData)
        {
            byte[] decryptedData = ProtectedData.Unprotect(
                    Convert.FromBase64String(encryptedData),
                    entropy,
                    DataProtectionScope.CurrentUser);
            return Encoding.Unicode.GetString(decryptedData);
        }

        //TODO: Look into using SecureString instead of string
        // Check if that will add more safety in this secnario 
        // where we shall need to get plain text info as output of Encrypt method
        private string ReadConsolePassword(string infoKey)
        {
            Console.WriteLine($"Your {infoKey} is not known. To store it securely, please enter {infoKey} here:");
            var pwd = new StringBuilder();
            while (true)
            {
                ConsoleKeyInfo i = Console.ReadKey(true);
                if (i.Key == ConsoleKey.Enter)
                {
                    Console.WriteLine();
                    break;
                }
                else if (i.Key == ConsoleKey.Backspace)
                {
                    if (pwd.Length > 0)
                    {
                        pwd.Remove(pwd.Length - 1, 1);
                        Console.Write("\b \b");
                    }
                }
                else
                {
                    pwd.Append(i.KeyChar);
                    Console.Write("*");
                }
            }
            return pwd.ToString();
        }
    }
}
