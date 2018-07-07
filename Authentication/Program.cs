using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace Authentication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        /// <summary>
        /// Builds wbe host
        /// </summary>
        /// <param name="args"> args </param>
        /// <returns></returns>
        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<AuthenticationStartup>()
                .Build();
    }
}
