internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("Starting long-running task. Press any key to cancel...");

        var cts = new CancellationTokenSource();

        var token = cts.Token;

        var task = Task.Factory.StartNew(() =>
        {
            for (int i = 0; i < 10; i++)
            {
                if (token.IsCancellationRequested)
                {
                    Console.WriteLine("Task cancelled.");
                    return;
                }

                Console.WriteLine($"Task progress: {i + 1}/10");
                Thread.Sleep(200);
            }

            Console.WriteLine("Task completed successfully.");
        }, token);

        Console.ReadKey(true);

        if (!task.IsCompleted)
        {
            cts.Cancel();
            Console.WriteLine("Cancellation requested.");
        }

        try
        {
            task.Wait(token);
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine("Task was cancelled.");
        }

        Console.WriteLine("Application exiting.");
    }

    //private static void Main(string[] args)
    //{
    //    var cts = new CancellationTokenSource();

    //    var token = cts.Token;

    //    var task = Task.Factory.StartNew(() =>
    //    {
    //        for (int i = 0; i < 10; i++)
    //        {
    //            token.ThrowIfCancellationRequested();

    //            Console.WriteLine("Task running");
    //            Thread.Sleep(200);
    //        }

    //        Console.WriteLine("Task completed");
    //    }, token);

    //    Console.ReadKey();

    //    cts.Cancel();

    //    try
    //    {
    //        task.Wait(token);
    //    }
    //    catch (OperationCanceledException)
    //    {
    //        Console.WriteLine("Task cancelled");
    //    }
    //    catch (Exception ex)
    //    {
    //        Console.WriteLine($"Unexpected exception: {ex}");
    //    }
    //}
}