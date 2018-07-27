using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using System;

namespace Authentication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.Title = "Authentication";

            BuildWebHost(args).Run();
        }

        /// <summary>
        /// Builds wbe host
        /// </summary>
        /// <param name="args"> args </param>
        /// <returns></returns>
        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }
}
