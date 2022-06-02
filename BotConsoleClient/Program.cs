using System;
using System.Threading;
using System.Threading.Tasks;
using BotCore;
using DryIoc;
using DryIoc.MefAttributedModel;
using Vocabulary;

namespace BotConsoleClient
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            using var compositionRoot = BuildCompositionRoot();
            using var cts = new CancellationTokenSource();
            
            var bot = compositionRoot.Resolve<Bot>(args).Start(cts.Token);
            var botFailure = bot.ContinueWith(t => throw t.Exception, TaskContinuationOptions.OnlyOnFaulted);

            Console.WriteLine("Press any key to stop");
            Console.Read();

            cts.Cancel();

            try
            {
                await bot;
                await botFailure;
            }
            catch (OperationCanceledException)
            {
                // Do nothing
            }

            Console.WriteLine("Hello World!");
        }

        private static IContainer BuildCompositionRoot()
        {
            var container = new Container().WithMefAttributedModel();
            container.RegisterExports(typeof(Bot).Assembly);
            container.RegisterExports(typeof(IVocabularyRepository).Assembly);
            return container;
        }
    }
}
