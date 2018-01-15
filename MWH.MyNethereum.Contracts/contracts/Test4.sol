pragma solidity 0.4.19;

contract Test4 {

    int public _product;
    string[5] _msgs;
    
    function Test4() public {
        _product = 1;
    }
    
    event MultipliedEvent(
        address indexed sender,
        int oldProduct,
        int value,
        int newProduct
        );
        
    event NewMessageEvent(
        address indexed sender,
        uint256 indexed ind,
        string msg
        );

    function multiply(int value) public returns(int product) {
        int old = _product;
        _product = value * _product;
        MultipliedEvent( msg.sender, old, value, _product );
        return _product;
    }
    
    function getProduct() public constant returns(int product) {
        return _product;
    }

    function setMsg(uint256 i, string m) public returns(uint256 mi) {
        _msgs[i] = m;
        NewMessageEvent( msg.sender, i, m);
        return -i;
    }

    function getMsg(uint256 index) public constant returns(string m) {
        return _msgs[index];
    }
    
    event DepositReceipt(
        uint indexed timestamp,
        address indexed from,
        bytes32 indexed id,
        uint _value
    );

    function deposit(bytes32 _id) public payable {
        // Any call to this function (even deeply nested) can
        // be detected from the JavaScript API by filtering
        // for `Deposit` to be called.
        DepositReceipt(now, msg.sender, _id, msg.value);
    }
}