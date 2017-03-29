using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;

namespace Sayonara
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
								.UseSetting("detailedErrors", "true") //Helped with Azure debugging
								.UseIISIntegration()
                .UseStartup<Startup>()
								.CaptureStartupErrors(true) //Helped with Azure debugging
                .Build();

            host.Run();
        }
    }
}
