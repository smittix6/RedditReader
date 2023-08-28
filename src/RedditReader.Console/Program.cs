using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RedditReader.Data.Repositories;
using RedditReader.Data.RestClients;
using RedditReader.Logic;
using Microsoft.Extensions.Configuration;

// DI
HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
builder.Configuration.Sources.Clear();
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
builder.Services.AddHttpClient<IRedditAuthClient, RedditAuthClient>();
builder.Services.AddHttpClient<IRedditRestClient, RedditRestClient>();
builder.Services.AddSingleton<IStatsRepository, StatsRepository>();
builder.Services.AddSingleton<IRedditAuthClient, RedditAuthClient>();
builder.Services.AddSingleton<IRedditRestClient, RedditRestClient>();
builder.Services.AddSingleton<IPostsRepository, PostsRepository>();
builder.Services.AddScoped<IStatsLogger, StatsLogger>();
builder.Services.AddHostedService<SubredditListenerService>();

// Run
using IHost host = builder.Build();
await host.RunAsync();

Console.WriteLine("Shutting down...");
