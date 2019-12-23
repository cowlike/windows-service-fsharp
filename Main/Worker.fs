module UPrint.Worker

open System
open System.Threading
open System.Threading.Tasks
open Microsoft.Extensions.Hosting
open Microsoft.Extensions.Logging

let rec doSomeWork (stoppingToken: CancellationToken) (logger: ILogger) =
  async {
    do! Async.Sleep(1000)
    logger.LogInformation("Worker running at: {0}", DateTimeOffset.Now)

    if (not stoppingToken.IsCancellationRequested) then
      return! (doSomeWork stoppingToken logger)
    else
      return ()
  }

type Worker(logger: ILogger<Worker>) =
  inherit BackgroundService()
  let _logger = logger

  override this.ExecuteAsync(stoppingToken: CancellationToken) =
    doSomeWork stoppingToken _logger
    |> Async.StartAsTask :> Task
