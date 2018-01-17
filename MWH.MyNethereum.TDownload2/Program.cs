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
        static BlockingCollection<string> bcTx = new BlockingCollection<string>(5);
        static BlockingCollection<string> bcBlock = new BlockingCollection<string>(5);

        static void Main(string[] args)
        {
            using (CancellationTokenSource ctsProducer = new CancellationTokenSource())
            {
                Console.WriteLine("Starting Producer and Consumers...");
                CancellationToken ctProducer = ctsProducer.Token;
                Task producer = Task.Run(() => Producer(ctProducer), ctProducer);
                Task consumerBlock = Task.Run(() => ConsumerBlock());
                Task consumerTx = Task.Run(() => ConsumerTX());

                Console.WriteLine("Waiting for Producer and Consumers...");
                Console.ReadLine();
                Console.WriteLine("Cancelling Producer...");
                ctsProducer.Cancel();
                Console.WriteLine("Producer notified...");
                Console.WriteLine("Waiting for Producer to exit...");
                producer.Wait();
                Console.WriteLine("Producer exited");

                Console.WriteLine("Waiting for ConsumerBlock to exit...");
                consumerBlock.Wait();
                Console.WriteLine("Waiting for ConsumerTx to exit...");
                consumerTx.Wait();
                Console.WriteLine("Consumers exited");

                Console.WriteLine("Press Enter to exit...");
                Console.ReadLine();
            }
        }

        static void Producer(CancellationToken ct)
        {
            int itemBlock = 0;
            int itemTx = 0;
            while (true)
            {
                Console.WriteLine("Dowloading... (simulated)\t5 sec.");
                Thread.Sleep(5000);
                itemBlock += 1000;
                itemTx += 100;

                bool blockAdded = false;
                while (!blockAdded)
                {
                    if (bcBlock.TryAdd(itemBlock.ToString()))
                    {
                        blockAdded = true;
                        Console.WriteLine("TryAdd.Block:\t" + itemBlock.ToString() + "\t1 sec.");
                        Thread.Sleep(1000);
                        itemBlock++;
                    }
                    else
                    {
                        blockAdded = false;
                        Console.WriteLine("TryAdd.Block:\t" + "blocked\t3 sec.");
                        Thread.Sleep(3000);
                    }
                }

                for (int t = 0; t < 10; t++)
                {
                    if (bcTx.TryAdd(itemTx.ToString()))
                    {
                        Console.WriteLine("TryAdd.Tx:\t" + itemTx.ToString() + "\t1 sec.");
                        Thread.Sleep(1000);
                        itemTx++;
                    }
                    else
                    {
                        Console.WriteLine("TryAdd.Tx:\t" + "blocked\t3 sec.");
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

        static void ConsumerBlock()
        {
            string item;
            int nTimesBlocked = 0;
            Console.WriteLine("                                        Uploading block... (simulated)");
            while (true)
            {
                if (bcBlock.TryTake(out item))
                {
                    Console.WriteLine("                                        TryTake.Block:\t" + item + "\t2 sec.");
                    nTimesBlocked = 0;
                    Thread.Sleep(2000);
                }
                else
                {
                    nTimesBlocked++;
                    if (nTimesBlocked > 4) break;
                    Console.WriteLine("                                        TryTake.Block:\t" + "blocked\t5 sec.");
                    Thread.Sleep(5000);
                }
            }
            Console.WriteLine("                                        TryTake.Block: exiting");
        }

        static void ConsumerTX()
        {
            string item;
            int nTimesBlocked = 0;
            Console.WriteLine("                                        Uploading tx... (simulated)");
            while (true)
            {
                if (bcTx.TryTake(out item))
                {
                    Console.WriteLine("                                        TryTake.Tx:\t" + item + "\t2 sec.");
                    nTimesBlocked = 0;
                    Thread.Sleep(2000);
                }
                else
                {
                    nTimesBlocked++;
                    if (nTimesBlocked > 4) break;
                    Console.WriteLine("                                        TryTake.Tx:\t" + "blocked\t3 sec.");
                    Thread.Sleep(3000);
                }
            }
            Console.WriteLine("                                        TryTake.Tx: exiting");
        }
    }
}
