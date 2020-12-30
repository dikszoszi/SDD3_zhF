using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace TraceURL
{
    internal class Program
    {
        static CancellationTokenSource cts = new CancellationTokenSource();
        static void Main(string[] args)
        {
            string[] urls = new string[] {
                "google.com",
                "microsoft.com",
                "users.nik.uni-obuda.hu"
            };

            Process[] processes = new Process[urls.Length];

            for (int i = 0; i < urls.Length; i++)
            {
                processes[i] = new Process();
                processes[i].StartInfo = new ProcessStartInfo("tracert", urls[i])
                {
                    CreateNoWindow = true,
                    RedirectStandardOutput = true,
                    UseShellExecute = false
                };
                processes[i].Start();
            }

            foreach (Process proc in processes)
            {
                Console.WriteLine("Waiting ...");
                proc.WaitForExit();
            }

            string[] results = processes.Select(x => x.StandardOutput.ReadToEnd()).ToArray();
            for (int i = 0; i < results.Length; i++)
            {
                Console.WriteLine($"{results[i].Split('\n').Length - 7} hops used for {processes[i].StartInfo.Arguments}");
            }


            Task<int>[] tasks = new Task<int>[results.Length];

            StringProcessor sp = new StringProcessor();

            for (int i = 0; i < tasks.Length; i++)
            {
                int t = i;
                tasks[i] = new Task<int>(() => { return sp.IsTimedOut(results[t]); });
                tasks[i].ContinueWith((x) => { Console.WriteLine($"Task finished time outs: {x.Result}"); }, TaskContinuationOptions.OnlyOnRanToCompletion);
                tasks[i].ContinueWith((x) => { Console.WriteLine($"Exception:{x.Exception.Message}"); }, TaskContinuationOptions.OnlyOnFaulted);
                tasks[i].Start();
            }

            Task.WaitAll(tasks);
            Console.WriteLine("All timeouts: " + sp.AllTimeouts);

            Console.ReadLine();
        }
    }
}
