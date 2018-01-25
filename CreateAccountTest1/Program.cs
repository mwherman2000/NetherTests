using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nethereum.Hex.HexTypes;
using Nethereum.Web3;

namespace CreateAccountTest1
{
    class Program
    {
        static void Main(string[] args)
        {
            string password = "apassword";

            var ecKey = Nethereum.Signer.EthECKey.GenerateKey();
            var privateKey0 = ecKey.GetPrivateKeyAsBytes();
            var account0 = new Nethereum.Web3.Accounts.Account(privateKey0);

            var managed0 = new Nethereum.Web3.Accounts.Managed.ManagedAccount(account0.Address, password);
            Console.WriteLine("managed.Address:\t" + managed0.Address);
            Console.WriteLine("account.PrivateKey:\t" + account0.PrivateKey);

            var password2 = "PASSWORD";
            var accountPathMain = @"C:\Users\mwher\AppData\Roaming\Ethereum\keystore\";
            var accountFile2 = @"UTC--2017-12-18T00-28-50.564289200Z--4504d2bf0378f3aa6f5b20214e1334fcfb02f10b";
            var accountFile3 = @"UTC--2017-12-28T14-43-35.295098900Z--253b120af53edfc54626b141409b956eadbc4adb";

            var accountPathFile2 = accountPathMain + accountFile2;
            var account2 = Nethereum.Web3.Accounts.Account.LoadFromKeyStoreFile(accountPathFile2, password2);

            var accountPathFile3 = accountPathMain + accountFile3;
            var account3 = Nethereum.Web3.Accounts.Account.LoadFromKeyStoreFile(accountPathFile3, password2);

            Web3 web3 = new Web3("https://mainnet.infura.io/hZeiirtHOLO11uuyLySi");

            Console.WriteLine("account2.Address:\t" + account2.Address);
            Console.WriteLine("account2.PrivateKey:\t" + account2.PrivateKey);
            Task task2 = GetAccountBalanceExample(web3, account2.Address);
            task2.Wait();

            Console.WriteLine("account3.Address:\t" + account3.Address);
            Console.WriteLine("account3.PrivateKey:\t" + account3.PrivateKey);
            Task task3 = GetAccountBalanceExample(web3, account3.Address);
            task3.Wait();

            var accountPathRinkeby = @"C:\Users\mwher\AppData\Roaming\Ethereum\rinkeby\keystore\";
            var accountFile4 = @"UTC--2017-12-10T19-45-17.627687400Z--3866e56fdb1de93186a93215f1c13cd1e4c94174";
            var accountFile5 = @"UTC--2017-12-10T19-52-06.054028800Z--253b120af53edfc54626b141409b956eadbc4adb";

            var accountPathFile4 = accountPathRinkeby + accountFile4;
            var account4 = Nethereum.Web3.Accounts.Account.LoadFromKeyStoreFile(accountPathFile4, password2);
            var accountPathFile5 = accountPathRinkeby + accountFile5;
            var account5 = Nethereum.Web3.Accounts.Account.LoadFromKeyStoreFile(accountPathFile5, password2);


            Web3 web4 = new Web3(account4, "https://rinkeby.infura.io");

            Console.WriteLine("account4.Address:\t" + account4.Address);
            Console.WriteLine("account4.PrivateKey:\t" + account4.PrivateKey);
            Task task4 = GetAccountBalanceExample(web4, account4.Address);
            task4.Wait();
            Console.WriteLine("account5.Address:\t" + account5.Address);
            Console.WriteLine("account5.PrivateKey:\t" + account5.PrivateKey);
            Task task5 = GetAccountBalanceExample(web4, account5.Address);
            task5.Wait();

            Task<string> taskSend = SendEther(web4, account4.Address, account5.Address, 123);
            taskSend.Wait();
            string result = taskSend.Result;
            // TODO: Wait for transaction receipt

            Console.WriteLine("account4.Address:\t" + account4.Address);
            Console.WriteLine("account4.PrivateKey:\t" + account4.PrivateKey);
            task4 = GetAccountBalanceExample(web4, account4.Address);
            task4.Wait();
            Console.WriteLine("account5.Address:\t" + account5.Address);
            Console.WriteLine("account5.PrivateKey:\t" + account5.PrivateKey);
            task5 = GetAccountBalanceExample(web4, account5.Address);
            task5.Wait();

            string password0 = "apassword";
            string ad0 = "0x54DD36f6AB5c078c41DFc03c07e1c84DD2c2D1d6";
            string pk0 = "0x2fb14780124c55138255578a892169ef9bc41e89807db2e417f1e0890fe7753a";
            var a0 = new Nethereum.Web3.Accounts.Account(pk0);
            var ma0 = new Nethereum.Web3.Accounts.Managed.ManagedAccount(a0.Address, password0);

            //Task<string> taskSend2 = SendEther(web4, account4.Address, a0.Address, 50000);
            //taskSend2.Wait();
            //string result2 = taskSend.Result;
            //// TODO: Wait for transaction receipt

            Web3 web5 = new Web3(a0, "https://rinkeby.infura.io");

            Console.WriteLine("a0.Address:\t" + a0.Address);
            Console.WriteLine("a0.PrivateKey:\t" + a0.PrivateKey);
            task5 = GetAccountBalanceExample(web4, a0.Address);
            task5.Wait();

            //Task<string> taskSend5 = SendEther(web5, a0.Address, account4.Address, 1);
            //taskSend5.Wait();
            //string result5 = taskSend.Result;
            //// TODO: Wait for transaction receipt

            Web3 web6 = new Web3("https://rinkeby.infura.io");
            web6 = new Web3();
            Task<string> task6 = CreateAccountTest1(web6, "foobar");
            task6.Wait();
            string newAddr = task6.Result;
            Console.WriteLine("new account:\t" + newAddr);


            Console.WriteLine("Press Enter to exit...");
            Console.ReadLine();
        }

        static async Task<string> CreateAccountTest1(Web3 web3, string password)
        {
            string s;

            s = await web3.Personal.NewAccount.SendRequestAsync(password);

            return s;
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

        static public async Task<string> SendEther(Web3 web3, string from, string to, uint amount)
        {
            var result = await web3.TransactionManager.SendTransactionAsync(from, to, new HexBigInteger(amount));
            return result;
        }
    }
}
