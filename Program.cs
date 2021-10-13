using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace ExampleWorkerConsole
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            var host = Host.CreateDefaultBuilder(args)
                .ConfigureServices(services =>
                {
                })
                .Build();

            await host.RunAsync();
        }
    }
}
