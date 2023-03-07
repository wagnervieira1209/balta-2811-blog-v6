using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogV6
{
    public static class Configuration
    {
        // Token - JWT - Json Web Token
        public static string JwtKey = "zmzmXMamASz3MRt4M5MMo93hFR4499DeRR9=";
        public static string ApiKeyName = "api_key";
        public static string ApiKey = "curso_api_Ilast4/z0meoMxcw/m0zdjf=";

        public class Smtp
        {
            public string Host { get; set; } 
            public int Port { get; set; } = 25;
            public string Username { get; set; }
            public string Password { get; set; }
        }
    }
}