using System;
using System.Collections.Generic;
using System.Linq;

namespace ServiceIntegration.Global
{
    internal static class Utilites
    {
        public static byte[] ToByteArray(this System.IO.Stream stream)
        {
            stream.Position = 0;
            byte[] buffer = new byte[stream.Length];
            for (int totalBytesCopied = 0; totalBytesCopied < stream.Length;)
                totalBytesCopied += stream.Read(buffer, totalBytesCopied, Convert.ToInt32(stream.Length) - totalBytesCopied);
            return buffer;
        }

        public static string ToJson<T>(this IDictionary<T, T> dictionary) =>
            dictionary != null ? Newtonsoft.Json.JsonConvert.SerializeObject(dictionary, Newtonsoft.Json.Formatting.None) : string.Empty;

        public static bool AnyOrDefault<T>(this IEnumerable<T> source) => 
            (source?.Any() ?? false);

    }
}
