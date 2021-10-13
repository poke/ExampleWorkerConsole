using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
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
                    services.AddDbContext<DataContext>(options =>
                        options.UseSqlite("Data Source=database.db"));

                    // register background service
                    services.AddHostedService<ExampleBackgroundService>();
                })
                .Build();


            // if you need to retrieve a service at startup time, then
            // you can do that through `host.Services` on the built host
            using (var scope = host.Services.CreateScope())
            {
                // of course, if you want to retrieve a database context (or
                // some other scoped service), then you will need to create a
                // temporary service scope
                var db = scope.ServiceProvider.GetService<DataContext>();

                await db.Database.EnsureCreatedAsync();
            }


            // run the host in order to keep the process running while
            // the hosted (background) services are active
            await host.RunAsync();
        }
    }
}
