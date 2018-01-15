pragma solidity 0.4.19;

contract Test0 {
    int public foo = 123;
}

contract Test2 {

    int public _product;
    string[5] _msgs;

    function Test2() public {
        _product = 1;
    }

    function multiply(int value) public returns(int product) {
        _product = value * _product;
        return _product;
    }
    
    function getProduct() public constant returns(int product) {
        return _product;
    }

    function setMsg(uint256 index, string m) public returns(uint256 i) {
        _msgs[index] = m;
        return -index;
    }

    function getMsg(uint256 index) public constant returns(string m) {
        return _msgs[index];
    }
}