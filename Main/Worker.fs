module UPrint.Worker

open System
open System.Threading
open System.Threading.Tasks
open Microsoft.Extensions.Hosting
open Microsoft.Extensions.Logging

let rec doSomeWork count (stoppingToken: CancellationToken) (logger: ILogger) =
  async {
    do! Async.Sleep(1000)
    logger.LogInformation("Worker running at: {0} {1}", DateTimeOffset.Now, count)

    if (not stoppingToken.IsCancellationRequested) then
      return! (doSomeWork (count + 1) stoppingToken logger)
    else
      return ()
  }

type Worker(logger: ILogger<Worker>) =
  inherit BackgroundService()
  let _logger = logger

  override this.ExecuteAsync(stoppingToken: CancellationToken) =
    doSomeWork 0 stoppingToken _logger
    |> Async.StartAsTask :> Task
