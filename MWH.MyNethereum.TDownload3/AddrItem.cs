using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MWH.MyNethereum.TDownload
{
    public enum AddressType
    {
        Unknown,
        Account,
        Contract
    }

    public enum AddressEndpointType
    {
        Unknown,
        From,
        To
    }

    public class AddrItem
    {
        public string Address;
        public AddressType AddrType;
        public AddressEndpointType EndPointType;
        public HexBigInteger BlockNumber;
        public string TxHash;
        public HexBigInteger TxValue;

        public HexBigInteger TxTimestamp;
        public int TxYear;
        public int TxMonth;
        public int TxWeek;
        public int TxDayOfYear;
        public int TxDayOfMonth;
        public int TxDayOfWeek;
        public int TxHour;
    }

    public class BlockItem : Block
    {
        public int TxCount;

        public int BlYear;
        public int BlMonth;
        public int BlWeek;
        public int BlDayOfYear;
        public int BlDayOfMonth;
        public int BlDayOfWeek;
        public int BlHour;
    }

    public class TxItem : Transaction
    {
        public HexBigInteger TxTimestamp;
        public int TxYear;
        public int TxMonth;
        public int TxWeek;
        public int TxDayOfYear;
        public int TxDayOfMonth;
        public int TxDayOfWeek;
        public int TxHour;
    }
}
