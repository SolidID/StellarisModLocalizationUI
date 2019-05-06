using System;
using System.Collections.Generic;
using System.Text;

namespace ModLocalization.Core
{
    public static class Extensions
    {
        /// <summary>
        /// Dumps the <see cref="message"/> to the console
        /// </summary>
        /// <param name="message"></param>
        public static void Dump(this string message)
        {
            Console.WriteLine(message);
        }
    }
}
