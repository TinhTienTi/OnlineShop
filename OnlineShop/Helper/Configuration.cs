using PayPal.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineShop.Helper
{
    public static class Configuration
    {
        public readonly static string ClientID;
        public readonly static string ClientSecret;
        static Configuration()
        {
            var config = GetConfig();
            ClientID = config["clientID"];
            ClientSecret = config["clientSecret"];
        }
        public static Dictionary<string, string> GetConfig()
        {
            return ConfigManager.Instance.GetProperties();
        }
        private static string GetAccessToken()
        {
            string accessToken = new OAuthTokenCredential(ClientID, ClientSecret, GetConfig()).GetAccessToken();
            return accessToken;
        }
        public static APIContext GetAPIContext()
        {
            APIContext apiContext = new APIContext(GetAccessToken());
            apiContext.Config = GetConfig();
            return apiContext;
        }
    }
}