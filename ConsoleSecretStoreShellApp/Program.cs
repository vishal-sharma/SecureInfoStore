using com.gmail.sharma.vishal.InfoStore;
using System.Configuration;

namespace com.gmail.sharma.vishal.ConsoleSecretStoreShellApp
{
    class Program
    {
        static void Main(string[] args)
        {
            DemoSecretInfo();
        }

        static void DemoSecretInfo()
        {            
            var secureInfoStore = new SecureInfoStore();
            var envVariableNameForUserName = ConfigurationManager.AppSettings["userName"];
            var envVariableNameForPassword = ConfigurationManager.AppSettings["userKey"];
            var username = secureInfoStore.GetSecureInfo(envVariableNameForUserName);
            var password = secureInfoStore.GetSecureInfo(envVariableNameForPassword);
        }
    }
}
