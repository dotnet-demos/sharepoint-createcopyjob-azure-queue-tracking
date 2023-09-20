using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PnP.Core.Auth.Services.Builder.Configuration;
using PnP.Core.Services.Builder.Configuration;
// See https://aka.ms/new-console-template for more information
await Task.Delay(10);
Console.WriteLine("Hello, World!");
IHostBuilder builder = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostBuilderContext,services) => {
        services.AddSingleton<SharePointCopyOperations>();
        services.AddSingleton<AzureStorageQueueCreateCopyJobTracker>();
        services.AddPnPCore(options=>
        {
            options.Sites.Add(hostBuilderContext.Configuration["SourceSiteRootUrl"],new PnPCoreSiteOptions()
            {
                SiteUrl = hostBuilderContext.Configuration["SourceSiteRootUrl"]
            });
        });
        services.AddPnPCoreAuthentication(options=>{
            options.Credentials.Configurations.Add("test",new PnPCoreAuthenticationCredentialConfigurationOptions(){
                ClientId = hostBuilderContext.Configuration["AzureAD:ClientId"],
                UsernamePassword = new PnPCoreAuthenticationUsernamePasswordOptions(){
                    Username=hostBuilderContext.Configuration["AzureAD:UserName"],
                    Password = hostBuilderContext.Configuration["AzureAD:Password"]
                }
            });
            options.Credentials.DefaultConfiguration = "test";
            options.Sites.Add(hostBuilderContext.Configuration["SourceSiteRootUrl"],new PnPCoreAuthenticationSiteOptions(){
                AuthenticationProviderName = "test"
            });
        });
    })
    .ConfigureAppConfiguration((hostbuilder,configBuilder)=>{
        configBuilder.AddJsonFile("appsettings.json");
    })
    .UseConsoleLifetime();
var host= builder.Build();
SharePointCopyOperations ops = host.Services.GetRequiredService<SharePointCopyOperations>();
await ops.CopyAllDriveContentsBetweenSites();
