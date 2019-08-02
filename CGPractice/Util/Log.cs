using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGPractice.Util
{
    class Log
    {
        private static StringBuilder sb = new StringBuilder();
        // debug打印
        public static void debug(params string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("[Debug " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "]:");
            Console.ForegroundColor = ConsoleColor.Gray;
            print(args);
        }

        public static void print(string[] args)
        {
            sb.Clear();
            for (int i = 0; i < args.Length; i++)
            {

                sb.Append(args[i]);
                sb.Append(", ");
            }
            Console.WriteLine(sb.ToString());
        }
    }
}
