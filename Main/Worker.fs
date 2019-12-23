module UPrint.Worker

open System
open System.Threading
open System.Threading.Tasks
open Microsoft.Extensions.Hosting
open Microsoft.Extensions.Logging

type LogMsg = Info of LogLevel * string

let log (logger:ILogger) level msg = logger.Log(level, msg)

let rec doSomeWork count (stoppingToken: CancellationToken) (logger: ILogger) =
  async {
    do! Async.Sleep(1000)
    if count % 5 = 0
      then (LogLevel.Warning, "ping!")
      else (LogLevel.Information, sprintf "Worker running at: %A %d" DateTimeOffset.Now count)
    ||> log logger

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
