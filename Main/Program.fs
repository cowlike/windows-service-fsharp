module Sample.Main

open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Hosting
open Microsoft.Extensions.Logging
open Microsoft.Extensions.Logging.EventLog

open Sample.Worker

let configureLogging (builder: ILoggingBuilder) =
  builder.AddFilter<EventLogLoggerProvider>(fun level -> level >= LogLevel.Information)
  |> ignore

let configureServices _ (services : IServiceCollection) =
  services.AddHostedService<Worker>()
    .Configure(fun (cfg: EventLogSettings) ->
      cfg.LogName <- "Sample Service"
      cfg.SourceName <- "Sample Service Source")
    |> ignore


let createHostBuilder args =
  Host.CreateDefaultBuilder(args)
    .UseWindowsService()
    .ConfigureLogging(configureLogging)
    .ConfigureServices(configureServices)

[<EntryPoint>]
let main argv =
  (createHostBuilder argv).Build().Run()
  0
