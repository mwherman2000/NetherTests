using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BallotWinForms1
{
    public class Listeners
    {
        public class BTLastBlockNumbersListener
        {
            public void Subscribe(BackgroundTask bt, int lId)
            {
                bt.BTLastBlockNumberEventHandlerPtr += new BackgroundTask.BTLastBlockNumberEventHandler(BTLastBlockHandlerHandler);
            }

            private void BTLastBlockHandlerHandler(BackgroundTask bt, EventArgs e)
            {
                BTULongEventArgs btargs = (BTULongEventArgs)e;
                Program.mf.txtMessage.Text = btargs.Timestamp.ToString();
                Program.mf.txtLastBlockNumber.Text = btargs.Value.ToString();

                string msg = "Check if geth.exe is running and synced";
                if (Program.mf.chkPaused.Checked) Program.mf.txtStartedMessage.Text = "Uncheck Paused checkbox";
                else if (btargs.Value == 0) Program.mf.txtStartedMessage.Text = msg;
                else if (Program.mf.txtStartedMessage.Text == msg)
                    Program.mf.txtStartedMessage.Text = "";
            }
        }

        public class BTAccountsListener
        {
            public void Subscribe(BackgroundTask bt, int lId)
            {
                bt.BTAccountsEventHandlerPtr += new BackgroundTask.BTAccountsEventHandler(BTAccountsHandler);
            }

            private void BTAccountsHandler(BackgroundTask bt, EventArgs e)
            {
                BTListStringsEventArgs btargs = (BTListStringsEventArgs)e;
                Program.mf.txtMessage.Text = btargs.Timestamp.ToString();
                Program.mf.listAccounts.DataSource = btargs.Strings;

                int max = btargs.Strings.Count - 1;
                if (max < 0) max = 0;
                Program.mf.numFromAddress.Maximum = max;
                Program.mf.numToAddress.Maximum = max;
                Program.mf.numVoteFromAddress.Maximum = max;

                if (String.IsNullOrEmpty(Program.mf.txtFromAddress.Text))
                {
                    Program.mf.numToAddress.Value = 0;
                    Program.mf.txtFromAddress.Text = Program.mf.listAccounts.Items[(int)Program.mf.numToAddress.Value].ToString();
                }

                if (max >= 1)
                {
                    if (String.IsNullOrEmpty(Program.mf.txtToAddress.Text))
                    {
                        Program.mf.numToAddress.Value = 1;
                        Program.mf.txtToAddress.Text = Program.mf.listAccounts.Items[(int)Program.mf.numToAddress.Value].ToString();
                    }
                    if (String.IsNullOrEmpty(Program.mf.txtVoteFromAddress.Text))
                    {
                        Program.mf.numVoteFromAddress.Value = 1;
                        Program.mf.txtVoteFromAddress.Text = Program.mf.listAccounts.Items[(int)Program.mf.numVoteFromAddress.Value].ToString();
                    }
                }
            }
        }

        public class BTAccountsBalancesListener
        {
            public void Subscribe(BackgroundTask bt, int lId)
            {
                bt.BTAccountsBalancesEventHandlerPtr += new BackgroundTask.BTAccountsBalancesEventHandler(BTAccountsBalancesHandler);
            }

            private void BTAccountsBalancesHandler(BackgroundTask bt, EventArgs e)
            {
                BTListStringsDecimalsEventArgs btargs = (BTListStringsDecimalsEventArgs)e;
                Program.mf.txtMessage.Text = btargs.Timestamp.ToString();
                Program.mf.listBalances.DataSource = btargs.StringsValues;
            }
        }

        public class BTTxReceiptListener
        {
            public void Subscribe(BackgroundTask bt, int lId)
            {
                bt.BTTxReceiptEventHandlerPtr += new BackgroundTask.BTTxReceiptEventHandler(BTTxReceiptHandler);
            }

            private void BTTxReceiptHandler(BackgroundTask bt, EventArgs e)
            {
                BTULongEventArgs btargs = (BTULongEventArgs)e;
                Program.mf.txtMessage.Text = btargs.Timestamp.ToString();
                string value = btargs.Value.ToString();
                if (value == "9") value = "(pending)";
                TimeSpan ts = new TimeSpan(0);
                double tsSeconds = 0;
                try
                {
                    ts = DateTime.Parse(Program.mf.txtMessage.Text) - DateTime.Parse(Program.mf.txtStartedMessage.Text);
                    tsSeconds = ts.TotalSeconds;
                }
                catch (Exception ex)
                {
                    tsSeconds = 0;
                }
 
                if ((value == "0" || value == "1") && Program.mf.txtTxStatus.Text == "(pending)")
                    value += " (" + ts.Seconds.ToString() + " seconds)";
                Program.mf.txtTxStatus.Text = value;
            }
        }

        public class BTSyncingOutputListener
        {
            public void Subscribe(BackgroundTask bt, int lId)
            {
                bt.BTSyncingOutputEventHandlerPtr += new BackgroundTask.BTSyncingOutputEventHandler(BTSyncingOutputEventHandler);
            }

            private void BTSyncingOutputEventHandler(BackgroundTask bt, EventArgs e)
            {
                BTSyncingOutputEventArgs btargs = (BTSyncingOutputEventArgs)e;
                if (btargs != null)
                {
                    Program.mf.txtMessage.Text = btargs.Timestamp.ToString();
                    if (btargs.SO.IsSyncing)
                    {
                        Program.mf.lblSyncing.Text = "Syncing";
                        ulong delta = (ulong)btargs.SO.HighestBlock.Value - (ulong)btargs.SO.CurrentBlock.Value;
                        Program.mf.txtCurrentBlock.Text = btargs.SO.CurrentBlock.Value.ToString() + " (" + delta.ToString() + ")";
                        Program.mf.txtStartingBlock.Text = btargs.SO.StartingBlock.Value.ToString();
                        Program.mf.txtHighestBlock.Text = btargs.SO.HighestBlock.Value.ToString();
                    }
                    else
                    {
                        Program.mf.lblSyncing.Text = "Not syncing";
                    }
                }
                else
                {
                    Program.mf.lblSyncing.Text = "SO null";
                }
            }
        }
    }
}
