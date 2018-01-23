using Nethereum.RPC.Eth.DTOs;
using Nethereum.Web3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IPCEndPointTest1
{
    class Program
    {
        static void Main(string[] args)
        {
            //Web3 web3 = new Web3();
            var ipcClient = new Nethereum.JsonRpc.IpcClient.IpcClient("./geth.ipc");
            Web3 web3 = new Web3(ipcClient);
            //Web3 web3 = new Web3("https://mainnet.infura.io/hZeiirtHOLO11uuyLySi");

            ulong startBlockNumber = 0;
            ulong endBlockNumber = 1000000;

            DateTime dtStart = DateTime.Now;
            for (ulong blockNumber = startBlockNumber; blockNumber <= endBlockNumber; blockNumber++) // BLOCKS
            {
                var taskGetBlockWithTx = GetBlockWithTx(web3, blockNumber);
                taskGetBlockWithTx.Wait();
                var block = taskGetBlockWithTx.Result;
                if (blockNumber % 1000 == 0) Console.Write(".");
            }
            Console.WriteLine();
            DateTime dtEnd = DateTime.Now;
            TimeSpan tsElapsed = dtEnd - dtStart;
            double fElapsedMinutes = tsElapsed.TotalMinutes;
            Console.WriteLine("fElapsedMinutes:t" + fElapsedMinutes.ToString());

            Console.WriteLine("Press Enter to exit...");
            Console.WriteLine();
        }

        static async Task<BlockWithTransactions> GetBlockWithTx(Web3 web3, ulong blockNumber)
        {
            var blockNumberParameter = new Nethereum.RPC.Eth.DTOs.BlockParameter(blockNumber);
            var block = await web3.Eth.Blocks.GetBlockWithTransactionsByNumber.SendRequestAsync(blockNumberParameter);
            return block;
        }
    }
}
