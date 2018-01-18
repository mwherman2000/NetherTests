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
    }

    public class BlockItem : Block
    {
        public int TxCount;
    }
}
