using System.Text.RegularExpressions;
using System;

namespace FriendstagramApi.Services.General
{
    public class GeneralManager
    {

        public static string CreateUniqueName()
        {
            Guid guid = Guid.NewGuid();
            string GuidString = Convert.ToBase64String(guid.ToByteArray());
            GuidString = Regex.Replace(GuidString, "[^a-zA-Z0-9_.]+", "", RegexOptions.Compiled);
            return GuidString;
        }
    }
}
