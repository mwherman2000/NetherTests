using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.DTOs;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MWH.MyNethereum.TDownload
{
    class Program
    {
        static BlockingCollection<Transaction> bcTx = new BlockingCollection<Transaction>(5);
        static BlockingCollection<Block> bcBlock = new BlockingCollection<Block>(5);

        static void Main(string[] args)
        {
            using (CancellationTokenSource ctsProducer = new CancellationTokenSource())
            {
                Console.WriteLine("Starting Producer and Consumers...");
                CancellationToken ctProducer = ctsProducer.Token;
                Task producer = Task.Run(() => Producer(ctProducer), ctProducer);
                Task consumerBlock = Task.Run(() => ConsumerBlockWrite());
                Task consumerTx = Task.Run(() => ConsumerTXWrite());

                Console.WriteLine("Waiting for Producer and Consumers...");
                Console.ReadLine();
                Console.WriteLine("canceling Producer...");
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
            SHA256 SHA256Gen = SHA256Managed.Create();

            Block itemBlock = new Block();
            itemBlock.Number = new HexBigInteger(0);

            for (; ; itemBlock.Number.Value++ ) // BLOCKS
            {
                Console.WriteLine("Dowloading... (simulated)\t5 sec.");
                Thread.Sleep(5000);
                itemBlock.Number.Value += 1000;
                itemBlock.BlockHash = BytesToHexString(SHA256Gen.ComputeHash(Encoding.ASCII.GetBytes(itemBlock.Number.Value.ToString())));

                bool blockAdded = false;
                while (!blockAdded)
                {
                    if (bcBlock.TryAdd(itemBlock))
                    {
                        blockAdded = true;
                        Console.WriteLine("TryAdd.Block:\t" + itemBlock.Number.Value.ToString() + "\t1 sec.");
                        Thread.Sleep(1000);

                        for (int t = 0; t < 10; t++) // TX
                        {
                            Transaction itemTx = new Transaction();
                            itemTx.BlockNumber = itemBlock.Number;
                            itemTx.TransactionIndex = new HexBigInteger(t);
                            string key = itemTx.BlockNumber.Value.ToString() + "." + itemTx.TransactionIndex.Value.ToString();
                            itemTx.TransactionHash = BytesToHexString(SHA256Gen.ComputeHash(Encoding.ASCII.GetBytes(key)));

                            if (bcTx.TryAdd(itemTx))
                            {
                                Console.WriteLine("TryAdd.Tx:\t" + itemTx.TransactionHash.ToString() + "\t1 sec.");
                                Thread.Sleep(1000);
                            }
                            else
                            {
                                Console.WriteLine("TryAdd.Tx:\t" + "blocked\t3 sec.");
                                Thread.Sleep(3000);
                                t--;
                            }
                        }
                    }
                    else
                    {
                        blockAdded = false;
                        Console.WriteLine("TryAdd.Block:\t" + "blocked\t3 sec.");
                        Thread.Sleep(3000);
                        itemBlock.Number.Value--;
                    }
                }

                if (ct.IsCancellationRequested)
                {
                    Console.WriteLine("Producer canceling...");
                    break;
                }
            }
        }

        const string FILENAME_BLOCKS = @"c:\temp\a-blocks.csv";

        static void ConsumerBlockWrite()
        {
            Block item;
            int nTimesBlocked = 0;
            Console.WriteLine("                                        Uploading block... (simulated)");

            using (StreamWriter file = new StreamWriter(FILENAME_BLOCKS))
                while (true)
                {
                    if (bcBlock.TryTake(out item))
                    {
                        Console.WriteLine("                                        TryTake.Block:\t" + item.Number.Value.ToString());
                        file.WriteLine("\"" + item.Number.Value.ToString() + "\"" + "," +
                                       "\"" + item.BlockHash.ToString() + "\""
                            );
                        nTimesBlocked = 0;
                        //Thread.Sleep(2000);
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

        const string FILENAME_TX = @"c:\temp\a-tx.csv";

        static void ConsumerTXWrite()
        {
            Transaction item;
            int nTimesBlocked = 0;
            Console.WriteLine("                                        Uploading tx... (simulated)");

            using (StreamWriter file = new StreamWriter(FILENAME_TX))
                while (true)
                {
                    if (bcTx.TryTake(out item))
                    {
                        Console.WriteLine("                                        TryTake.Tx:\t" + item.TransactionHash);
                        file.WriteLine("\"" + item.BlockNumber.Value.ToString() + "\"" + "," +
                                       "\"" + item.TransactionIndex.Value.ToString() + "\"" + "," +
                                       "\"" + item.TransactionHash + "\""
                            );
                        nTimesBlocked = 0;
                        //Thread.Sleep(2000);
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

        private readonly static char[] digits = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F' };

        static string BytesToHexString(byte[] bytes)
        {
            char[] hex = new char[bytes.Length * 2];
            int index = 0;
            foreach (byte b in bytes)
            {
                hex[index++] = digits[b >> 4];
                hex[index++] = digits[b & 0x0F];
            }
            return new string(hex);
        }
    }
}
