using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Nethereum.Hex.HexTypes;
using Nethereum.Web3;

//using MWH.MyNethereum;

namespace MWH.MyNethereum.QuickRef
{
    class Program
    {
        static string azCoinbase = "42b37f56f7d94fc7de1b0cab83dbb9b1ac23b02b";
        static string azAccount3 = "0xcE8443E4d2596d36640e393E306C216221d01aDe".ToLower();
        static string azAccount4 = "0x0f27aA5A5be481a83d71a08cd726904ea4C26B1f".ToLower();
        static string azAccount5 = "0x2C1060E7a87bB91b931B025705C2f224b422445d".ToLower();

        //const string coinbaseSentAddress = "0x4504d2bf0378f3aa6f5b20214e1334fcfb02f10b";
        //const string otherMainnetAddress = "0x3866e56fdb1de93186a93215f1c13cd1e4c94174";
        //const string senderAddress1 = "0x3866E56fDb1DE93186a93215f1C13Cd1E4C94174";
        const string senderPassword1 = "PASSWORD";

        //const string receiverAddress1 = "0x7fba3a49aa2acaebaebe722ded44817b4cc60460";

        const string contractAddressTest3 = "0x059ffc13aa80990430e4d75eaee573642d5bcd20"; // Test3
        const string contractAbiTest3 = @"[{""constant"":false,""inputs"":[{""name"":""i"",""type"":""uint256""},{""name"":""m"",""type"":""string""}],""name"":""setMsg"",""outputs"":[{""name"":""mi"",""type"":""uint256""}],""payable"":false,""stateMutability"":""nonpayable"",""type"":""function""},{""constant"":false,""inputs"":[{""name"":""value"",""type"":""int256""}],""name"":""multiply"",""outputs"":[{""name"":""product"",""type"":""int256""}],""payable"":false,""stateMutability"":""nonpayable"",""type"":""function""},{""constant"":true,""inputs"":[{""name"":""index"",""type"":""uint256""}],""name"":""getMsg"",""outputs"":[{""name"":""m"",""type"":""string""}],""payable"":false,""stateMutability"":""view"",""type"":""function""},{""constant"":true,""inputs"":[],""name"":""_product"",""outputs"":[{""name"":"""",""type"":""int256""}],""payable"":false,""stateMutability"":""view"",""type"":""function""},{""constant"":true,""inputs"":[],""name"":""getProduct"",""outputs"":[{""name"":""product"",""type"":""int256""}],""payable"":false,""stateMutability"":""view"",""type"":""function""},{""inputs"":[],""payable"":false,""stateMutability"":""nonpayable"",""type"":""constructor""},{""anonymous"":false,""inputs"":[{""indexed"":true,""name"":""sender"",""type"":""address""},{""indexed"":false,""name"":""oldProduct"",""type"":""int256""},{""indexed"":false,""name"":""value"",""type"":""int256""},{""indexed"":false,""name"":""newProduct"",""type"":""int256""}],""name"":""MultipliedEvent"",""type"":""event""},{""anonymous"":false,""inputs"":[{""indexed"":true,""name"":""sender"",""type"":""address""},{""indexed"":true,""name"":""ind"",""type"":""uint256""},{""indexed"":false,""name"":""msg"",""type"":""string""}],""name"":""NewMessageEvent"",""type"":""event""}]";

        const string contractAddressTest4 = "0x6af34bb0986c06fd97a80b0135ac54a603fecae9"; // Test4
        const string contractAbiTest4 = @"[{""constant"":false,""inputs"":[{""name"":""i"",""type"":""uint256""},{""name"":""m"",""type"":""string""}],""name"":""setMsg"",""outputs"":[{""name"":""mi"",""type"":""uint256""}],""payable"":false,""stateMutability"":""nonpayable"",""type"":""function""},{""constant"":false,""inputs"":[{""name"":""value"",""type"":""int256""}],""name"":""multiply"",""outputs"":[{""name"":""product"",""type"":""int256""}],""payable"":false,""stateMutability"":""nonpayable"",""type"":""function""},{""constant"":true,""inputs"":[{""name"":""index"",""type"":""uint256""}],""name"":""getMsg"",""outputs"":[{""name"":""m"",""type"":""string""}],""payable"":false,""stateMutability"":""view"",""type"":""function""},{""constant"":true,""inputs"":[],""name"":""_product"",""outputs"":[{""name"":"""",""type"":""int256""}],""payable"":false,""stateMutability"":""view"",""type"":""function""},{""constant"":true,""inputs"":[],""name"":""getProduct"",""outputs"":[{""name"":""product"",""type"":""int256""}],""payable"":false,""stateMutability"":""view"",""type"":""function""},{""constant"":false,""inputs"":[{""name"":""_id"",""type"":""bytes32""}],""name"":""deposit"",""outputs"":[],""payable"":true,""stateMutability"":""payable"",""type"":""function""},{""inputs"":[],""payable"":false,""stateMutability"":""nonpayable"",""type"":""constructor""},{""anonymous"":false,""inputs"":[{""indexed"":true,""name"":""sender"",""type"":""address""},{""indexed"":false,""name"":""oldProduct"",""type"":""int256""},{""indexed"":false,""name"":""value"",""type"":""int256""},{""indexed"":false,""name"":""newProduct"",""type"":""int256""}],""name"":""MultipliedEvent"",""type"":""event""},{""anonymous"":false,""inputs"":[{""indexed"":true,""name"":""sender"",""type"":""address""},{""indexed"":true,""name"":""ind"",""type"":""uint256""},{""indexed"":false,""name"":""msg"",""type"":""string""}],""name"":""NewMessageEvent"",""type"":""event""},{""anonymous"":false,""inputs"":[{""indexed"":true,""name"":""timestamp"",""type"":""uint256""},{""indexed"":true,""name"":""from"",""type"":""address""},{""indexed"":true,""name"":""id"",""type"":""bytes32""},{""indexed"":false,""name"":""_value"",""type"":""uint256""}],""name"":""DepositReceipt"",""type"":""event""}]";

        static public string LastProtocolVersion = "";
        static public string LastTxHash = "";
        static public string LastCoinbase = "";
        static public HexBigInteger LastBalance = new HexBigInteger(0);
        static public HexBigInteger LastMaxBlockNumber = new HexBigInteger(0);

        static void Main(string[] args)
        {
            const int ONE_WEI = 1;

            Web3 web3 = new Web3("http://eth1ehgy7hyj.eastus.cloudapp.azure.com:8545");
            //Web3 web3 = new Web3("https://mainnet.infura.io:8545");
            //Web3 web3 = new Web3();

            var task0 = TaskExamples.GetProtocolVersionExample(web3);
            task0.Wait();
            LastProtocolVersion = task0.Result;
            Console.WriteLine("GetProtocolVersionExample: done");

            var taskcb = TaskExamples.GetCoinbaseExample(web3);
            taskcb.Wait();
            LastCoinbase = taskcb.Result;
            Console.WriteLine("GetCoinbaseExample: done");

            var task1 = TaskExamples.GetMaxBlockExample(web3);
            task1.Wait();
            LastMaxBlockNumber = task1.Result;
            Console.WriteLine("GetMaxBlockExample: done");

            //var task6 = TaskExamples.ListPersonalAccountsExample(web3);
            //task6.Wait();
            //Console.WriteLine("ListPersonalAccountsExample: done");

            var task2 = TaskExamples.GetAccountBalanceExample(web3, azCoinbase);
            task2.Wait();
            Console.WriteLine("GetAccountBalanceExample: done");
            task2 = TaskExamples.GetAccountBalanceExample(web3, azAccount3);
            task2.Wait();
            task2 = TaskExamples.GetAccountBalanceExample(web3, azAccount4);
            task2.Wait();
            task2 = TaskExamples.GetAccountBalanceExample(web3, azAccount5);
            task2.Wait();
            LastBalance = task2.Result;
            Console.WriteLine("GetAccountBalanceExample: done");

            //var task3 = TaskExamples.SendEtherToAccountExample(web3, azAccount4, senderPassword1, azAccount5, ONE_WEI);
            //task3.Wait();
            //Console.WriteLine("SendEtherToAccountExample: done");
            //task3 = TaskExamples.SendEtherToAccountExample(web3, azAccount4, senderPassword1, azAccount3, 0);
            //task3.Wait();
            //LastTxHash = task3.Result;
            //Console.WriteLine("SendEtherToAccountExample: done");

            //var task4 = TaskExamples.WaitForTxReceiptExample(web3, LastTxHash);
            //task4.Wait();
            //Console.WriteLine("WaitForTxReceiptExample: done");

            ulong lastBlockNumber = (ulong)LastMaxBlockNumber.Value;
            //ulong lastBlockNumber = 4825497;
            var task5 = TaskExamples.ScanBlocksExample(web3, lastBlockNumber-10, lastBlockNumber);
            task5.Wait();
            Console.WriteLine("ScanBlocksExample: done");

            var task7 = TaskExamples.ScanTxExample(web3, (ulong)0, (ulong)LastMaxBlockNumber.Value);
            task7.Wait();
            Console.WriteLine("ScanTxExample: done");

            //var task8 = TaskExamples.InteractWithExistingContractExample(web3, senderAddress1, senderPassword1, contractAddressTest3, contractAbiTest3);
            //task8.Wait();
            //Console.WriteLine("InteractWithExistingContractExample: done");

            //var task9 = TaskExamples.InteractWithExistingContractWithEventsExample(web3, senderAddress1, senderPassword1, contractAddressTest3, contractAbiTest3);
            //task9.Wait();
            //Console.WriteLine("InteractWithExistingContractWithEventsExample: done");

            //var task10 = TaskExamples.GetAllChangesExample(web3, senderAddress1, senderPassword1, contractAddressTest4, contractAbiTest4);
            //task10.Wait();
            //Console.WriteLine("GetAllChangesExample: done");

            //var task11 = TaskExamples.GetContractValuesHistoryUniqueOffsetValueExample(web3, contractAddressTest3, LastMaxBlockNumber, 500, 3);
            //task11.Wait();
            //Console.WriteLine("GetContractValuesHistoryUniqueOffsetValueExample: done");

            //var task12 = TaskExamples.SendEtherToAccountWithExtraDataExample(web3, senderAddress1, senderPassword1, receiverAddress1, 123, DateTime.Now.ToString() + " extraData");
            //task12.Wait();
            //Console.WriteLine("SendEtherToAccountWithExtraDataExample: done");

            //var tokenSource = new CancellationTokenSource();
            //var token = tokenSource.Token;
            //var taskMonitorDepositEvents = Task.Run(() => TaskExamples.MonitorDepositEventsExample(web3, contractAddressTest4, contractAbiTest4, token), token);

            //var task13 = TaskExamples.InteractWithExistingContractWithEtherAndEventsExample(web3, senderAddress1, senderPassword1, contractAddressTest4, contractAbiTest4, 678);
            //task13.Wait();
            //task13 = TaskExamples.InteractWithExistingContractWithEtherAndEventsExample(web3, senderAddress1, senderPassword1, contractAddressTest4, contractAbiTest4, 678);
            //task13.Wait();
            //Console.WriteLine("InteractWithExistingContractWithEtherAndEventsExample: done");

            //Thread.Sleep(10 * 1000);
            //tokenSource.Cancel();
            //Console.WriteLine("MonitorDepositEventsExample: done");

            //var task14 = TaskExamples.IsAddressForAContract(web3, contractAddressTest4);
            //task14.Wait();
            //bool result = task14.Result;
            //Console.WriteLine("IsAddressForAContract: done: " + result.ToString());
            //task14 = TaskExamples.IsAddressForAContract(web3, senderAddress1);
            //task14.Wait();
            //result = task14.Result;
            //Console.WriteLine("IsAddressForAContract: done: " + result.ToString());

            var task15 = TaskExamples.SyncingOutput(web3);
            task15.Wait();
            var so = task15.Result;
            Console.WriteLine("SyncingOutput: done");

            Console.WriteLine("Press enter to exit...");
            Console.ReadLine();
        }
    }
}
