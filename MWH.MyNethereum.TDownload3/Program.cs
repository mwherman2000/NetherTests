using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Web3;
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
        const int ONE_MINUTE = 60 * 1000;
        static int PauseAllTasks = 0;

        static ulong startBlockNumber = 4 * 1000000 + 1;
        static ulong endBlockNumber =   4950000; // 5 * 1000000;

        static double projectedMinutesPrev1 = 0;
        static double projectedMinutesPrev2 = 0;
        static double projectedMinutesPrev3 = 0;

        static bool traceBlockedMsgs = false;

        static bool traceTryAddBlock = false;
        static bool traceTryAddTx = false;
        static bool traceTryAddAddr = false;

        static bool traceTryTakeBlock = false;
        static bool traceTryTakeTx = false;
        static bool traceTryTakeAddr = false;

        static bool addBlock = true;
        static bool addTx = true;
        static bool addAddr = true;

        static double fConsumedItemsMinutesPrev4 = 0.0;
        static double fConsumedItemsMinutesPrev3 = 0.0;
        static double fConsumedItemsMinutesPrev2 = 0.0;
        static double fConsumedItemsMinutesPrev1 = 0.0;

        static ulong nConsumedBlockItemsPrev4 = 0;
        static ulong nConsumedBlockItemsPrev3 = 0;
        static ulong nConsumedBlockItemsPrev2 = 0;
        static ulong nConsumedBlockItemsPrev1 = 0;

        static ulong nProducedBlockItems = 0;
        static ulong nProducedTxItems = 0;
        static ulong nProducedAddrItems = 0;
        static ulong nConsumedBlockItems = 0;
        static ulong nConsumedTxItems = 0;
        static ulong nConsumedAddrItems = 0;

        static BlockingCollection<BlockItem> bcBlock = new BlockingCollection<BlockItem>(1000);
        static BlockingCollection<TxItem> bcTx = new BlockingCollection<TxItem>(2000);
        static BlockingCollection<AddrItem> bcAddr = new BlockingCollection<AddrItem>(4000);

        static Dictionary<string,AddressType> contractAddrs = new Dictionary<string, AddressType>();

        static void Main(string[] args)
        {
            try
            {
                DateTime dtStart = DateTime.Now;
                Console.WriteLine("Starting ProduceItems and Consumers...");

                CancellationTokenSource ctsProduceItems = new CancellationTokenSource();
                CancellationTokenSource ctsConsumeBlocks = new CancellationTokenSource();
                CancellationTokenSource ctsConsumeTxs = new CancellationTokenSource();
                CancellationTokenSource ctsConsumeAddrs = new CancellationTokenSource();

                CancellationToken ctProduceItems = ctsProduceItems.Token;
                CancellationToken ctConsumeBlocks = ctsConsumeBlocks.Token;
                CancellationToken ctConsumeTxs = ctsConsumeTxs.Token;
                CancellationToken ctConsumeAddrs = ctsConsumeAddrs.Token;

                Task produceItems = Task.Run(() => ProduceItems(startBlockNumber, endBlockNumber, ctProduceItems), ctProduceItems);
                Task consumeBlocks = Task.Run(() => ConsumeBlocks(startBlockNumber, endBlockNumber, ctConsumeBlocks), ctConsumeBlocks);
                Task consumeTxs = Task.Run(() => ConsumeTxs(startBlockNumber, endBlockNumber, ctConsumeTxs), ctConsumeTxs);
                Task consumeAddrs = Task.Run(() => ConsumeAddr(startBlockNumber, endBlockNumber, ctConsumeAddrs), ctConsumeAddrs);

                Console.WriteLine("Waiting for ProduceItems and Consumers...");
                while (!produceItems.IsCompleted) // Monitoring loop
                {
                    Console.WriteLine("-------------------------------------------------------");
                    Console.WriteLine("nProducedBlockItems:\t" + nProducedBlockItems.ToString());
                    Console.WriteLine("nProducedTxItems:   \t" + nProducedTxItems.ToString());
                    Console.WriteLine("nProducedAddrItems: \t" + nProducedAddrItems.ToString());
                    Console.WriteLine("nConsumedBlockItems:\t" + nConsumedBlockItems.ToString());
                    Console.WriteLine("nConsumedTxItems:   \t" + nConsumedTxItems.ToString());
                    Console.WriteLine("nConsumedAddrItems: \t" + nConsumedAddrItems.ToString());
                    Console.WriteLine("-------------------------------------------------------");
                    Console.WriteLine("bcBlock.Count:      \t" + bcBlock.Count.ToString());
                    Console.WriteLine("bcTx.Count:         \t" + bcTx.Count.ToString());
                    Console.WriteLine("bcAddr.Count:       \t" + bcAddr.Count.ToString());
                    Console.WriteLine("contractAddrs.Count:\t" + contractAddrs.Count.ToString());
                    Console.WriteLine("-------------------------------------------------------");
                    Console.WriteLine("produceItems.IsCompleted: \t" + produceItems.IsCompleted.ToString());
                    Console.WriteLine("consumeBlocks.IsCompleted:\t" + consumeBlocks.IsCompleted.ToString());
                    Console.WriteLine("consumeTxs.IsCompleted:   \t" + consumeTxs.IsCompleted.ToString());
                    Console.WriteLine("consumeAddrs.IsCompleted: \t" + consumeAddrs.IsCompleted.ToString());
                    Console.WriteLine("-------------------------------------------------------");
                    DateTime dtCurrent = DateTime.Now;
                    Console.WriteLine("Start:    \t" + dtStart.ToString());
                    Console.WriteLine("Current:  \t" + dtCurrent.ToString());
                    TimeSpan tsConsumedItemsCurrent = dtCurrent - dtStart;
                    double fConsumedItemsMinutesCurrent = tsConsumedItemsCurrent.TotalMinutes;
                    Console.WriteLine("Elapsed:  \t" + ((ulong)fConsumedItemsMinutesCurrent).ToString() + " minutes");
                    if (fConsumedItemsMinutesCurrent >= 1.0 && nConsumedBlockItems >= 1)
                    {
                        ulong nBlocksRemaining = (endBlockNumber - startBlockNumber + 1) - nConsumedBlockItems;

                        fConsumedItemsMinutesPrev4 = fConsumedItemsMinutesPrev3;
                        fConsumedItemsMinutesPrev3 = fConsumedItemsMinutesPrev2;
                        fConsumedItemsMinutesPrev2 = fConsumedItemsMinutesPrev1;
                        fConsumedItemsMinutesPrev1 = fConsumedItemsMinutesCurrent;

                        nConsumedBlockItemsPrev4 = nConsumedBlockItemsPrev2;
                        nConsumedBlockItemsPrev3 = nConsumedBlockItemsPrev2;
                        nConsumedBlockItemsPrev2 = nConsumedBlockItemsPrev1;
                        nConsumedBlockItemsPrev1 = nConsumedBlockItems;
 
                        if (fConsumedItemsMinutesPrev4 >= 1.0 && nConsumedBlockItemsPrev4 >= 1)
                        {
                            double fConsumedItemsMinutesDelta = fConsumedItemsMinutesPrev1 - fConsumedItemsMinutesPrev4;
                            ulong nConsumedBlockItemsDelta =   nConsumedBlockItemsPrev1   - nConsumedBlockItemsPrev4;

                            double rateConsumedItemsPerMinuteDelta = nConsumedBlockItemsDelta / fConsumedItemsMinutesDelta;
                            Console.WriteLine("Rate (now):\t" + ((int)rateConsumedItemsPerMinuteDelta).ToString() + " blocks per minute");
                            if (rateConsumedItemsPerMinuteDelta >= 1.0 && nBlocksRemaining >= 1)
                            {
                                double projectedMinutesDelta = (double)nBlocksRemaining / rateConsumedItemsPerMinuteDelta;
                                DateTime dtProjectedDelta = dtCurrent.AddMinutes(projectedMinutesDelta);
                                Console.WriteLine("Projected:  \t" + ((ulong)projectedMinutesDelta).ToString() + " minutes");
                                Console.WriteLine("Projected:  \t" + dtProjectedDelta.ToString());
                            }
                        }

                        double rateConsumedItemsPerMinute = nConsumedBlockItems / fConsumedItemsMinutesCurrent;
                        Console.WriteLine("Rate:      \t" + ((int)rateConsumedItemsPerMinute).ToString() + " blocks per minute");
                        double projectedMinutesCurrent = (double)nBlocksRemaining / rateConsumedItemsPerMinute;
                        projectedMinutesPrev3 = projectedMinutesPrev2;
                        projectedMinutesPrev2 = projectedMinutesPrev1;
                        projectedMinutesPrev1 = projectedMinutesCurrent;
                        if (projectedMinutesPrev3 > 0)
                        {
                            double projectedMinutesAverage = (projectedMinutesPrev3 + projectedMinutesPrev2 + projectedMinutesPrev1) / 3.0;
                            DateTime dtProjected = dtCurrent.AddMinutes(projectedMinutesAverage);
                            Console.WriteLine("Projected:  \t" + ((ulong)projectedMinutesCurrent).ToString() + " minutes"
                                + " (" + (int)(100.0 * fConsumedItemsMinutesCurrent / (fConsumedItemsMinutesCurrent + projectedMinutesCurrent)) + "%)");
                            Console.WriteLine("Projected:  \t" + dtProjected.ToString());
                        }
                    }

                    Console.WriteLine("Press 'X' to cancel ProduceItems.....................");
                    if (Console.KeyAvailable)
                    {
                        var key = Console.ReadKey();
                        switch (key.KeyChar)
                        {
                            case 'a': // Addr off
                                {
                                    traceTryAddAddr = false;
                                    traceTryTakeAddr = false;
                                    break;
                                }
                            case 'A': // Addr on
                                {
                                    traceTryAddAddr = true;
                                    traceTryTakeAddr = true;
                                    break;
                                }
                            case 'b': // Blocks off
                                {
                                    traceTryAddBlock = false;
                                    traceTryTakeBlock = false;
                                    break;
                                }
                            case 'B': // Blocks on
                                {
                                    traceTryAddBlock = true;
                                    traceTryTakeBlock = true;
                                    break;
                                }
                            case 'd': // Blocked messages off
                                {
                                    traceBlockedMsgs = false;
                                    break;
                                }
                            case 'D': // Blocked messages on
                                {
                                    traceBlockedMsgs = true;
                                    break;
                                }
                            case 'p': // Pulse off
                                {
                                    traceTryAddBlock = false;
                                    traceTryTakeBlock = false;
                                    break;
                                }
                            case 'P': // Pulse on
                                {
                                    traceTryAddBlock = true;
                                    traceTryTakeBlock = true;
                                    break;
                                }
                            case 't': // Tx off
                                {
                                    traceTryAddTx = false;
                                    traceTryTakeTx = false;
                                    break;
                                }
                            case 'T': // Tx on
                                {
                                    traceTryAddTx = true;
                                    traceTryTakeTx = true;
                                    break;
                                }
                            case 'X': // Exit
                                {
                                    Console.WriteLine("Canceling ProduceItems...");
                                    ctsProduceItems.Cancel();
                                    Console.WriteLine("ProduceItems notified...");
                                    break;
                                }
                            case 'W': // Wait
                                {
                                    PauseAllTasks = 4;
                                    Console.WriteLine("Waiting...");
                                    Thread.Sleep((int)(ONE_MINUTE));
                                    PauseAllTasks = 0;
                                    break;
                                }
                            default:
                                {
                                    Console.WriteLine();
                                    Console.WriteLine("Enter X to confirm exit");
                                    break;
                                }
                        }
                    }
                    Thread.Sleep(10000);
                }
                Console.WriteLine("Waiting for ProduceItems to exit...");
                produceItems.Wait();
                Console.WriteLine("ProduceItems exited");

                ctsConsumeBlocks.Cancel();
                ctsConsumeTxs.Cancel();
                ctsConsumeAddrs.Cancel();

                Console.WriteLine("Waiting for ConsumeBlocks to exit...");
                consumeBlocks.Wait();
                Console.WriteLine("Waiting for ConsumeTxs to exit...");
                consumeTxs.Wait();
                Console.WriteLine("Waiting for ConsumeAddrs to exit...");
                consumeAddrs.Wait();
                Console.WriteLine("Consumers exited");

                ctsProduceItems.Dispose();
                ctsConsumeBlocks.Dispose();
                ctsConsumeTxs.Dispose();
                ctsConsumeAddrs.Dispose();

                DateTime dtEnd = DateTime.Now;
                Console.WriteLine("Start:\t" + dtStart.ToString());
                Console.WriteLine("End:\t" + dtEnd.ToString());
                TimeSpan ts = dtEnd - dtStart;
                double tsMinutes = ts.TotalMinutes;
                Console.WriteLine("Elapsed:\t" + tsMinutes.ToString() + " minutes");
            }
            catch(Exception ex)
            {
                Console.WriteLine("Exception: " + ex.ToString());
                if (ex.InnerException != null) Console.WriteLine("Exception: " + ex.InnerException.ToString());
            }
            Console.WriteLine("Press Enter to exit...");
            Console.ReadLine();
        }

        static void ProduceItems(ulong startBlockNumber, ulong endBlockNumber, CancellationToken ct)
        {
            SHA256 SHA256Gen = SHA256Managed.Create();

            //Web3 web3 = new Web3();
            //Web3 web3 = new Web3("https://mainnet.infura.io/hZeiirtHOLO11uuyLySi");
            var ipcClient = new Nethereum.JsonRpc.IpcClient.IpcClient("./geth.ipc");
            Web3 web3 = new Web3(ipcClient);

            for (ulong blockNumber = startBlockNumber; blockNumber <= endBlockNumber; blockNumber++ ) // BLOCKS
            {
                if (PauseAllTasks > 0)
                {
                    PauseAllTasks--;
                    Console.WriteLine("ProduceAllItems.Blocks:\tWaiting...");
                    Thread.Sleep(ONE_MINUTE);
                }

                while (bcAddr.Count > bcAddr.BoundedCapacity * 0.5 || bcTx.Count > bcTx.BoundedCapacity * 0.5)
                {
                    if (traceBlockedMsgs) Console.WriteLine("ProduceAllItems.Blocks:\tWaiting for Addr/Tx...");
                    Thread.Sleep(50);
                }

                var taskGetBlockWithTx = GetBlockWithTx(web3, blockNumber);
                taskGetBlockWithTx.Wait();
                var block = taskGetBlockWithTx.Result;

                BlockItem itemBlock = new BlockItem();
                itemBlock.Number = new HexBigInteger(0);
                if (block.Number != null) itemBlock.Number.Value = block.Number.Value;
                itemBlock.BlockHash = block.BlockHash;
                itemBlock.Timestamp = block.Timestamp;
                itemBlock.ExtraData = itemBlock.ExtraData;
                itemBlock.TxCount = 0;
                if (block.Transactions != null)
                {
                    itemBlock.TxCount = block.Transactions.Length;
                }

                DateTime dt = Helpers.UnixTimeStampToDateTime((double)itemBlock.Timestamp.Value);
                itemBlock.BlYear = dt.Year;
                itemBlock.BlMonth = dt.Month;
                itemBlock.BlWeek = dt.DayOfYear / 7;
                itemBlock.BlDayOfYear = dt.DayOfYear;
                itemBlock.BlDayOfMonth = dt.Day;
                itemBlock.BlDayOfWeek = Convert.ToInt32(dt.DayOfWeek);
                itemBlock.BlHour = dt.Hour;

                bool blockAdded = false;
                bool txAdded = false;
                while (!blockAdded)
                {
                    if (bcBlock.TryAdd(itemBlock))
                    {
                        nProducedBlockItems++;
                        if (!addBlock)
                        {
                            BlockItem itemBlockDummy = new BlockItem();
                            bcBlock.TryTake(out itemBlockDummy);
                        }

                        blockAdded = true;
                        if (traceTryAddBlock) Console.WriteLine("TryAdd.Block:\t" + itemBlock.Number.Value.ToString());
                        //Thread.Sleep(1000);

                        var blockTx = block.Transactions;
                        int txCount = blockTx.Length;
                        for (int t = 0; t < txCount; t++) // TX
                        {
                            if (PauseAllTasks > 0)
                            {
                                PauseAllTasks--;
                                Console.WriteLine("ProduceAllItems.Txs:\tWaiting...");
                                Thread.Sleep(ONE_MINUTE);
                            }

                            var tx = blockTx[t];

                            TxItem itemTx = new TxItem();
                            itemTx.BlockNumber = tx.BlockNumber;
                            itemTx.TransactionIndex = tx.TransactionIndex;
                            itemTx.TransactionHash = tx.TransactionHash;
                            itemTx.From = tx.From;
                            itemTx.To = tx.To;
                            itemTx.Value = tx.Value;
                            itemTx.Gas = tx.Gas;
                            itemTx.Input = tx.Input;

                            itemTx.TxTimestamp = itemBlock.Timestamp;
                            DateTime dtTx = Helpers.UnixTimeStampToDateTime((double)itemTx.TxTimestamp.Value);
                            itemTx.TxYear = dtTx.Year;
                            itemTx.TxMonth = dtTx.Month;
                            itemTx.TxWeek = dtTx.DayOfYear / 7;
                            itemTx.TxDayOfYear = dtTx.DayOfYear;
                            itemTx.TxDayOfMonth = dtTx.Day;
                            itemTx.TxDayOfWeek = Convert.ToInt32(dtTx.DayOfWeek);
                            itemTx.TxHour = dtTx.Hour;


                            if (bcTx.TryAdd(itemTx))
                            {
                                nProducedTxItems++;
                                if (!addTx)
                                {
                                    TxItem itemTxDummy = new TxItem();
                                    bcTx.TryTake(out itemTxDummy);
                                }

                                txAdded = true;
                                if (traceTryAddTx) Console.WriteLine("TryAdd.Tx:\t" + itemTx.TransactionHash.ToString());
                                //Thread.Sleep(1000);

                                if (!String.IsNullOrEmpty(tx.From))
                                {
                                    AddressType addrType = AddressType.Unknown;

                                    AddrItem itemAddr = new AddrItem();
                                    itemAddr.Address = tx.From;
                                    itemAddr.AddrType = addrType;
                                    itemAddr.BlockNumber = itemTx.BlockNumber;
                                    itemAddr.EndPointType = AddressEndpointType.From;
                                    itemAddr.TxHash = itemTx.TransactionHash;
                                    itemAddr.TxTimestamp = itemBlock.Timestamp;
                                    itemAddr.TxValue = itemTx.Value;

                                    itemAddr.TxTimestamp = itemTx.TxTimestamp;
                                    DateTime dtAddr = Helpers.UnixTimeStampToDateTime((double)itemTx.TxTimestamp.Value);
                                    itemAddr.TxYear = dtAddr.Year;
                                    itemAddr.TxMonth = dtAddr.Month;
                                    itemAddr.TxWeek = dtAddr.DayOfYear / 7;
                                    itemAddr.TxDayOfYear = dtAddr.DayOfYear;
                                    itemAddr.TxDayOfMonth = dtAddr.Day;
                                    itemAddr.TxDayOfWeek = Convert.ToInt32(dtAddr.DayOfWeek);
                                    itemAddr.TxHour = dtAddr.Hour;

                                    while (!bcAddr.TryAdd(itemAddr))
                                    {
                                        if (PauseAllTasks > 0)
                                        {
                                            PauseAllTasks--;
                                            Console.WriteLine("ProduceAllItems.Addr.From:\tWaiting...");
                                            Thread.Sleep(ONE_MINUTE);
                                        }

                                        //Console.Beep();
                                        if (traceBlockedMsgs) Console.WriteLine("TryAdd.Addr.From:\t" + " Blocked\t1 sec.");
                                        Thread.Sleep(1000);
                                    }
                                    nProducedAddrItems++;
                                    if (!addAddr)
                                    {
                                        AddrItem itemAddrDummy = new AddrItem();
                                        bcAddr.TryTake(out itemAddrDummy);
                                    }
                                    if (traceTryAddAddr) Console.WriteLine("TryAdd.Addr.From:\t" + itemAddr.Address);
                                }

                                if (!String.IsNullOrEmpty(tx.To))
                                {
                                    AddressType addrType = AddressType.Unknown;

                                    AddrItem itemAddr = new AddrItem();
                                    itemAddr.Address = tx.To;
                                    itemAddr.AddrType = addrType;
                                    itemAddr.BlockNumber = itemTx.BlockNumber;
                                    itemAddr.EndPointType = AddressEndpointType.To;
                                    itemAddr.TxHash = itemTx.TransactionHash;
                                    itemAddr.TxTimestamp = itemBlock.Timestamp;
                                    itemAddr.TxValue = itemTx.Value;

                                    itemAddr.TxTimestamp = itemTx.TxTimestamp;
                                    DateTime dtAddr = Helpers.UnixTimeStampToDateTime((double)itemTx.TxTimestamp.Value);
                                    itemAddr.TxYear = dtAddr.Year;
                                    itemAddr.TxMonth = dtAddr.Month;
                                    itemAddr.TxWeek = dtAddr.DayOfYear / 7;
                                    itemAddr.TxDayOfYear = dtAddr.DayOfYear;
                                    itemAddr.TxDayOfMonth = dtAddr.Day;
                                    itemAddr.TxDayOfWeek = Convert.ToInt32(dtAddr.DayOfWeek);
                                    itemAddr.TxHour = dtAddr.Hour;

                                    while (!bcAddr.TryAdd(itemAddr))
                                    {
                                        if (PauseAllTasks > 0)
                                        {
                                            PauseAllTasks--;
                                            Console.WriteLine("ProduceAllItems.Addr.To:\tWaiting...");
                                            Thread.Sleep(ONE_MINUTE);
                                        }

                                        //Console.Beep();
                                        if (traceBlockedMsgs) Console.WriteLine("TryAdd.Addr.To:\t" + " Blocked\t1 sec.");
                                        Thread.Sleep(1000);
                                    }
                                    nProducedAddrItems++;
                                    if (!addAddr)
                                    {
                                        AddrItem itemAddrDummy = new AddrItem();
                                        bcAddr.TryTake(out itemAddrDummy);
                                    }
                                    if (traceTryAddAddr) Console.WriteLine("TryAdd.Addr.To:\t" + itemAddr.Address);
                                }
                            }
                            else
                            {
                                //Console.Beep();
                                if (txAdded == true) t--;
                                txAdded = false;
                                if (traceBlockedMsgs) Console.WriteLine("TryAdd.Tx:\t" + " Blocked\t1 sec.");
                                Thread.Sleep(1000);
                            }
                        }
                    }
                    else
                    {
                        if (blockAdded) blockNumber--;
                        blockAdded = false;
                        if (traceBlockedMsgs) Console.WriteLine("TryAdd.Block:\t" + " Blocked\t1 sec.");
                        Thread.Sleep(1000);
                    }
                }

                if (ct.IsCancellationRequested)
                {
                    Console.WriteLine("ProduceItems:\tCanceling...");
                    break;
                }
            }
        }

        const string FILENAME_BLOCKS = @"c:\temp\a-blocks-{0}-{1}.csv";

        static void ConsumeBlocks(ulong startBlockNumber, ulong endBlockNumber, CancellationToken ct)
        {
            //Web3 web3 = new Web3();
            //Web3 web3 = new Web3("https://mainnet.infura.io/hZeiirtHOLO11uuyLySi");
            var ipcClient = new Nethereum.JsonRpc.IpcClient.IpcClient("./geth.ipc");
            Web3 web3 = new Web3(ipcClient);

            BlockItem itemBlock;
            int nTimesBlocked = 0;

            string filename = String.Format(FILENAME_BLOCKS, startBlockNumber, endBlockNumber);
            using (StreamWriter file = new StreamWriter(filename))
                while (addBlock)
                {
                    if (PauseAllTasks > 0)
                    {
                        PauseAllTasks--;
                        Console.WriteLine("ConsumeBlocks:\tWaiting...");
                        Thread.Sleep(ONE_MINUTE);
                    }

                    if (bcBlock.TryTake(out itemBlock))
                    {
                        if (traceTryTakeBlock) Console.WriteLine("                                        TryTake.Block:\t" + itemBlock.Number.Value.ToString());
                        nConsumedBlockItems++;
                        file.WriteLine("" + itemBlock.Number.Value.ToString() + "" + "," +
                                       "" + itemBlock.Timestamp.Value.ToString() + "" + "," +
                                       "\"" + itemBlock.BlockHash + "\"" + "," +
                                       //"\"" + (String.IsNullOrEmpty(itemBlock.ExtraData) ? "" : itemBlock.ExtraData.ToString()) + "\"" + "," +
                                       "" + itemBlock.TxCount.ToString() + "" + "," +
                                       "" + itemBlock.BlYear.ToString() + "" + "," +
                                       "" + itemBlock.BlMonth.ToString() + "" + "," +
                                       "" + itemBlock.BlWeek.ToString() + "" + "," +
                                       "" + itemBlock.BlDayOfYear.ToString() + "" + "," +
                                       "" + itemBlock.BlDayOfMonth.ToString() + "" + "," +
                                       "" + itemBlock.BlDayOfWeek.ToString() + "" + "," +
                                       "" + itemBlock.BlHour.ToString() + ""
                            );
                        nTimesBlocked = 0;
                        //Thread.Sleep(2000);
                    }
                    else
                    {
                        nTimesBlocked++;
                        //if (nTimesBlocked > 10) break;
                        if (traceBlockedMsgs) Console.WriteLine("                                        TryTake.Block:\t" + " Blocked\t1 sec.");
                        Thread.Sleep(1000);
                    }

                    if (ct.IsCancellationRequested)
                    {
                        Console.WriteLine("ConsumeBlocks:\tCanceling...");
                        break;
                    }
                }
            Console.WriteLine("                                        TryTake.Block:\tExiting");
        }

        const string FILENAME_TX = @"c:\temp\a-tx-{0}-{1}.csv";

        static void ConsumeTxs(ulong startBlockNumber, ulong endBlockNumber, CancellationToken ct)
        {
            //Web3 web3 = new Web3();
            //Web3 web3 = new Web3("https://mainnet.infura.io/hZeiirtHOLO11uuyLySi");
            var ipcClient = new Nethereum.JsonRpc.IpcClient.IpcClient("./geth.ipc");
            Web3 web3 = new Web3(ipcClient);

            TxItem itemTx;
            int nTimesBlocked = 0;

            string filename = String.Format(FILENAME_TX, startBlockNumber, endBlockNumber);
            using (StreamWriter file = new StreamWriter(filename))
                while (addTx)
                {
                    if (PauseAllTasks > 0)
                    {
                        PauseAllTasks--;
                        Console.WriteLine("ConsumeTxss:\tWaiting...");
                        Thread.Sleep(ONE_MINUTE);
                    }

                    if (bcTx.TryTake(out itemTx))
                    {
                        if (traceTryTakeTx) Console.WriteLine("                                        TryTake.Tx:\t" + itemTx.TransactionHash);
                        nConsumedTxItems++;
                        file.WriteLine("" + itemTx.BlockNumber.Value.ToString() + "" + "," +
                                       "" + itemTx.TransactionIndex.Value.ToString() + "" + "," +
                                       "\"" + itemTx.TransactionHash + "\"" + "," +
                                       "\"" + itemTx.From + "\"" + "," +
                                       "\"" + (String.IsNullOrEmpty(itemTx.To) ? "" : itemTx.To) + "\"" + "," +
                                       "" + itemTx.Value.Value.ToString() + "" + "," +
                                       "" + itemTx.Gas.Value.ToString() + "" + "," +
                                       //+ "\"" + (String.IsNullOrEmpty(itemTx.Input) ? "" : Helpers.ConvertHexToASCII(itemTx.Input)) + "\"" +
                                       "" + itemTx.TxTimestamp.Value.ToString() + "" + "," + 
                                       "" + itemTx.TxYear.ToString() + "" + "," +
                                       "" + itemTx.TxMonth.ToString() + "" + "," +
                                       "" + itemTx.TxWeek.ToString() + "" + "," +
                                       "" + itemTx.TxDayOfYear.ToString() + "" + "," +
                                       "" + itemTx.TxDayOfMonth.ToString() + "" + "," +
                                       "" + itemTx.TxDayOfWeek.ToString() + "" + "," +
                                       "" + itemTx.TxHour.ToString() + ""
                            );
                        nTimesBlocked = 0;
                        //Thread.Sleep(2000);
                    }
                    else
                    {
                        nTimesBlocked++;
                        //if (nTimesBlocked > 10) break;
                        if (traceBlockedMsgs) Console.WriteLine("                                        TryTake.Tx:\t" + " Blocked\t1 sec.");
                        Thread.Sleep(1000);
                    }

                    if (ct.IsCancellationRequested)
                    {
                        Console.WriteLine("ConsumeTxs:\tCanceling...");
                        break;
                    }
                }
            Console.WriteLine("                                        TryTake.Tx:\tExiting");
        }


        const string FILENAME_ADDR = @"c:\temp\a-addr-{0}-{1}.csv";

        static void ConsumeAddr(ulong startBlockNumber, ulong endBlockNumber, CancellationToken ct)
        {
            //Web3 web3 = new Web3();
            //Web3 web3 = new Web3("https://mainnet.infura.io/hZeiirtHOLO11uuyLySi");
            var ipcClient = new Nethereum.JsonRpc.IpcClient.IpcClient("./geth.ipc");
            Web3 web3 = new Web3(ipcClient);

            AddrItem itemAddr;
            int nTimesBlocked = 0;

            string filename = String.Format(FILENAME_ADDR, startBlockNumber, endBlockNumber);
            using (StreamWriter file = new StreamWriter(filename))
                while (addAddr)
                {
                    if (PauseAllTasks > 0)
                    {
                        PauseAllTasks--;
                        Console.WriteLine("ConsumeAddr:\tWaiting...");
                        Thread.Sleep(ONE_MINUTE);
                    }

                    AddressType addrType = AddressType.Unknown;
                    if (bcAddr.TryTake(out itemAddr))
                    {
                        if (contractAddrs.Keys.Contains(itemAddr.Address))
                        {
                            addrType = contractAddrs[itemAddr.Address];
                        }
                        else
                        {
                            Task<AddressType> tAddrType = null;
                            bool success = false;
                            int nAttempts = 0;
                            while (!success)
                            {
                                try
                                {
                                    tAddrType = GetAddressType(web3, itemAddr.Address);
                                    success = tAddrType.Wait(1000);
                                    if (success)
                                    {
                                        addrType = tAddrType.Result;
                                    }
                                    else
                                    {
                                        nAttempts++;
                                        if (nAttempts >= 5)
                                        {
                                            throw new TimeoutException();
                                        }
                                    }
                                }
                                catch(Exception ex)
                                {
                                    Console.WriteLine("                                        TryTake.contractAddrs:\tretry " + itemAddr.Address);
                                    success = false;
                                    Thread.Sleep(100);
                                }
                            }
                            contractAddrs.Add(itemAddr.Address, addrType);
                            itemAddr.AddrType = addrType;
                            if (traceTryTakeAddr) Console.WriteLine("                                        TryTake.contractAddrs:\t" + itemAddr.Address + " " + itemAddr.AddrType.ToString());
                        }

                        if (traceTryTakeAddr) Console.WriteLine("                                        TryTake.Addr:\t" + itemAddr.Address);
                        nConsumedAddrItems++;
                        file.WriteLine("\"" + itemAddr.Address + "\"" + "," +
                                       "\"" + itemAddr.AddrType.ToString() + "\"" + "," +
                                       "" + itemAddr.BlockNumber.Value.ToString() + "" + "," +
                                       "\"" + itemAddr.EndPointType.ToString() + "\"" + "," +
                                       "\"" + itemAddr.TxHash + "\"" + "," +
                                       "" + itemAddr.TxTimestamp.Value.ToString() + "" + "," +
                                       "" + itemAddr.TxValue.Value.ToString() + "" + "," +
                                       "" + itemAddr.TxYear.ToString() + "" + "," +
                                       "" + itemAddr.TxMonth.ToString() + "" + "," +
                                       "" + itemAddr.TxWeek.ToString() + "" + "," +
                                       "" + itemAddr.TxDayOfYear.ToString() + "" + "," +
                                       "" + itemAddr.TxDayOfMonth.ToString() + "" + "," +
                                       "" + itemAddr.TxDayOfWeek.ToString() + "" + "," +
                                       "" + itemAddr.TxHour.ToString() + ""
                            );
                        nTimesBlocked = 0;
                        //Thread.Sleep(2000);
                    }
                    else
                    {
                        nTimesBlocked++;
                        //if (nTimesBlocked > 10) break;
                        if (traceBlockedMsgs) Console.WriteLine("                                        TryTake.Addr:\t" + " Blocked\t0.1 sec.");
                        Thread.Sleep(100);
                    }

                    if (ct.IsCancellationRequested)
                    {
                        Console.WriteLine("ConsumeAddr:\tCanceling...");
                        break;
                    }
                }
            Console.WriteLine("                                        TryTake.Addr:\tExiting");
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

        static async Task<BlockWithTransactions> GetBlockWithTx(Web3 web3, ulong blockNumber)
        {
            var blockNumberParameter = new Nethereum.RPC.Eth.DTOs.BlockParameter(blockNumber);
            var block = await web3.Eth.Blocks.GetBlockWithTransactionsByNumber.SendRequestAsync(blockNumberParameter);
            return block;
        }

        static async Task<AddressType> GetAddressType(Web3 web3, string addr)
        {
            AddressType addrType = AddressType.Unknown;
            var code = await web3.Eth.GetCode.SendRequestAsync(addr);
            if (code == null || code == "0x")
            {
                addrType = AddressType.Account;
            }
            else
            {
                addrType = AddressType.Contract;
            }
            return addrType;
        }
    }
}
