using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MWH.MyNethereum.TDownload
{
    class Program
    {
        static BlockingCollection<string> bc = new BlockingCollection<string>(5);

        static void Main(string[] args)
        {
            using (CancellationTokenSource ctsProducer = new CancellationTokenSource())
            {
                CancellationToken ctProducer = ctsProducer.Token;
                Task producer = Task.Run(() => Producer(ctProducer), ctProducer);
                Task consumer = Task.Run(() => Consumer());

                Console.WriteLine("Waiting for Producer and Consumer...");
                Console.ReadLine();
                Console.WriteLine("Cancelling Producer...");
                ctsProducer.Cancel();
                Console.WriteLine("Producer notified...");
                Console.WriteLine("Waiting for Producer to exit...");
                producer.Wait();
                Console.WriteLine("Producer exited");
                Console.WriteLine("Waiting for Consumer to exit...");
                consumer.Wait();
                Console.WriteLine("Consumer exited");
                Console.WriteLine("Press Enter to exit...");
                Console.ReadLine();
            }
        }

        static void Producer(CancellationToken ct)
        {
            int item = 0;
            while(true)
            {
                Console.WriteLine("Dowloading... (simulated)\t5 sec.");
                Thread.Sleep(5000);
                item += 100;
                for (int t = 0; t < 10; t++)
                {
                    if (bc.TryAdd(item.ToString()))
                    {
                        Console.WriteLine("TryAdd:\t" + item.ToString() + "\t1 sec.");
                        Thread.Sleep(1000);
                        item++;
                    }
                    else
                    {
                        Console.WriteLine("TryAdd:\t" + "blocked\t3 sec.");
                        Thread.Sleep(3000);
                        t--;
                    }
                }

                if (ct.IsCancellationRequested)
                {
                    Console.WriteLine("Producer cancelling...");
                    break;
                }
            }
        }

        static void Consumer()
        {
            string item;
            int nTimesBlocked = 0;
            Console.WriteLine("                              Uploading... (simulated)");
            while (true)
            {
                if (bc.TryTake(out item))
                {
                    Console.WriteLine("                              TryTake:\t" + item + "\t2 sec.");
                    nTimesBlocked = 0;
                    Thread.Sleep(2000);
                }
                else
                {
                    nTimesBlocked++;
                    if (nTimesBlocked > 4) break;
                    Console.WriteLine("                              TryTake:\t" + "blocked\t3 sec.");
                    Thread.Sleep(3000);
                }
            }
        }
    }
}
