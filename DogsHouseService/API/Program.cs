using API.DI;

IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>());

var webHost = CreateHostBuilder(args).Build();
await webHost.RunAsync();