using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Nethereum.Hex.HexTypes;
using Nethereum.Web3;

namespace MWH.MyNethereum
{
    static public class TaskExamples
    {
        const int UNLOCK_TIMEOUT = 2 * 60; // 2 minutes (arbitrary)
        const int SLEEP_TIME = 5 * 1000; // 5 seconds (arbitrary)
        const int MAX_TIMEOUT = 4 * 60 * 1000; // 2 minutes (arbirtrary)

        static public async Task<string> GetProtocolVersionExample(Web3 web3)
        {
            Console.WriteLine("GetProtocolVersionExample:");

            var protocolVersion = await web3.Eth.ProtocolVersion.SendRequestAsync();
            Console.WriteLine("protocolVersion:\t" + protocolVersion.ToString());
            return protocolVersion;
        }

        static public async Task<string> GetCoinbaseExample(Web3 web3)
        {
            Console.WriteLine("GetCoinbaseExample:");

            var coinbase = await web3.Eth.CoinBase.SendRequestAsync();
            Console.WriteLine("coinbase:\t" + coinbase.ToString());
            return coinbase;
        }

        static public async Task<HexBigInteger> GetMaxBlockExample(Web3 web3)
        {
            HexBigInteger maxBlockNumber = new HexBigInteger(0);
            Console.WriteLine("GetMaxBlockExample:");

            try
            {
                maxBlockNumber = await web3.Eth.Blocks.GetBlockNumber.SendRequestAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("GetMaxBlockExample:\t" + ex.ToString());
                if (ex.InnerException != null) Console.WriteLine("GetMaxBlockExample:\t" + ex.InnerException.ToString());
            }
            Console.WriteLine("maxBlockNumber:\t" + maxBlockNumber.Value.ToString());
            return maxBlockNumber;
        }

        static public async Task<bool> ScanBlocksExample(Web3 web3, ulong startBlockNumber, ulong endBlockNumber)
        {
            Console.WriteLine("ScanBlocksExample:");

            long txTotalCount = 0;
            for (ulong blockNumber = startBlockNumber; blockNumber <= endBlockNumber; blockNumber++)
            {
                var blockParameter = new Nethereum.RPC.Eth.DTOs.BlockParameter(blockNumber);
                var block = await web3.Eth.Blocks.GetBlockWithTransactionsByNumber.SendRequestAsync(blockParameter);
                ulong size = (ulong)block.Size.Value;
                var trans = block.Transactions;
                int txCount = trans.Length;
                txTotalCount += txCount;
                //if (blockNumber % 1000 == 0) Console.Write(".");
                //if (blockNumber % 10000 == 0)
                {
                    DateTime blockDateTime = Helpers.UnixTimeStampToDateTime((double)block.Timestamp.Value);
                    Console.WriteLine(blockNumber.ToString() + " " + size.ToString() + " " 
                        + txTotalCount.ToString() + " " + blockDateTime.ToString()
                        + " " + (endBlockNumber - blockNumber).ToString());
                }
            }
            Console.WriteLine();
            return true;
        }

        static public async Task<long> ScanTxExample(Web3 web3, ulong startBlockNumber, ulong endBlockNumber)
        {
            Console.WriteLine("ScanTxExample:");

            long txTotalCount = 0;
            for (ulong blockNumber = startBlockNumber; blockNumber <= endBlockNumber; blockNumber++)
            {
                var blockParameter = new Nethereum.RPC.Eth.DTOs.BlockParameter(blockNumber);
                var block = await web3.Eth.Blocks.GetBlockWithTransactionsByNumber.SendRequestAsync(blockParameter);
                var trans = block.Transactions;
                int txCount = trans.Length;
                txTotalCount += txCount;
                //Console.WriteLine("block:\t" + blockNumber.ToString() + " " + txCount.ToString());
                if (txCount > 0)
                {
                    foreach (var tx in trans)
                    {
                        try
                        {
                            var bn = tx.BlockNumber.Value;
                            var th = tx.TransactionHash;
                            var ti = tx.TransactionIndex.Value;

                            var rpt = await web3.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(th);
                            BigInteger status = 0;
                            if (rpt.Status != null) status = rpt.Status.Value;

                            var nc = tx.Nonce.Value;
                            var from = tx.From;
                            Task taskfr = GetAccountBalanceExample(web3, from);
                            taskfr.Wait();

                            Console.WriteLine(th.ToString() + " bn " + bn.ToString() + " ti " + ti.ToString() + " fr " + from.ToString() + " st " + status.ToString());

                            var to = tx.To;
                            if (to == null) to = "to:NULL";
                            else
                            {
                                Task taskto = GetAccountBalanceExample(web3, to);
                                taskto.Wait();
                            }
                            var v = tx.Value.Value;
                            var g = tx.Gas.Value;
                            var gp = tx.GasPrice.Value;

                            Console.WriteLine(th.ToString() + " bn " + bn.ToString() + " ti " + ti.ToString() + " nc " + nc.ToString() + " fr " + from.ToString() + " to " + to.ToString() + " v " + v.ToString() + " g " + g.ToString() + " gp " + gp.ToString());
                            var dataHex = tx.Input;
                            if (dataHex != null)
                            {
                                var data = Helpers.ConvertHexToASCII(dataHex);
                                Console.WriteLine("DataHex:\t" + dataHex.ToString() + "\nData:\t" + data.ToString());
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("ScanTxExample.Tx:\t" + ex.ToString());
                            if (ex.InnerException != null) Console.WriteLine("ScanTxExample.Tx:\t" + ex.InnerException.ToString());
                        }
                    }
                    Console.WriteLine();
                }
            }
            return txTotalCount;
        }

        static public async Task<HexBigInteger> GetAccountBalanceExample(Web3 web3, string accountAddress)
        {
            Console.WriteLine("GetAccountBalanceExample:");

            var balanceWei = await web3.Eth.GetBalance.SendRequestAsync(accountAddress);
            var balanceEther = Web3.Convert.FromWei(balanceWei.Value);
            Console.WriteLine("accountAddress:\t" + accountAddress.ToString());
            Console.WriteLine("balanceEther:\t" + balanceEther.ToString());
            Console.WriteLine("balanceWei:\t" + balanceWei.Value.ToString());
            return balanceWei;
        }

        static public async Task<long> ListPersonalAccountsExample(Web3 web3)
        {
            Console.WriteLine("ListPersonalAccountsExample:");

            var accounts = await web3.Personal.ListAccounts.SendRequestAsync();
            foreach (var account in accounts)
            {
                var balanceWei = await web3.Eth.GetBalance.SendRequestAsync(account);
                var balanceEther = Web3.Convert.FromWei(balanceWei.Value);
                Console.WriteLine("account:\t" + account + " balanceEther:\t" + balanceEther.ToString());
            }
            return accounts.Length;
        }

        static public async Task<string> SendEtherToAccountExample(Web3 web3, string fromAddress, string fromPassword, string toAddress, long amountWei)
        {
            Console.WriteLine("SendEtherToAccountExample:");

            var unlockResult = await web3.Personal.UnlockAccount.SendRequestAsync(fromAddress, fromPassword, UNLOCK_TIMEOUT);
            var sendTxHash = await web3.Eth.TransactionManager.SendTransactionAsync(fromAddress, toAddress, new HexBigInteger(amountWei));
            Console.WriteLine("fromAddress:\t" + fromAddress.ToString());
            Console.WriteLine("toAddress:\t" + toAddress.ToString());
            Console.WriteLine("amountWei:\t" + amountWei.ToString());
            Console.WriteLine("sendTxHash:\t" + sendTxHash.ToString());
            return sendTxHash;
        }

        static public async Task<Nethereum.RPC.Eth.DTOs.TransactionReceipt> WaitForTxReceiptExample(Web3 web3, string txHash)
        {
            Console.WriteLine("WaitForTxReceiptExample:");
            Console.WriteLine("txHash:\t" + txHash.ToString());

            int timeoutCount = 0;
            var txReceipt = await web3.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(txHash);
            while (txReceipt == null && timeoutCount < MAX_TIMEOUT)
            {
                Console.WriteLine("Sleeping...");
                Thread.Sleep(SLEEP_TIME);
                txReceipt = await web3.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(txHash);
                timeoutCount += SLEEP_TIME;
            }
            Console.WriteLine("timeoutCount " + timeoutCount.ToString());
            if (txReceipt != null) Console.WriteLine("txReceipt.Status:\t" + txReceipt.Status.Value.ToString());
            return txReceipt;
        }

        static public async Task<bool> InteractWithExistingContractExample(Web3 web3, string fromAddress, string fromPassword, string contractAddress, string contractAbi)
        {
            Console.WriteLine("InteractWithExistingContractExample:");

            var contract = web3.Eth.GetContract(contractAbi, contractAddress);

            var setMessageFunction = contract.GetFunction("setMsg");
            var getMessageFunction = contract.GetFunction("getMsg");

            string nowTimestamp = DateTime.UtcNow.ToString() + " UTC";
            Console.WriteLine("now:\t" + nowTimestamp);

            var unlockResult = await web3.Personal.UnlockAccount.SendRequestAsync(fromAddress, fromPassword, UNLOCK_TIMEOUT);
            var txHash1 = await setMessageFunction.SendTransactionAsync(fromAddress, new HexBigInteger(900000), null, 1, "Hello World");
            Console.WriteLine("txHash1:\t" + txHash1.ToString());
            var txHash2 = await setMessageFunction.SendTransactionAsync(fromAddress, new HexBigInteger(900000), null, 2, nowTimestamp);
            Console.WriteLine("txHash2:\t" + txHash2.ToString());

            var txReceipt2 = await web3.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(txHash2);
            int timeoutCount = 0;
            while (txReceipt2 == null && timeoutCount < MAX_TIMEOUT)
            {
                Console.WriteLine("Sleeping...");
                Thread.Sleep(SLEEP_TIME);
                txReceipt2 = await web3.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(txHash2);
                timeoutCount += SLEEP_TIME;
            }
            Console.WriteLine("timeoutCount:\t" + timeoutCount.ToString());

            var txReceipt3 = await setMessageFunction.SendTransactionAndWaitForReceiptAsync(fromAddress, new HexBigInteger(900000), null, null, 2, nowTimestamp + " Wait");
            Console.WriteLine("txReceipt3:\t" + txReceipt3.TransactionHash.ToString());
            Console.WriteLine("txReceipt3:\t" + txReceipt3.CumulativeGasUsed.Value.ToString());

            var getResult1 = await getMessageFunction.CallAsync<string>(1);
            Console.WriteLine("getResult1:\t" + getResult1.ToString());
            var getResult2 = await getMessageFunction.CallAsync<string>(2);
            Console.WriteLine("getResult2:\t" + getResult2.ToString());
            return true;
        }

        static public async Task<bool> InteractWithExistingContractWithEventsExample(Web3 web3, string fromAddress, string fromPassword, string contractAddress, string contractAbi)
        {
            Console.WriteLine("InteractWithExistingContractWithEventsExample:");

            var contract = web3.Eth.GetContract(contractAbi, contractAddress);

            var setMessageFunction = contract.GetFunction("setMsg");
            var getMessageFunction = contract.GetFunction("getMsg");
            var multipliedEvent = contract.GetEvent("MultipliedEvent");
            var newMessageEvent = contract.GetEvent("NewMessageEvent");

            var filterAllMultipliedEvent = await multipliedEvent.CreateFilterAsync();
            var filterAllNewMessageEvent = await newMessageEvent.CreateFilterAsync();

            string nowTimestamp = DateTime.UtcNow.ToString() + " UTC";
            Console.WriteLine("now:\t" + nowTimestamp);

            var unlockResult = await web3.Personal.UnlockAccount.SendRequestAsync(fromAddress, fromPassword, UNLOCK_TIMEOUT);
            var txHash1 = await setMessageFunction.SendTransactionAsync(fromAddress, new HexBigInteger(900000), null, 1, "Hello World");
            Console.WriteLine("txHash1:\t" + txHash1.ToString());
            var txHash2 = await setMessageFunction.SendTransactionAsync(fromAddress, new HexBigInteger(900000), null, 2, nowTimestamp);
            Console.WriteLine("txHash2:\t" + txHash2.ToString());

            var txReceipt2 = await web3.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(txHash2);
            int timeoutCount = 0;
            while (txReceipt2 == null && timeoutCount < MAX_TIMEOUT)
            {
                Console.WriteLine("Sleeping...");
                Thread.Sleep(SLEEP_TIME);
                txReceipt2 = await web3.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(txHash2);
                timeoutCount += SLEEP_TIME;
            }
            Console.WriteLine("timeoutCount:\t" + timeoutCount.ToString());

            var txReceipt3 = await setMessageFunction.SendTransactionAndWaitForReceiptAsync(fromAddress, new HexBigInteger(900000), null, null, 2, nowTimestamp + " Wait");
            Console.WriteLine("txReceipt3:\t" + txReceipt3.TransactionHash.ToString());
            Console.WriteLine("txReceipt3:\t" + txReceipt3.CumulativeGasUsed.Value.ToString());

            var getResult1 = await getMessageFunction.CallAsync<string>(1);
            Console.WriteLine("getResult1:\t" + getResult1.ToString());
            var getResult2 = await getMessageFunction.CallAsync<string>(2);
            Console.WriteLine("getResult2:\t" + getResult2.ToString());

            var logMultipliedEvents = await multipliedEvent.GetFilterChanges<FunctionOutputHelpers.MultipliedEventArgs>(filterAllMultipliedEvent);
            foreach (var mea in logMultipliedEvents)
            {
                Console.WriteLine("multipliedEvent:\t" +
                mea.Event.sender + " " + mea.Event.oldProduct.ToString() + " " + mea.Event.value.ToString() + " " + mea.Event.newProduct.ToString());
            }

            var logNewMessageEvents = await newMessageEvent.GetFilterChanges<FunctionOutputHelpers.NewMessageEventArgs>(filterAllNewMessageEvent);
            foreach (var mea in logNewMessageEvents)
            {
                Console.WriteLine("newMessageEvent:\t" +
                mea.Event.sender + " " + mea.Event.ind.ToString() + " " + mea.Event.msg.ToString());
            }
            return true;
        }

        static public async Task<long> GetAllChangesExample(Web3 web3, string fromAddress, string fromPassword, string contractAddress, string contractAbi)
        {
            Console.WriteLine("GetAllChangesExample:");

            var contract = web3.Eth.GetContract(contractAbi, contractAddress);
            var evt = contract.GetEvent("DepositReceipt");
            var blockParameter = new Nethereum.RPC.Eth.DTOs.BlockParameter(0);
            var evtFilter = await evt.CreateFilterAsync(blockParameter);
            var evtLog = await evt.GetAllChanges<FunctionOutputHelpers.DepositReceiptArgs>(evtFilter);
            foreach (var mea in evtLog)
            {
                var dt = Helpers.UnixTimeStampToDateTime((double)mea.Event.timestamp);
                Console.WriteLine("GetAllChangesExample:\t" +
                dt.ToString() + " " + mea.Event.from.ToString() + " " + mea.Event.id.ToString() + " " + mea.Event.value.ToString());

            }
            return evtLog.Count;
        }

        static public async Task<bool> GetContractValuesHistoryUniqueOffsetValueExample(Web3 web3, string contractAddress, HexBigInteger recentBlockNumber, ulong numberBlocks, int offset)
        {
            Console.WriteLine("GetContractValuesHistoryUniqueOffsetValueExample:");

            string previousValue = "";
            for (ulong blockNumber = (ulong)recentBlockNumber.Value; blockNumber > (ulong)recentBlockNumber.Value - numberBlocks; blockNumber--)
            {
                var blockNumberParameter = new Nethereum.RPC.Eth.DTOs.BlockParameter(blockNumber);
                var valueAtOffset = await web3.Eth.GetStorageAt.SendRequestAsync(contractAddress, new HexBigInteger(offset), blockNumberParameter);
                if (valueAtOffset != previousValue)
                {
                    var block = await web3.Eth.Blocks.GetBlockWithTransactionsByNumber.SendRequestAsync(blockNumberParameter);
                    DateTime blockDateTime = Helpers.UnixTimeStampToDateTime((double)block.Timestamp.Value);
                    Console.WriteLine("blockDateTime:\t" + blockDateTime.ToString());

                    for (int storageOffset = 0; storageOffset < offset + 2; storageOffset++)
                    {
                        var valueAt = await web3.Eth.GetStorageAt.SendRequestAsync(contractAddress, new HexBigInteger(storageOffset), blockNumberParameter);
                        Console.WriteLine("value:\t" + blockNumber.ToString() + " " + storageOffset.ToString() + " " + valueAt + " " + Helpers.ConvertHexToASCII(valueAt.Substring(2)));
                    }
                    previousValue = valueAtOffset;
                }
            }
            return true;
        }

        static public async Task<string> SendEtherToAccountWithExtraDataExample(Web3 web3, string fromAddress, string fromPassword, string toAddress, long amountWei, string extraData)
        {
            Console.WriteLine("SendEtherToAccountWithExtraDataExample:");

            var extraDataHex = new HexUTF8String(extraData).HexValue;
            var ti = new Nethereum.RPC.Eth.DTOs.TransactionInput(extraDataHex, toAddress, fromAddress, new HexBigInteger(900000), new HexBigInteger(amountWei));

            var unlockResult = await web3.Personal.UnlockAccount.SendRequestAsync(fromAddress, fromPassword, UNLOCK_TIMEOUT);
            var sendTxHash = await web3.Eth.TransactionManager.SendTransactionAsync(ti);
            Console.WriteLine("fromAddress:\t" + fromAddress.ToString());
            Console.WriteLine("toAddress:\t" + toAddress.ToString());
            Console.WriteLine("amountWei:\t" + amountWei.ToString());
            Console.WriteLine("extraData:\t" + extraData.ToString());
            Console.WriteLine("extraDataHex:\t" + extraDataHex.ToString());
            Console.WriteLine("sendTxHash:\t" + sendTxHash.ToString());
            return sendTxHash;
        }

        static public async Task<bool> InteractWithExistingContractWithEtherAndEventsExample(Web3 web3, string fromAddress, string fromPassword, string contractAddress, string contractAbi, long amountWei)
        {
            Console.WriteLine("InteractWithExistingContractWithEtherAndEventsExample:");

            string extraData = DateTime.Now.ToString() + " extraData contract";

            var extraDataHex = new HexUTF8String(extraData).HexValue;
            var ti = new Nethereum.RPC.Eth.DTOs.TransactionInput(extraDataHex, contractAddress, fromAddress, new HexBigInteger(900000), new HexBigInteger(amountWei));

            var contract = web3.Eth.GetContract(contractAbi, contractAddress);

            var depositFunction = contract.GetFunction("deposit");
            var depositReceiptEvent = contract.GetEvent("DepositReceipt");

            var filterAllDepositReceipt = await depositReceiptEvent.CreateFilterAsync();

            var unlockResult = await web3.Personal.UnlockAccount.SendRequestAsync(fromAddress, fromPassword, UNLOCK_TIMEOUT);
            var txHash2 = await depositFunction.SendTransactionAsync(ti, "1234-5678-9012-3456"); // first transfer
            ti.Value = new HexBigInteger(357);
            txHash2 = await depositFunction.SendTransactionAsync(ti, "1234-5678-9012-3456"); // second transfer
            Console.WriteLine("txHash2:\t" + txHash2.ToString());

            var txReceipt2 = await web3.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(txHash2);
            int timeoutCount = 0;
            while (txReceipt2 == null && timeoutCount < MAX_TIMEOUT)
            {
                Console.WriteLine("Sleeping...");
                Thread.Sleep(SLEEP_TIME);
                txReceipt2 = await web3.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(txHash2);
                timeoutCount += SLEEP_TIME;
            }
            Console.WriteLine("timeoutCount:\t" + timeoutCount.ToString());
            Console.WriteLine("fromAddress:\t" + fromAddress.ToString());
            Console.WriteLine("contractAddress:\t" + contractAddress.ToString());
            Console.WriteLine("amountWei:\t" + amountWei.ToString());
            Console.WriteLine("extraData:\t" + extraData.ToString());
            Console.WriteLine("extraDataHex:\t" + extraDataHex.ToString());
            Console.WriteLine("txHash2:\t" + txHash2.ToString());

            var logDepositReceipts = await depositReceiptEvent.GetFilterChanges<FunctionOutputHelpers.DepositReceiptArgs>(filterAllDepositReceipt);
            foreach (var dra in logDepositReceipts)
            {
                var dt = Helpers.UnixTimeStampToDateTime((double)dra.Event.timestamp);
                Console.WriteLine("depositReceiptEvent:\t" +
                dt.ToString() + " " + dra.Event.from.ToString() + " " + dra.Event.id.ToString() + " " + dra.Event.value.ToString());
            }

            return true;
        }

        static public async Task<bool> MonitorDepositEventsExample(Web3 web3, string contractAddress, string contractAbi, CancellationToken ct)
        {
            Console.WriteLine("MonitorDepositEventsExample:");

            var contract = web3.Eth.GetContract(contractAbi, contractAddress);

            var depositReceiptEvent = contract.GetEvent("DepositReceipt");
            var blockParameter = new Nethereum.RPC.Eth.DTOs.BlockParameter(0);
            var filterAllDepositReceipt = await depositReceiptEvent.CreateFilterAsync(blockParameter);

            Console.WriteLine("contractAddress:\t" + contractAddress.ToString());

            for (int iter = 0; iter < 100; iter++)
            {
                Console.Write("m");

                if (ct.IsCancellationRequested == true)
                {
                    Console.WriteLine("Task cancelled");
                    ct.ThrowIfCancellationRequested();
                }

                var logDepositReceipts = await depositReceiptEvent.GetFilterChanges<FunctionOutputHelpers.DepositReceiptArgs>(filterAllDepositReceipt);
                foreach (var dra in logDepositReceipts)
                {
                    var dt = Helpers.UnixTimeStampToDateTime((double)dra.Event.timestamp);
                    Console.WriteLine("DepositReceiptEvent.monitor:\t" +
                    dt.ToString() + " " + dra.Event.from.ToString() + " " + dra.Event.id.ToString() + " " + dra.Event.value.ToString());
                }
                Thread.Sleep(10 * 1000);
            }
            return true;
        }

        static public async Task<bool> IsAddressForAContract(Web3 web3, string address)
        {
            Console.WriteLine("IsAddressForAContract:");

            var code = await web3.Eth.GetCode.SendRequestAsync(address);
            if (code == null || code == "0x")
            {
                Console.WriteLine("GetCode: address {0} is not a Contract (doesn't have code)", address);
                return false;
            }
            else
            {
                Console.WriteLine("GetCode: address {0} is a Contract (has code)", address);
                Console.WriteLine("code.Length:\t" + code.Length.ToString());
                return true;
            }
        }

        static public async Task<Nethereum.RPC.Eth.DTOs.SyncingOutput> SyncingOutput(Web3 web3)
        {
            Console.WriteLine("Syncing:");

            var so = await web3.Eth.Syncing.SendRequestAsync();
            Console.WriteLine("so.IsSyncing:\t" + so.IsSyncing.ToString());
            if (so.IsSyncing)
            {
                Console.WriteLine(" so.CurrentBlock:\t" + so.CurrentBlock.Value.ToString());
                Console.WriteLine(" so.HighestBlock:\t" + so.HighestBlock.Value.ToString());
                Console.WriteLine(" so.StartingBlock:\t" + so.StartingBlock.Value.ToString());
            }
            return so;
        }
    }
}
