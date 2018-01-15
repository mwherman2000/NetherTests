using Nethereum.Hex.HexTypes;
using Nethereum.Web3;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BallotWinForms1
{
    public partial class MainForm : Form
    {
        const int UNLOCK_TIMEOUT = 2 * 60; // 2 minutes (arbitrary)

        BackgroundTask bt = null;
        Task btt = null;
        Listeners.BTLastBlockNumbersListener btListener1 = null;
        Listeners.BTAccountsListener btListener2 = null;
        Listeners.BTAccountsBalancesListener btListener3 = null;
        Listeners.BTTxReceiptListener btListener4 = null;
        Listeners.BTSyncingOutputListener btListener5 = null;

        private Web3 web3 = null;
        private string fromPassword = "";

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            int delay = int.Parse(txtRefreshRate.Text);
            bt = new BackgroundTask(delay);
            btt = bt.Start();

            btListener1 = new Listeners.BTLastBlockNumbersListener();
            btListener1.Subscribe(bt, 1);

            btListener2 = new Listeners.BTAccountsListener();
            btListener2.Subscribe(bt, 1);

            btListener3 = new Listeners.BTAccountsBalancesListener();
            btListener3.Subscribe(bt, 1);

            btListener4 = new Listeners.BTTxReceiptListener();
            btListener4.Subscribe(bt, 1);

            btListener5 = new Listeners.BTSyncingOutputListener();
            btListener5.Subscribe(bt, 1);
        }

        private void chkPaused_CheckedChanged(object sender, EventArgs e)
        {
            bt.Paused(chkPaused.CheckState == CheckState.Checked);
        }

        private void txtRefreshRate_TextChanged(object sender, EventArgs e)
        {
            int delay = 0;
            if (!String.IsNullOrEmpty(txtRefreshRate.Text))
            {
                try
                {
                    delay = int.Parse(txtRefreshRate.Text);
                }
                catch(Exception ex)
                {
                    txtRefreshRate.Text = "2";
                    delay = 2;
                }
                bt.SetRefreshDelay(delay);
            }
        }

        private void numFromAddress_ValueChanged(object sender, EventArgs e)
        {
            int i = (int)numFromAddress.Value;
            txtFromAddress.Text = listAccounts.Items[i].ToString();
        }

        private void numToAddress_ValueChanged(object sender, EventArgs e)
        {
            int i = (int)numToAddress.Value;
            txtToAddress.Text = listAccounts.Items[i].ToString();
        }

        private void numVoteFromAddress_ValueChanged(object sender, EventArgs e)
        {
            int i = (int)numVoteFromAddress.Value;
            txtVoteFromAddress.Text = listAccounts.Items[i].ToString();

            if (String.IsNullOrEmpty(txtDelegateToAddress.Text) && String.IsNullOrEmpty(txtRightToVoteAddress.Text))
            {
                txtDelegateToAddress.Text = txtVoteFromAddress.Text;
                txtRightToVoteAddress.Text = txtVoteFromAddress.Text;
            }
        }

        private async void btnSendEther_Click(object sender, EventArgs e)
        {
            txtTxHash.Text = "";
            txtTxStatus.Text = "";

            if (String.IsNullOrEmpty(fromPassword)) fromPassword = Prompt.ShowDialog("Password", "Enter password...");
            if (!String.IsNullOrEmpty(fromPassword))
            {
                if (web3 == null) web3 = new Web3();
                string fromAddress = txtFromAddress.Text;
                string toAddress = txtToAddress.Text;
                ulong amountWei = ulong.Parse(txtValue.Text);
                txtStartedMessage.Text = DateTime.UtcNow.ToString(); // set this early so that it doesn't overwrite txtStartedMesssage
                string txHash = "";
                try
                {
                    var unlockResult = await web3.Personal.UnlockAccount.SendRequestAsync(fromAddress, fromPassword, UNLOCK_TIMEOUT);
                    string extraDataHex = txtDataHex.Text;
                    if (extraDataHex.Length == 0 || extraDataHex == "0x")
                    {
                        txHash = await web3.Eth.TransactionManager.SendTransactionAsync(fromAddress, toAddress, new HexBigInteger(amountWei));
                    }
                    else
                    {
                        Nethereum.RPC.Eth.DTOs.TransactionInput ti = new Nethereum.RPC.Eth.DTOs.TransactionInput();
                        ti.From = fromAddress;
                        ti.Gas = new HexBigInteger(900000);
                        ti.To = toAddress;
                        ti.Value = new HexBigInteger(amountWei);
                        ti.Data = extraDataHex;
                        txHash = await web3.Eth.TransactionManager.SendTransactionAsync(ti);
                    }
                }
                catch (Exception ex)
                {
                    txtStartedMessage.Text = ex.Message;
                    fromPassword = ""; // doesn't cause password reprompt
                }
                bt.SetLastTxHash(txHash);
                txtTxHash.Text = txHash;
            }
            else
            {
                txtStartedMessage.Text = "No password";
            }
        }

        private async void btnGetWinningProposal_Click(object sender, EventArgs e)
        {
            if (web3 == null) web3 = new Web3();
            var contract = web3.Eth.GetContract(txtContractAbi.Text, txtContractAddress.Text);
            var fWinningProposal = contract.GetFunction("winningProposal");
            var winningProposal = await fWinningProposal.CallAsync<int>();

            txtWinningProposal.Text = winningProposal.ToString();
        }

        private async void btnSendVote_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(fromPassword)) fromPassword = Prompt.ShowDialog("Password", "Enter password...");
            if (!String.IsNullOrEmpty(fromPassword))
            {
                string fromAddress = txtVoteFromAddress.Text;
                int proposal = int.Parse(txtVoteForProposal.Text);

                if (web3 == null) web3 = new Web3();
                var unlockResult = await web3.Personal.UnlockAccount.SendRequestAsync(fromAddress, fromPassword, UNLOCK_TIMEOUT);

                var contract = web3.Eth.GetContract(txtContractAbi.Text, txtContractAddress.Text);
                var fVote = contract.GetFunction("vote");
                Nethereum.RPC.Eth.DTOs.TransactionInput ti = new Nethereum.RPC.Eth.DTOs.TransactionInput();
                ti.From = fromAddress;
                ti.Gas = new HexBigInteger(900000);
                ti.To = contract.Address;
                var txHash = await fVote.SendTransactionAsync(ti, proposal);
                // fromAddress, new HexBigInteger(3000000), null, proposal
                bt.SetLastTxHash(txHash);
                txtTxHash.Text = txHash;
                txtStartedMessage.Text = DateTime.UtcNow.ToString();
            }
            else
            {
                txtStartedMessage.Text = "No password";
            }
        }

        private async void btnRightToVote_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(fromPassword)) fromPassword = Prompt.ShowDialog("Password", "Enter password...");
            if (!String.IsNullOrEmpty(fromPassword))
            {
                string fromAddress = txtVoteFromAddress.Text;
                string rightToVoteAddress = txtRightToVoteAddress.Text;

                if (web3 == null) web3 = new Web3();
                var unlockResult = await web3.Personal.UnlockAccount.SendRequestAsync(fromAddress, fromPassword, UNLOCK_TIMEOUT);

                var contract = web3.Eth.GetContract(txtContractAbi.Text, txtContractAddress.Text);
                var fGiveRightToVote = contract.GetFunction("giveRightToVote");
                var txHash = await fGiveRightToVote.SendTransactionAsync(fromAddress, new HexBigInteger(3000000), null, rightToVoteAddress);
                bt.SetLastTxHash(txHash);
                txtTxHash.Text = txHash;
                txtStartedMessage.Text = DateTime.UtcNow.ToString();
            }
            else
            {
                txtStartedMessage.Text = "No password";
            }
        }

        private async void btnDelegateToAddress_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(fromPassword)) fromPassword = Prompt.ShowDialog("Password", "Enter password...");
            if (!String.IsNullOrEmpty(fromPassword))
            {
                string fromAddress = txtVoteFromAddress.Text;
                string delegateToAddress = txtDelegateToAddress.Text;

                if (web3 == null) web3 = new Web3();
                var unlockResult = await web3.Personal.UnlockAccount.SendRequestAsync(fromAddress, fromPassword, UNLOCK_TIMEOUT);

                var contract = web3.Eth.GetContract(txtContractAbi.Text, txtContractAddress.Text);
                var fDelegate = contract.GetFunction("delegate");
                var txHash = await fDelegate.SendTransactionAsync(fromAddress, new HexBigInteger(3000000), null, delegateToAddress);
                bt.SetLastTxHash(txHash);
                txtTxHash.Text = txHash;
                txtStartedMessage.Text = DateTime.UtcNow.ToString();
            }
            else
            {
                txtStartedMessage.Text = "No password";
            }
        }

        private void btnTxScan_Click(object sender, EventArgs e)
        {
            string url = "";
            if (txtTxStatus.Text.Length == 0) url = "https://rinkeby.etherscan.io/address/" + txtFromAddress.Text;
            else url = "https://rinkeby.etherscan.io/tx/" + txtTxHash.Text;

            System.Diagnostics.Process.Start(url);
        }

        private void txtTxStatus_TextChanged(object sender, EventArgs e)
        {
            if (txtTxStatus.Text == "(pending)") btnTxScan.Enabled = false;
            else btnTxScan.Enabled = true;
        }

        private void txtData_TextChanged(object sender, EventArgs e)
        {
            txtDataHex.Text = new HexUTF8String(txtData.Text).HexValue;
        }
    }
}
