using System;
using Newtonsoft.Json.Serialization;

namespace LunchPac
{
    public class ClientReadyJsonResolver : DefaultContractResolver
    {
        protected override string ResolvePropertyName(string propertyName)
        {
            return LowercaseFirstCharacter(propertyName);
        }

        private string LowercaseFirstCharacter(string name)
        {
            return String.Format("{0}{1}", name.Substring(0, 1).ToLower(), name.Substring(1));
        }
    }
}