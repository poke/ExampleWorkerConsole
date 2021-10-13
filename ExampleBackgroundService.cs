using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ExampleWorkerConsole
{
    public class ExampleBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public ExampleBackgroundService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // keep running
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    // retrieve the database context within a new service scope in
                    // order to allow retrieving the scoped service within the singleton
                    // background service
                    var db = scope.ServiceProvider.GetService<DataContext>();

                    // interact with the database
                    var item = new Item
                    {
                        Name = $"Item {DateTime.UtcNow:s}"
                    };
                    db.Items.Add(item);
                    await db.SaveChangesAsync();

                    Console.WriteLine(item.Name);
                }

                // wait every 15 seconds
                await Task.Delay(TimeSpan.FromSeconds(15), stoppingToken);
            }
        }
    }
}
