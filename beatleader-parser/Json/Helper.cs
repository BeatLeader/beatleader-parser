using System.IO;
using System;
using System.Text.Json;

namespace Parser.Json
{
    internal class Helper
    {
        internal static T? DeserializeFromStream<T>(Stream stream)
        {
            try
            {
                return JsonSerializer.Deserialize<T>(stream);
            }
            catch (Exception e)
            {
                return default;
            }
        }
    }
}
