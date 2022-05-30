using System;
using System.Threading;
using System.Threading.Tasks;
using BotCore;

namespace BotConsoleClient
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            using var cts = new CancellationTokenSource();
            var bot = new Bot(args[0]).Start(cts.Token);
            bot.ContinueWith(t => throw t.Exception, TaskContinuationOptions.OnlyOnFaulted);

            Console.WriteLine("Press any key to stop");
            Console.Read();

            cts.Cancel();
            await bot;
            //bot.Wait();

            Console.WriteLine("Hello World!");
        }
    }
}
