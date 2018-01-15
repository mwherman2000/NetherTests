namespace BallotWinForms1
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtStartingBlock = new System.Windows.Forms.TextBox();
            this.chkPaused = new System.Windows.Forms.CheckBox();
            this.txtCurrentBlock = new System.Windows.Forms.TextBox();
            this.label20 = new System.Windows.Forms.Label();
            this.txtHighestBlock = new System.Windows.Forms.TextBox();
            this.label19 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.lblSyncing = new System.Windows.Forms.Label();
            this.btnTxScan = new System.Windows.Forms.Button();
            this.label13 = new System.Windows.Forms.Label();
            this.txtStartedMessage = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.txtTxStatus = new System.Windows.Forms.TextBox();
            this.txtTxHash = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.listBalances = new System.Windows.Forms.ListBox();
            this.txtMessage = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.listAccounts = new System.Windows.Forms.ListBox();
            this.txtLastBlockNumber = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtRefreshRate = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label16 = new System.Windows.Forms.Label();
            this.txtData = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.txtDataHex = new System.Windows.Forms.TextBox();
            this.btnSendEther = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.txtValue = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtToAddress = new System.Windows.Forms.TextBox();
            this.txtFromAddress = new System.Windows.Forms.TextBox();
            this.numToAddress = new System.Windows.Forms.NumericUpDown();
            this.numFromAddress = new System.Windows.Forms.NumericUpDown();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label14 = new System.Windows.Forms.Label();
            this.txtVoteFromAddress = new System.Windows.Forms.TextBox();
            this.btnDelegateToAddress = new System.Windows.Forms.Button();
            this.numVoteFromAddress = new System.Windows.Forms.NumericUpDown();
            this.btnRightToVote = new System.Windows.Forms.Button();
            this.btnGetWinningProposal = new System.Windows.Forms.Button();
            this.btnSendVote = new System.Windows.Forms.Button();
            this.txtRightToVoteAddress = new System.Windows.Forms.TextBox();
            this.txtDelegateToAddress = new System.Windows.Forms.TextBox();
            this.txtWinningProposal = new System.Windows.Forms.TextBox();
            this.txtVoteForProposal = new System.Windows.Forms.TextBox();
            this.txtContractAbi = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.txtContractAddress = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numToAddress)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numFromAddress)).BeginInit();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numVoteFromAddress)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.SystemColors.ControlLight;
            this.groupBox1.Controls.Add(this.txtStartingBlock);
            this.groupBox1.Controls.Add(this.chkPaused);
            this.groupBox1.Controls.Add(this.txtCurrentBlock);
            this.groupBox1.Controls.Add(this.label20);
            this.groupBox1.Controls.Add(this.txtHighestBlock);
            this.groupBox1.Controls.Add(this.label19);
            this.groupBox1.Controls.Add(this.label18);
            this.groupBox1.Controls.Add(this.lblSyncing);
            this.groupBox1.Controls.Add(this.btnTxScan);
            this.groupBox1.Controls.Add(this.label13);
            this.groupBox1.Controls.Add(this.txtStartedMessage);
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Controls.Add(this.txtTxStatus);
            this.groupBox1.Controls.Add(this.txtTxHash);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.listBalances);
            this.groupBox1.Controls.Add(this.txtMessage);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.listAccounts);
            this.groupBox1.Controls.Add(this.txtLastBlockNumber);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txtRefreshRate);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(6, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(983, 168);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "1. Node State";
            // 
            // txtStartingBlock
            // 
            this.txtStartingBlock.Location = new System.Drawing.Point(278, 52);
            this.txtStartingBlock.Name = "txtStartingBlock";
            this.txtStartingBlock.ReadOnly = true;
            this.txtStartingBlock.Size = new System.Drawing.Size(99, 20);
            this.txtStartingBlock.TabIndex = 19;
            this.txtStartingBlock.Text = "0";
            // 
            // chkPaused
            // 
            this.chkPaused.AutoSize = true;
            this.chkPaused.Location = new System.Drawing.Point(219, 28);
            this.chkPaused.Name = "chkPaused";
            this.chkPaused.Size = new System.Drawing.Size(62, 17);
            this.chkPaused.TabIndex = 6;
            this.chkPaused.Text = "Paused";
            this.chkPaused.UseVisualStyleBackColor = true;
            this.chkPaused.CheckedChanged += new System.EventHandler(this.chkPaused_CheckedChanged);
            // 
            // txtCurrentBlock
            // 
            this.txtCurrentBlock.Location = new System.Drawing.Point(354, 12);
            this.txtCurrentBlock.Name = "txtCurrentBlock";
            this.txtCurrentBlock.ReadOnly = true;
            this.txtCurrentBlock.Size = new System.Drawing.Size(125, 20);
            this.txtCurrentBlock.TabIndex = 20;
            this.txtCurrentBlock.Text = "0";
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(383, 36);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(73, 13);
            this.label20.TabIndex = 25;
            this.label20.Text = "Highest Block";
            // 
            // txtHighestBlock
            // 
            this.txtHighestBlock.Location = new System.Drawing.Point(383, 52);
            this.txtHighestBlock.Name = "txtHighestBlock";
            this.txtHighestBlock.ReadOnly = true;
            this.txtHighestBlock.Size = new System.Drawing.Size(96, 20);
            this.txtHighestBlock.TabIndex = 24;
            this.txtHighestBlock.Text = "0";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(278, 15);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(71, 13);
            this.label19.TabIndex = 23;
            this.label19.Text = "Current Block";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(277, 36);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(73, 13);
            this.label18.TabIndex = 22;
            this.label18.Text = "Starting Block";
            // 
            // lblSyncing
            // 
            this.lblSyncing.AutoSize = true;
            this.lblSyncing.Location = new System.Drawing.Point(217, 55);
            this.lblSyncing.Name = "lblSyncing";
            this.lblSyncing.Size = new System.Drawing.Size(13, 13);
            this.lblSyncing.TabIndex = 21;
            this.lblSyncing.Text = "--";
            // 
            // btnTxScan
            // 
            this.btnTxScan.Location = new System.Drawing.Point(892, 50);
            this.btnTxScan.Name = "btnTxScan";
            this.btnTxScan.Size = new System.Drawing.Size(79, 23);
            this.btnTxScan.TabIndex = 16;
            this.btnTxScan.Text = "Etherscan";
            this.btnTxScan.UseVisualStyleBackColor = true;
            this.btnTxScan.Click += new System.EventHandler(this.btnTxScan_Click);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(496, 138);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(56, 13);
            this.label13.TabIndex = 18;
            this.label13.Text = "Tx Started";
            // 
            // txtStartedMessage
            // 
            this.txtStartedMessage.Location = new System.Drawing.Point(562, 135);
            this.txtStartedMessage.Name = "txtStartedMessage";
            this.txtStartedMessage.ReadOnly = true;
            this.txtStartedMessage.Size = new System.Drawing.Size(409, 20);
            this.txtStartedMessage.TabIndex = 17;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(496, 25);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(47, 13);
            this.label12.TabIndex = 17;
            this.label12.Text = "Tx Hash";
            // 
            // txtTxStatus
            // 
            this.txtTxStatus.Location = new System.Drawing.Point(562, 52);
            this.txtTxStatus.Name = "txtTxStatus";
            this.txtTxStatus.ReadOnly = true;
            this.txtTxStatus.Size = new System.Drawing.Size(315, 20);
            this.txtTxStatus.TabIndex = 12;
            this.txtTxStatus.TextChanged += new System.EventHandler(this.txtTxStatus_TextChanged);
            // 
            // txtTxHash
            // 
            this.txtTxHash.Location = new System.Drawing.Point(562, 22);
            this.txtTxHash.Name = "txtTxHash";
            this.txtTxHash.ReadOnly = true;
            this.txtTxHash.Size = new System.Drawing.Size(409, 20);
            this.txtTxHash.TabIndex = 16;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(496, 55);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(52, 13);
            this.label9.TabIndex = 11;
            this.label9.Text = "Tx Status";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(496, 78);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(51, 13);
            this.label8.TabIndex = 10;
            this.label8.Text = "Balances";
            // 
            // listBalances
            // 
            this.listBalances.FormattingEnabled = true;
            this.listBalances.Location = new System.Drawing.Point(562, 78);
            this.listBalances.Name = "listBalances";
            this.listBalances.ScrollAlwaysVisible = true;
            this.listBalances.Size = new System.Drawing.Size(409, 43);
            this.listBalances.TabIndex = 9;
            // 
            // txtMessage
            // 
            this.txtMessage.Location = new System.Drawing.Point(113, 135);
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.ReadOnly = true;
            this.txtMessage.Size = new System.Drawing.Size(366, 20);
            this.txtMessage.TabIndex = 8;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 138);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(60, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Updated at";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 78);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(52, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Accounts";
            // 
            // listAccounts
            // 
            this.listAccounts.FormattingEnabled = true;
            this.listAccounts.Location = new System.Drawing.Point(113, 78);
            this.listAccounts.Name = "listAccounts";
            this.listAccounts.ScrollAlwaysVisible = true;
            this.listAccounts.Size = new System.Drawing.Size(366, 43);
            this.listAccounts.TabIndex = 4;
            // 
            // txtLastBlockNumber
            // 
            this.txtLastBlockNumber.Location = new System.Drawing.Point(113, 52);
            this.txtLastBlockNumber.Name = "txtLastBlockNumber";
            this.txtLastBlockNumber.ReadOnly = true;
            this.txtLastBlockNumber.Size = new System.Drawing.Size(100, 20);
            this.txtLastBlockNumber.TabIndex = 3;
            this.txtLastBlockNumber.Text = "0";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 55);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Last Block #";
            // 
            // txtRefreshRate
            // 
            this.txtRefreshRate.Location = new System.Drawing.Point(113, 25);
            this.txtRefreshRate.Name = "txtRefreshRate";
            this.txtRefreshRate.Size = new System.Drawing.Size(100, 20);
            this.txtRefreshRate.TabIndex = 1;
            this.txtRefreshRate.Text = "10";
            this.txtRefreshRate.TextChanged += new System.EventHandler(this.txtRefreshRate_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Refresh (sec)";
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.SystemColors.ControlLight;
            this.groupBox2.Controls.Add(this.label16);
            this.groupBox2.Controls.Add(this.txtData);
            this.groupBox2.Controls.Add(this.label15);
            this.groupBox2.Controls.Add(this.txtDataHex);
            this.groupBox2.Controls.Add(this.btnSendEther);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.txtValue);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.txtToAddress);
            this.groupBox2.Controls.Add(this.txtFromAddress);
            this.groupBox2.Controls.Add(this.numToAddress);
            this.groupBox2.Controls.Add(this.numFromAddress);
            this.groupBox2.Location = new System.Drawing.Point(6, 185);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(983, 103);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "2. Send Ether";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(935, 23);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(36, 13);
            this.label16.TabIndex = 19;
            this.label16.Text = "(Data)";
            // 
            // txtData
            // 
            this.txtData.Location = new System.Drawing.Point(562, 19);
            this.txtData.Name = "txtData";
            this.txtData.Size = new System.Drawing.Size(366, 20);
            this.txtData.TabIndex = 18;
            this.txtData.TextChanged += new System.EventHandler(this.txtData_TextChanged);
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(937, 48);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(32, 13);
            this.label15.TabIndex = 17;
            this.label15.Text = "(Hex)";
            // 
            // txtDataHex
            // 
            this.txtDataHex.Location = new System.Drawing.Point(562, 45);
            this.txtDataHex.Name = "txtDataHex";
            this.txtDataHex.ReadOnly = true;
            this.txtDataHex.Size = new System.Drawing.Size(366, 20);
            this.txtDataHex.TabIndex = 16;
            // 
            // btnSendEther
            // 
            this.btnSendEther.Location = new System.Drawing.Point(15, 70);
            this.btnSendEther.Name = "btnSendEther";
            this.btnSendEther.Size = new System.Drawing.Size(79, 23);
            this.btnSendEther.TabIndex = 15;
            this.btnSendEther.Text = "Send";
            this.btnSendEther.UseVisualStyleBackColor = true;
            this.btnSendEther.Click += new System.EventHandler(this.btnSendEther_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(484, 75);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(32, 13);
            this.label7.TabIndex = 14;
            this.label7.Text = "(Wei)";
            // 
            // txtValue
            // 
            this.txtValue.Location = new System.Drawing.Point(113, 72);
            this.txtValue.Name = "txtValue";
            this.txtValue.Size = new System.Drawing.Size(366, 20);
            this.txtValue.TabIndex = 13;
            this.txtValue.Text = "1";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(484, 48);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(26, 13);
            this.label6.TabIndex = 12;
            this.label6.Text = "(To)";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(484, 23);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(36, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "(From)";
            // 
            // txtToAddress
            // 
            this.txtToAddress.Location = new System.Drawing.Point(113, 46);
            this.txtToAddress.Name = "txtToAddress";
            this.txtToAddress.ReadOnly = true;
            this.txtToAddress.Size = new System.Drawing.Size(366, 20);
            this.txtToAddress.TabIndex = 10;
            // 
            // txtFromAddress
            // 
            this.txtFromAddress.Location = new System.Drawing.Point(113, 20);
            this.txtFromAddress.Name = "txtFromAddress";
            this.txtFromAddress.ReadOnly = true;
            this.txtFromAddress.Size = new System.Drawing.Size(366, 20);
            this.txtFromAddress.TabIndex = 9;
            // 
            // numToAddress
            // 
            this.numToAddress.Location = new System.Drawing.Point(15, 46);
            this.numToAddress.Name = "numToAddress";
            this.numToAddress.Size = new System.Drawing.Size(79, 20);
            this.numToAddress.TabIndex = 1;
            this.numToAddress.ValueChanged += new System.EventHandler(this.numToAddress_ValueChanged);
            // 
            // numFromAddress
            // 
            this.numFromAddress.Location = new System.Drawing.Point(15, 20);
            this.numFromAddress.Name = "numFromAddress";
            this.numFromAddress.Size = new System.Drawing.Size(79, 20);
            this.numFromAddress.TabIndex = 0;
            this.numFromAddress.ValueChanged += new System.EventHandler(this.numFromAddress_ValueChanged);
            // 
            // groupBox3
            // 
            this.groupBox3.BackColor = System.Drawing.SystemColors.ControlLight;
            this.groupBox3.Controls.Add(this.label14);
            this.groupBox3.Controls.Add(this.txtVoteFromAddress);
            this.groupBox3.Controls.Add(this.btnDelegateToAddress);
            this.groupBox3.Controls.Add(this.numVoteFromAddress);
            this.groupBox3.Controls.Add(this.btnRightToVote);
            this.groupBox3.Controls.Add(this.btnGetWinningProposal);
            this.groupBox3.Controls.Add(this.btnSendVote);
            this.groupBox3.Controls.Add(this.txtRightToVoteAddress);
            this.groupBox3.Controls.Add(this.txtDelegateToAddress);
            this.groupBox3.Controls.Add(this.txtWinningProposal);
            this.groupBox3.Controls.Add(this.txtVoteForProposal);
            this.groupBox3.Controls.Add(this.txtContractAbi);
            this.groupBox3.Controls.Add(this.label11);
            this.groupBox3.Controls.Add(this.label10);
            this.groupBox3.Controls.Add(this.txtContractAddress);
            this.groupBox3.Location = new System.Drawing.Point(6, 298);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(983, 109);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "3. Ballot.sol";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(485, 49);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(96, 13);
            this.label14.TabIndex = 33;
            this.label14.Text = "Vote From Address";
            // 
            // txtVoteFromAddress
            // 
            this.txtVoteFromAddress.Location = new System.Drawing.Point(685, 46);
            this.txtVoteFromAddress.Name = "txtVoteFromAddress";
            this.txtVoteFromAddress.ReadOnly = true;
            this.txtVoteFromAddress.Size = new System.Drawing.Size(284, 20);
            this.txtVoteFromAddress.TabIndex = 17;
            // 
            // btnDelegateToAddress
            // 
            this.btnDelegateToAddress.Location = new System.Drawing.Point(15, 75);
            this.btnDelegateToAddress.Name = "btnDelegateToAddress";
            this.btnDelegateToAddress.Size = new System.Drawing.Size(92, 23);
            this.btnDelegateToAddress.TabIndex = 32;
            this.btnDelegateToAddress.Text = "Delegate";
            this.btnDelegateToAddress.UseVisualStyleBackColor = true;
            this.btnDelegateToAddress.Click += new System.EventHandler(this.btnDelegateToAddress_Click);
            // 
            // numVoteFromAddress
            // 
            this.numVoteFromAddress.Location = new System.Drawing.Point(600, 46);
            this.numVoteFromAddress.Name = "numVoteFromAddress";
            this.numVoteFromAddress.Size = new System.Drawing.Size(79, 20);
            this.numVoteFromAddress.TabIndex = 16;
            this.numVoteFromAddress.ValueChanged += new System.EventHandler(this.numVoteFromAddress_ValueChanged);
            // 
            // btnRightToVote
            // 
            this.btnRightToVote.Location = new System.Drawing.Point(15, 43);
            this.btnRightToVote.Name = "btnRightToVote";
            this.btnRightToVote.Size = new System.Drawing.Size(92, 23);
            this.btnRightToVote.TabIndex = 31;
            this.btnRightToVote.Text = "Right to Vote";
            this.btnRightToVote.UseVisualStyleBackColor = true;
            this.btnRightToVote.Click += new System.EventHandler(this.btnRightToVote_Click);
            // 
            // btnGetWinningProposal
            // 
            this.btnGetWinningProposal.Location = new System.Drawing.Point(746, 72);
            this.btnGetWinningProposal.Name = "btnGetWinningProposal";
            this.btnGetWinningProposal.Size = new System.Drawing.Size(108, 23);
            this.btnGetWinningProposal.TabIndex = 16;
            this.btnGetWinningProposal.Text = "Winning Proposal";
            this.btnGetWinningProposal.UseVisualStyleBackColor = true;
            this.btnGetWinningProposal.Click += new System.EventHandler(this.btnGetWinningProposal_Click);
            // 
            // btnSendVote
            // 
            this.btnSendVote.Location = new System.Drawing.Point(485, 73);
            this.btnSendVote.Name = "btnSendVote";
            this.btnSendVote.Size = new System.Drawing.Size(108, 23);
            this.btnSendVote.TabIndex = 30;
            this.btnSendVote.Text = "Vote";
            this.btnSendVote.UseVisualStyleBackColor = true;
            this.btnSendVote.Click += new System.EventHandler(this.btnSendVote_Click);
            // 
            // txtRightToVoteAddress
            // 
            this.txtRightToVoteAddress.Location = new System.Drawing.Point(113, 45);
            this.txtRightToVoteAddress.Name = "txtRightToVoteAddress";
            this.txtRightToVoteAddress.Size = new System.Drawing.Size(366, 20);
            this.txtRightToVoteAddress.TabIndex = 28;
            // 
            // txtDelegateToAddress
            // 
            this.txtDelegateToAddress.Location = new System.Drawing.Point(113, 76);
            this.txtDelegateToAddress.Name = "txtDelegateToAddress";
            this.txtDelegateToAddress.Size = new System.Drawing.Size(366, 20);
            this.txtDelegateToAddress.TabIndex = 26;
            // 
            // txtWinningProposal
            // 
            this.txtWinningProposal.Location = new System.Drawing.Point(860, 73);
            this.txtWinningProposal.Name = "txtWinningProposal";
            this.txtWinningProposal.ReadOnly = true;
            this.txtWinningProposal.Size = new System.Drawing.Size(109, 20);
            this.txtWinningProposal.TabIndex = 24;
            // 
            // txtVoteForProposal
            // 
            this.txtVoteForProposal.Location = new System.Drawing.Point(600, 75);
            this.txtVoteForProposal.Name = "txtVoteForProposal";
            this.txtVoteForProposal.Size = new System.Drawing.Size(109, 20);
            this.txtVoteForProposal.TabIndex = 23;
            this.txtVoteForProposal.Text = "3";
            // 
            // txtContractAbi
            // 
            this.txtContractAbi.Location = new System.Drawing.Point(515, 19);
            this.txtContractAbi.Name = "txtContractAbi";
            this.txtContractAbi.Size = new System.Drawing.Size(456, 20);
            this.txtContractAbi.TabIndex = 21;
            this.txtContractAbi.Text = resources.GetString("txtContractAbi.Text");
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(485, 22);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(24, 13);
            this.label11.TabIndex = 20;
            this.label11.Text = "ABI";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(14, 22);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(88, 13);
            this.label10.TabIndex = 19;
            this.label10.Text = "Contract Address";
            // 
            // txtContractAddress
            // 
            this.txtContractAddress.Location = new System.Drawing.Point(113, 19);
            this.txtContractAddress.Name = "txtContractAddress";
            this.txtContractAddress.Size = new System.Drawing.Size(366, 20);
            this.txtContractAddress.TabIndex = 18;
            this.txtContractAddress.Text = "0xf891d6e4e23c8035773b787b7984040891fbd1c2";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(996, 479);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "MainForm";
            this.Text = "MWH Nethereum - Ballot.sol";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numToAddress)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numFromAddress)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numVoteFromAddress)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        public System.Windows.Forms.TextBox txtLastBlockNumber;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtRefreshRate;
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.CheckBox chkPaused;
        private System.Windows.Forms.Label label3;
        public System.Windows.Forms.ListBox listAccounts;
        public System.Windows.Forms.TextBox txtMessage;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox2;
        public System.Windows.Forms.TextBox txtToAddress;
        public System.Windows.Forms.TextBox txtFromAddress;
        public System.Windows.Forms.NumericUpDown numToAddress;
        public System.Windows.Forms.NumericUpDown numFromAddress;
        private System.Windows.Forms.Button btnSendEther;
        private System.Windows.Forms.Label label7;
        public System.Windows.Forms.TextBox txtValue;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label8;
        public System.Windows.Forms.ListBox listBalances;
        public System.Windows.Forms.TextBox txtTxStatus;
        private System.Windows.Forms.Label label9;
        public System.Windows.Forms.TextBox txtTxHash;
        public System.Windows.Forms.TextBox txtStartedMessage;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        public System.Windows.Forms.TextBox txtContractAddress;
        public System.Windows.Forms.TextBox txtContractAbi;
        public System.Windows.Forms.TextBox txtRightToVoteAddress;
        public System.Windows.Forms.TextBox txtDelegateToAddress;
        public System.Windows.Forms.TextBox txtWinningProposal;
        public System.Windows.Forms.TextBox txtVoteForProposal;
        private System.Windows.Forms.Button btnGetWinningProposal;
        private System.Windows.Forms.Button btnSendVote;
        private System.Windows.Forms.Button btnRightToVote;
        private System.Windows.Forms.Button btnDelegateToAddress;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label12;
        public System.Windows.Forms.TextBox txtVoteFromAddress;
        public System.Windows.Forms.NumericUpDown numVoteFromAddress;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Button btnTxScan;
        private System.Windows.Forms.Label label16;
        public System.Windows.Forms.TextBox txtData;
        private System.Windows.Forms.Label label15;
        public System.Windows.Forms.TextBox txtDataHex;
        public System.Windows.Forms.TextBox txtCurrentBlock;
        public System.Windows.Forms.TextBox txtStartingBlock;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label20;
        public System.Windows.Forms.TextBox txtHighestBlock;
        public System.Windows.Forms.Label lblSyncing;
    }
}

