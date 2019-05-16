using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using Newtonsoft.Json;

namespace ModLocalization.Core
{
    public static class Extensions
    {
        static Extensions()
        {
        }

        /// <summary>
        /// Dumps the <see cref="obj"/> to the console
        /// </summary>
        /// <param name="obj"></param>
        public static void Dump<T>(this T obj)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            var sb = new StringBuilder();
            using (var writer = new StringWriter(sb))
            {
                JsonSerializer.Create(new JsonSerializerSettings
                {
                    Formatting = Formatting.Indented,
                    NullValueHandling = NullValueHandling.Include,
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                }).Serialize(writer, obj, typeof(T));
            }
            Console.WriteLine(sb.ToString());
        }

        /// <summary>
        /// Shortcut for a ForEach loop.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <param name="action"></param>
        public static void ForEach<T>(this IEnumerable<T> collection, Action<T> action)
        {
            foreach (var item in collection)
            {
                action.Invoke(item);
            }
        }
    }
}
