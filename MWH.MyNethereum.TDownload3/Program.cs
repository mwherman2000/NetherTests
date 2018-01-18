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
        static BlockingCollection<BlockItem> bcBlock = new BlockingCollection<BlockItem>(100);
        static BlockingCollection<Transaction> bcTx = new BlockingCollection<Transaction>(1000);
        static BlockingCollection<AddrItem> bcAddr = new BlockingCollection<AddrItem>(2000);

        static Dictionary<string,AddressType> contractAddrs = new Dictionary<string, AddressType>();

        static void Main(string[] args)
        {
            ulong startBlockNumber = 0;
            ulong endBlockNumber = startBlockNumber + 1000000;

            Web3 web3 = new Web3();
            //Web3 web3 = new Web3("https://mainnet.infura.io/hZeiirtHOLO11uuyLySi");

            using (CancellationTokenSource ctsProducer = new CancellationTokenSource())
            {
                Console.WriteLine("Starting Producer and Consumers...");
                CancellationTokenSource ctsConsumerBlock = new CancellationTokenSource();
                CancellationTokenSource ctsConsumerTx = new CancellationTokenSource();
                CancellationTokenSource ctsConsumerAddr = new CancellationTokenSource();

                CancellationToken ctProducer = ctsProducer.Token;
                CancellationToken ctConsumerBlock = ctsConsumerBlock.Token;
                CancellationToken ctConsumerTx = ctsConsumerTx.Token;
                CancellationToken ctConsumerAddr = ctsConsumerAddr.Token;

                Task producer = Task.Run(() => Producer(web3, startBlockNumber, endBlockNumber, ctProducer), ctProducer);
                Task consumerBlock = Task.Run(() => ConsumerBlockWrite(web3, startBlockNumber, endBlockNumber, ctConsumerBlock), ctConsumerBlock);
                Task consumerTx = Task.Run(() => ConsumerTXWrite(web3, startBlockNumber, endBlockNumber, ctConsumerTx), ctConsumerTx);
                Task consumerAddr = Task.Run(() => ConsumerAddrWrite(web3, startBlockNumber, endBlockNumber, ctConsumerAddr), ctConsumerAddr);

                Console.WriteLine("Waiting for Producer and Consumers...");
                while (!producer.IsCompleted)
                {
                    Console.WriteLine("Press Enter to cancel Producer...");
                    if (Console.KeyAvailable)
                    {
                        Console.ReadKey();
                        Console.WriteLine("Canceling Producer...");
                        ctsProducer.Cancel();
                        Console.WriteLine("Producer notified...");
                    }
                    Thread.Sleep(10000);
                }
                Console.WriteLine("Waiting for Producer to exit...");
                producer.Wait();
                Console.WriteLine("Producer exited");

                ctsConsumerBlock.Cancel();
                ctsConsumerTx.Cancel();
                ctsConsumerAddr.Cancel();

                Console.WriteLine("Waiting for ConsumerBlock to exit...");
                consumerBlock.Wait();
                Console.WriteLine("Waiting for ConsumerTx to exit...");
                consumerTx.Wait();
                Console.WriteLine("Waiting for ConsumerAddr to exit...");
                consumerAddr.Wait();
                Console.WriteLine("Consumers exited");

                ctsConsumerBlock.Dispose();
                ctsConsumerTx.Dispose();
                ctsConsumerAddr.Dispose();

                Console.WriteLine("Press Enter to exit...");
                Console.ReadLine();
            }
        }

        static void Producer(Web3 web3, ulong startBlockNumber, ulong endBlockNumber, CancellationToken ct)
        {
            SHA256 SHA256Gen = SHA256Managed.Create();

            for (ulong blockNumber = startBlockNumber; blockNumber <= endBlockNumber; blockNumber++ ) // BLOCKS
            {
                //Console.WriteLine("Dowloading...");

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
                
                bool blockAdded = false;
                while (!blockAdded)
                {
                    if (bcBlock.TryAdd(itemBlock))
                    {
                        blockAdded = true;
                        Console.WriteLine("TryAdd.Block:\t" + itemBlock.Number.Value.ToString());
                        //Thread.Sleep(1000);

                        var blockTx = block.Transactions;
                        int txCount = blockTx.Length;
                        for (int t = 0; t < txCount; t++) // TX
                        {
                            var tx = blockTx[t];

                            Transaction itemTx = new Transaction();
                            itemTx.BlockNumber = tx.BlockNumber;
                            itemTx.TransactionIndex = tx.TransactionIndex;
                            itemTx.TransactionHash = tx.TransactionHash;
                            itemTx.From = tx.From;
                            itemTx.To = tx.To;
                            itemTx.Value = tx.Value;
                            itemTx.Gas = tx.Gas;
                            itemTx.Input = tx.Input;

                            if (bcTx.TryAdd(itemTx))
                            {
                                Console.WriteLine("TryAdd.Tx:\t" + itemTx.TransactionHash.ToString());
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
                                    while (!bcAddr.TryAdd(itemAddr))
                                    {
                                        Console.WriteLine("TryAdd.Addr.From:\t" + "blocked\t1 sec.");
                                        Thread.Sleep(1000);
                                    }
                                    Console.WriteLine("TryAdd.Addr.From:\t" + itemAddr.Address);
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
                                    while (!bcAddr.TryAdd(itemAddr))
                                    {
                                        Console.WriteLine("TryAdd.Addr.To:\t" + "blocked\t1 sec.");
                                        Thread.Sleep(1000);
                                    }
                                    Console.WriteLine("TryAdd.Addr.To:\t" + itemAddr.Address);
                                }
                            }
                            else
                            {
                                Console.WriteLine("TryAdd.Tx:\t" + "blocked\t1 sec.");
                                Thread.Sleep(1000);
                                t--;
                            }
                        }
                    }
                    else
                    {
                        blockAdded = false;
                        Console.WriteLine("TryAdd.Block:\t" + "blocked\t1 sec.");
                        Thread.Sleep(1000);
                        blockNumber--;
                    }
                }

                if (ct.IsCancellationRequested)
                {
                    Console.WriteLine("Producer canceling...");
                    break;
                }
            }
        }

        const string FILENAME_BLOCKS = @"c:\temp\a-blocks-{0}-{1}.csv";

        static void ConsumerBlockWrite(Web3 web3, ulong startBlockNumber, ulong endBlockNumber, CancellationToken ct)
        {
            BlockItem itemBlock;
            int nTimesBlocked = 0;
            Console.WriteLine("                                        Uploading block...");

            string filename = String.Format(FILENAME_BLOCKS, startBlockNumber, endBlockNumber);
            using (StreamWriter file = new StreamWriter(filename))
                while (true)
                {
                    if (bcBlock.TryTake(out itemBlock))
                    {
                        Console.WriteLine("                                        TryTake.Block:\t" + itemBlock.Number.Value.ToString());
                        file.WriteLine("" + itemBlock.Number.Value.ToString() + "" + "," +
                                       "" + itemBlock.Timestamp.Value.ToString() + "" + "," +
                                       "\"" + itemBlock.BlockHash + "\"" + "," +
                                       //"\"" + (String.IsNullOrEmpty(itemBlock.ExtraData) ? "" : itemBlock.ExtraData.ToString()) + "\"" + "," +
                                       "" + itemBlock.TxCount.ToString() + ""

                            );
                        nTimesBlocked = 0;
                        //Thread.Sleep(2000);
                    }
                    else
                    {
                        nTimesBlocked++;
                        //if (nTimesBlocked > 10) break;
                        Console.WriteLine("                                        TryTake.Block:\t" + "blocked\t1 sec.");
                        Thread.Sleep(1000);
                    }

                    if (ct.IsCancellationRequested)
                    {
                        Console.WriteLine("ConsumerBlockWrite canceling...");
                        break;
                    }
                }
            Console.WriteLine("                                        TryTake.Block: exiting");
        }

        const string FILENAME_TX = @"c:\temp\a-tx-{0}-{1}.csv";

        static void ConsumerTXWrite(Web3 web3, ulong startBlockNumber, ulong endBlockNumber, CancellationToken ct)
        {
            Transaction itemTx;
            int nTimesBlocked = 0;
            Console.WriteLine("                                        Uploading tx...");

            string filename = String.Format(FILENAME_TX, startBlockNumber, endBlockNumber);
            using (StreamWriter file = new StreamWriter(filename))
                while (true)
                {
                    if (bcTx.TryTake(out itemTx))
                    {
                        Console.WriteLine("                                        TryTake.Tx:\t" + itemTx.TransactionHash);
                        file.WriteLine("" + itemTx.BlockNumber.Value.ToString() + "" + "," +
                                       "" + itemTx.TransactionIndex.Value.ToString() + "" + "," +
                                       "\"" + itemTx.TransactionHash + "\"" + "," +
                                       "\"" + itemTx.From + "\"" + "," +
                                       "\"" + (String.IsNullOrEmpty(itemTx.To) ? "" : itemTx.To) + "\"" + "," +
                                       "" + itemTx.Value.Value.ToString() + "" + "," +
                                       "" + itemTx.Gas.Value.ToString() + "" + ","
                                       //+ "\"" + (String.IsNullOrEmpty(itemTx.Input) ? "" : Helpers.ConvertHexToASCII(itemTx.Input)) + "\""
                            );
                        nTimesBlocked = 0;
                        //Thread.Sleep(2000);
                    }
                    else
                    {
                        nTimesBlocked++;
                        //if (nTimesBlocked > 10) break;
                        Console.WriteLine("                                        TryTake.Tx:\t" + "blocked\t1 sec.");
                        Thread.Sleep(1000);
                    }

                    if (ct.IsCancellationRequested)
                    {
                        Console.WriteLine("ConsumerTxWrite canceling...");
                        break;
                    }
                }
            Console.WriteLine("                                        TryTake.Tx: exiting");
        }


        const string FILENAME_ADDR = @"c:\temp\a-addr-{0}-{1}.csv";

        static void ConsumerAddrWrite(Web3 web3, ulong startBlockNumber, ulong endBlockNumber, CancellationToken ct)
        {
            AddrItem itemAddr;
            int nTimesBlocked = 0;
            Console.WriteLine("                                        Uploading addr...");

            string filename = String.Format(FILENAME_ADDR, startBlockNumber, endBlockNumber);
            using (StreamWriter file = new StreamWriter(filename))
                while (true)
                {
                    if (bcAddr.TryTake(out itemAddr))
                    {
                        AddressType addrType;
                        try
                        {
                            addrType = contractAddrs[itemAddr.Address];
                            if (addrType == AddressType.Unknown) throw new ArgumentOutOfRangeException(); // force GetAddressType
                            itemAddr.AddrType = addrType;
                        }
                        catch(Exception ex)
                        {
                            var tAddrType = GetAddressType(web3, itemAddr.Address);
                            tAddrType.Wait();
                            addrType = tAddrType.Result;

                            itemAddr.AddrType = addrType;

                            contractAddrs.Add(itemAddr.Address, itemAddr.AddrType);
                            Console.WriteLine("contractAddrs.Count:\t" + contractAddrs.Count.ToString());
                        }

                        Console.WriteLine("                                        TryTake.Addr:\t" + itemAddr.Address);
                        file.WriteLine("\"" + itemAddr.Address + "\"" + "," +
                                       "\"" + itemAddr.AddrType.ToString() + "\"" + "," +
                                       "" + itemAddr.BlockNumber.Value.ToString() + "" + "," +
                                       "\"" + itemAddr.EndPointType.ToString() + "\"" + "," +
                                       "\"" + itemAddr.TxHash + "\"" + "," +
                                       "" + itemAddr.TxTimestamp.Value.ToString() + "" + "," +
                                       "" + itemAddr.TxValue.Value.ToString() + ""
                            );
                        nTimesBlocked = 0;
                        //Thread.Sleep(2000);
                    }
                    else
                    {
                        nTimesBlocked++;
                        //if (nTimesBlocked > 10) break;
                        Console.WriteLine("                                        TryTake.Addr:\t" + "blocked\t1 sec.");
                        Thread.Sleep(1000);
                    }

                    if (ct.IsCancellationRequested)
                    {
                        Console.WriteLine("ConsumerAddrWrite canceling...");
                        break;
                    }
                }
            Console.WriteLine("                                        TryTake.Addr: exiting");
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
