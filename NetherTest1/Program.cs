using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Nethereum.Hex.HexTypes;
using Nethereum.Web3;

namespace NetherTest1
{
    class Program
    {
        const string senderAddress = "0x3866E56fDb1DE93186a93215f1C13Cd1E4C94174";
        const string contractAddress = "0x60c018fba1aa7c56c7047af764efa54336893a8b"; // "0xaecb9f746658ff48796a6234bb1923bb09e3074a";
        const string contractAbi = @"[{""constant"":false,""inputs"":[{""name"":""index"",""type"":""uint256""},{""name"":""m"",""type"":""string""}],""name"":""setMsg"",""outputs"":[{""name"":""i"",""type"":""uint256""}],""payable"":false,""stateMutability"":""nonpayable"",""type"":""function""},{""constant"":false,""inputs"":[{""name"":""value"",""type"":""int256""}],""name"":""multiply"",""outputs"":[{""name"":""product"",""type"":""int256""}],""payable"":false,""stateMutability"":""nonpayable"",""type"":""function""},{""constant"":true,""inputs"":[{""name"":""index"",""type"":""uint256""}],""name"":""getMsg"",""outputs"":[{""name"":""m"",""type"":""string""}],""payable"":false,""stateMutability"":""view"",""type"":""function""},{""constant"":true,""inputs"":[],""name"":""getProduct"",""outputs"":[{""name"":""product"",""type"":""int256""}],""payable"":false,""stateMutability"":""view"",""type"":""function""},{""inputs"":[],""payable"":false,""stateMutability"":""nonpayable"",""type"":""constructor""}]";
        const string pwd = "PASSWORD";

        static void Main(string[] args)
        {
            var task = DoWork();
            task.Wait();
            Console.WriteLine("Main: done");

            Console.ReadLine();
        }

        static public async Task DoWork()
        {
            var web3 = new Web3();

            var maxBlock = await web3.Eth.Blocks.GetBlockNumber.SendRequestAsync();
            Console.WriteLine("maxBlock: " + maxBlock.Value.ToString());

            var contractAddress2 = "0x9EDCb9A9c4d34b5d6A082c86cb4f117A1394F831"; 
            //                     "0x1B31d19B6a9a942BBf3c197cA1e5EfEde3fF8fF2";

            string oldVal = "";
            for (ulong b = (ulong)maxBlock.Value; b > (ulong)maxBlock.Value-5000; b--)
            {
                var bp = new Nethereum.RPC.Eth.DTOs.BlockParameter(b);
                var stgVal = await web3.Eth.GetStorageAt.SendRequestAsync(contractAddress, new HexBigInteger(3), bp);
                if (stgVal != oldVal)
                {
                    var blK = await web3.Eth.Blocks.GetBlockWithTransactionsByNumber.SendRequestAsync(bp);
                    DateTime dtBLK = UnixTimeStampToDateTime((double)blK.Timestamp.Value);
                    Console.WriteLine("DoWork: dtBLK: " + dtBLK.ToString());

                    for (int i = 0; i <= 5; i++)
                    {
                        var stgValue = await web3.Eth.GetStorageAt.SendRequestAsync(contractAddress, new HexBigInteger(i), bp);
                        Console.WriteLine("stgValue: " + b.ToString() + " " + i.ToString() + " " + stgValue + " " + ConvertHex(stgValue.Substring(2)));
                    }
                    oldVal = stgVal;
                }
            }

            var accts = await web3.Personal.ListAccounts.SendRequestAsync();
            foreach(var a in accts)
            {
                Console.WriteLine("a: " + a);

                var b = await web3.Eth.GetBalance.SendRequestAsync(a);
                string sb = Web3.Convert.FromWei(b.Value).ToString();
                Console.WriteLine("DoWork: balance: " + sb);
            }

            var balance = await web3.Eth.GetBalance.SendRequestAsync(senderAddress);
            string sBalance = Web3.Convert.FromWei(balance.Value).ToString();
            Console.WriteLine("DoWork: balance: " + sBalance);

            var blockNumber = await web3.Eth.Blocks.GetBlockNumber.SendRequestAsync();
            Console.WriteLine("DoWork: blockNumber: " + blockNumber.Value.ToString());

            var block = await web3.Eth.Blocks.GetBlockWithTransactionsByNumber.SendRequestAsync(new HexBigInteger(5));
            DateTime dt = UnixTimeStampToDateTime((double)block.Timestamp.Value);
            Console.WriteLine("DoWork: dt: " + dt.ToString());
            var trans = block.Transactions;
            foreach(var t in trans)
            {
                var bn = t.BlockNumber;
                var th = t.TransactionHash;
                var ti = t.TransactionIndex;
                var nc = t.Nonce;
                var from = t.From;
                var to = t.To;
                var v = t.Value;
                var g = t.Gas;
                var gp = t.GasPrice;
                // TODO
            }

            var contract = web3.Eth.GetContract(contractAbi, contractAddress);

            var setMsg = contract.GetFunction("setMsg");
            var getMsg = contract.GetFunction("getMsg");

            string now = DateTime.UtcNow.ToString() + " UTC";
            Console.WriteLine("DoWork: now: " + now);
            //object[] parms = { 2, now };
            //var setResult = await setMsg.CallAsync<int>(parms);

            var unlockResult = await web3.Personal.UnlockAccount.SendRequestAsync(senderAddress, pwd, 120);
            var txHash1 = await setMsg.SendTransactionAsync(senderAddress, new HexBigInteger(900000), null, 1, "Hello World");
            Console.WriteLine("DoWork: txHash1: " + txHash1.ToString());
            var txHash2 = await setMsg.SendTransactionAsync(senderAddress, new HexBigInteger(900000), null, 2, now);
            Console.WriteLine("DoWork: txHash2: " + txHash2.ToString());

            var txReceipt = await web3.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(txHash2);
            int timeoutCount = 0;
            while (txReceipt == null && timeoutCount < 24)
            {
                Console.WriteLine("DoWork: sleeping...");
                Thread.Sleep(5000);
                txReceipt = await web3.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(txHash2);
                timeoutCount++;
            }
            Console.WriteLine("DoWork: timeoutCount " + timeoutCount.ToString());

            var txReceipt3 = await setMsg.SendTransactionAndWaitForReceiptAsync(senderAddress, new HexBigInteger(900000), null, null, 2, now + " Wait");
            Console.WriteLine("DoWork: txReceipt3: " + txReceipt3.TransactionHash.ToString());
            Console.WriteLine("DoWork: txReceipt3: " + txReceipt3.CumulativeGasUsed.Value.ToString());
        

            var getResult1 = await getMsg.CallAsync<string>(1);
            Console.WriteLine("DoWork: done: " + getResult1.ToString());
            var getResult2 = await getMsg.CallAsync<string>(2);
            Console.WriteLine("DoWork: done: " + getResult2.ToString());
        }

        public static string ConvertHex(String hexString)
        {
            try
            {
                string ascii = string.Empty;

                for (int i = 0; i < hexString.Length; i += 2)
                {
                    String hs = string.Empty;

                    hs = hexString.Substring(i, 2);
                    uint decval = System.Convert.ToUInt32(hs, 16);
                    char character = System.Convert.ToChar(decval);
                    ascii += character;

                }

                return ascii;
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }

            return string.Empty;
        }

        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp); // .ToLocalTime();
            return dtDateTime;
        }

        public static DateTime JavaTimeStampToDateTime(double javaTimeStamp)
        {
            // Java timestamp is milliseconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddMilliseconds(javaTimeStamp); // .ToLocalTime();
            return dtDateTime;
        }
    }
}
