using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OneDiscovery.Security
{
    public static class Password
    {
        //Encode the plain text password with the salt
        public static string Encode(string plain, string salt)
        {
            byte[] passwordAndSaltBytes = System.Text.Encoding.UTF8.GetBytes(plain + salt);
            byte[] hashBytes = new System.Security.Cryptography.SHA256Managed().ComputeHash(passwordAndSaltBytes);
            string hashString = Convert.ToBase64String(hashBytes);
            return hashString;
        }

        //Generate the salt use GUID
        public static string GenerateSalt()
        {
            return Guid.NewGuid().ToString();
        }
    }
}