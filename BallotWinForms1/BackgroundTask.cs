using Nethereum.Web3;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BallotWinForms1
{
    public class BackgroundTask
    {
        static string[] accounts = null;

        ulong _LastBlockNumber = 0;
        int _RefreshDelay = 2;
        bool _Paused = false;
        string _TxHash = "";

        public delegate void BTLastBlockNumberEventHandler(BackgroundTask bt, EventArgs e);
        public event BTLastBlockNumberEventHandler BTLastBlockNumberEventHandlerPtr = null;

        public delegate void BTAccountsEventHandler(BackgroundTask bt, EventArgs e);
        public event BTAccountsEventHandler BTAccountsEventHandlerPtr = null;

        public delegate void BTAccountsBalancesEventHandler(BackgroundTask bt, EventArgs e);
        public event BTAccountsBalancesEventHandler BTAccountsBalancesEventHandlerPtr = null;

        public delegate void BTTxReceiptEventHandler(BackgroundTask bt, EventArgs e);
        public event BTTxReceiptEventHandler BTTxReceiptEventHandlerPtr = null;

        public delegate void BTSyncingOutputEventHandler(BackgroundTask bt, EventArgs e);
        public event BTSyncingOutputEventHandler BTSyncingOutputEventHandlerPtr = null;


        public BackgroundTask(int delay)
        {
            _RefreshDelay = delay;
        }

        public void Paused(bool flag)
        {
            _Paused = flag;
        }

        public void SetRefreshDelay(int delay)
        {
            _RefreshDelay = delay;
        }

        public void SetLastTxHash(string txHash)
        {
            _TxHash = txHash;
        }

        public async Task Start()
        {
            //Web3 web3 = new Web3();
            Web3 web3 = new Web3("http://eth1ehgy7hyj.eastus.cloudapp.azure.com:8545");
            while (true)
            {
                string protocolVersion = "";
                try
                {
                    Debug.WriteLine("web3.Eth.ProtocolVersion");
                    protocolVersion = await web3.Eth.ProtocolVersion.SendRequestAsync();
                }
                catch(Exception ex)
                {
                    Debug.WriteLine("web3.Eth.ProtocolVersion: " + ex.ToString());
                    protocolVersion = "";
                }

                if (!_Paused && protocolVersion != "")
                {
                    Nethereum.RPC.Eth.DTOs.SyncingOutput so = null;
                    if (BTSyncingOutputEventHandlerPtr != null)
                    {
                        try
                        {
                            Debug.WriteLine("web3.Eth.Syncing");
                            so = await web3.Eth.Syncing.SendRequestAsync();
                        }
                        catch(Exception ex)
                        {
                            Debug.WriteLine("web3.Eth.Syncing: " + ex.ToString());
                        }
                        if (so != null)
                        {
                            EventArgs e = new BTSyncingOutputEventArgs(so);
                            BTSyncingOutputEventHandlerPtr(this, e);
                        }
                        else
                        {
                            Debug.WriteLine("web3.Eth.Syncing: " + "so == null");
                        }
                    }

                    if (BTAccountsBalancesEventHandlerPtr != null)
                    {
                        if (accounts == null)
                        {
                            try
                            {
                                Debug.WriteLine("web3.Personal.ListAccounts.2");
                                accounts = await web3.Personal.ListAccounts.SendRequestAsync();
                            }
                            catch (Exception ex)
                            {
                                accounts = null;     
                                Debug.WriteLine("web3.Personal.ListAccounts.2: " + ex.ToString());
                            }
                        }
                        var listAccounts = new List<string>();
                        var listBalances = new List<decimal>();
                        var listAccountsBalances = new List<string>();
                        if (accounts != null)
                        {
                            Nethereum.Hex.HexTypes.HexBigInteger balanceWei = null;
                            foreach (var account in accounts)
                            {
                                listAccounts.Add(account);

                                try
                                {
                                    Debug.WriteLine("web3.Eth.GetBalance");
                                    balanceWei = await web3.Eth.GetBalance.SendRequestAsync(account);
                                }
                                catch (Exception ex)
                                {
                                    balanceWei = new Nethereum.Hex.HexTypes.HexBigInteger(-1);
                                    Debug.WriteLine("web3.Eth.GetBalance: " + ex.ToString());
                                }
                                var balanceEther = Web3.Convert.FromWei(balanceWei.Value);
                                listBalances.Add(balanceEther);

                                string accountBalance = account + " (" + balanceEther.ToString() + ")";
                                listAccountsBalances.Add(accountBalance);
                            }
                        }
                        EventArgs e = new BTListStringsDecimalsEventArgs(listAccounts, listBalances, listAccountsBalances);
                        BTAccountsBalancesEventHandlerPtr(this, e);
                    }

                    if (so != null && !so.IsSyncing)
                    {
                        if (BTLastBlockNumberEventHandlerPtr != null)
                        {
                            Debug.WriteLine("web3.Eth.Blocks.GetBlockNumber");
                            var maxBlockNumber = await web3.Eth.Blocks.GetBlockNumber.SendRequestAsync();
                            _LastBlockNumber = (ulong)maxBlockNumber.Value;
                            EventArgs e = new BTULongEventArgs(_LastBlockNumber);
                            BTLastBlockNumberEventHandlerPtr(this, e);
                        }

                        if (BTAccountsEventHandlerPtr != null)
                        {
                            Debug.WriteLine("web3.Personal.ListAccounts");
                            accounts = await web3.Personal.ListAccounts.SendRequestAsync();
                            var listAccounts = new List<string>();
                            foreach (var account in accounts)
                            {
                                listAccounts.Add(account);
                            }
                            EventArgs e = new BTListStringsEventArgs(listAccounts);
                            BTAccountsEventHandlerPtr(this, e);
                        }

                        if (BTTxReceiptEventHandlerPtr != null)
                        {
                            if (!String.IsNullOrEmpty(_TxHash))
                            {
                                ulong receipt = 0;
                                Debug.WriteLine("web3.Transactions.GetTransactionReceipt");
                                var txReceipt = await web3.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(_TxHash);
                                if (txReceipt == null)
                                {
                                    receipt = 9;
                                }
                                else
                                {
                                    receipt = (ulong)txReceipt.Status.Value;
                                }
                                EventArgs e = new BTULongEventArgs(receipt);
                                BTTxReceiptEventHandlerPtr(this, e);
                            }
                        }
                    }
                }
                else
                {
                    _LastBlockNumber = 0;
                    if (BTLastBlockNumberEventHandlerPtr != null)
                    {
                        EventArgs e = new BTULongEventArgs(_LastBlockNumber);
                        BTLastBlockNumberEventHandlerPtr(this, e);
                    }
                }

                Debug.WriteLine("Task.Delay: " + DateTime.UtcNow.ToString());
                await Task.Delay(_RefreshDelay * 1000);
            }
        }
    }

    public class BTULongEventArgs : EventArgs
    {
        public BTULongEventArgs(ulong _value)
        {
            Timestamp = DateTime.UtcNow;
            Value = _value;
        }
        public DateTime Timestamp { get; set; }
        public ulong Value { get; set; }
    }

    public class BTListStringsEventArgs : EventArgs
    {
        public BTListStringsEventArgs(List<string> _strings)
        {
            Timestamp = DateTime.UtcNow;
            Strings = _strings;
        }
        public DateTime Timestamp { get; set; }
        public List<string> Strings { get; set; }
    }

    public class BTListUlongsEventArgs : EventArgs
    {
        public BTListUlongsEventArgs(List<ulong> _values)
        {
            Timestamp = DateTime.UtcNow;
            Values = _values;
        }
        public DateTime Timestamp { get; set; }
        public List<ulong> Values { get; set; }
    }

    public class BTListStringsDecimalsEventArgs : EventArgs
    {
        public BTListStringsDecimalsEventArgs(List<string> _strings, List<decimal> _values, List<string> _stringsvalues)
        {
            Timestamp = DateTime.UtcNow;
            Values = _values;
            Strings = _strings;
            StringsValues = _stringsvalues;
        }
        public DateTime Timestamp { get; set; }
        public List<string> Strings { get; set; }
        public List<decimal> Values { get; set; }
        public List<string> StringsValues { get; set; }
    }

    public class BTSyncingOutputEventArgs : EventArgs
    {
        public BTSyncingOutputEventArgs(Nethereum.RPC.Eth.DTOs.SyncingOutput _so)
        {
            Timestamp = DateTime.UtcNow;
            SO = _so;
        }
        public DateTime Timestamp { get; set; }
        public Nethereum.RPC.Eth.DTOs.SyncingOutput SO { get; set; }
    }
}
