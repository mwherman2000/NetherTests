﻿pragma solidity ^0.4.19;

// Reference: https://theethereum.wiki/w/index.php/ERC20_Token_Standard#Further_Information_On_Ethereum_Tokens
  
// Account1: 0x3866E56fDb1DE93186a93215f1C13Cd1E4C94174
// Account2: 0x253B120Af53edfC54626B141409b956eADbC4adb
// Contract: 0x72b766028d83b7ad95bf7ad74e37de009e4cc9c0

// Owner: 0x3866e56fdb1de93186a93215f1c13cd1e4c94174

// ----------------------------------------------------------------------------------------------
// Sample fixed supply token contract
// Enjoy. (c) BokkyPooBah 2017. The MIT Licence.
// ----------------------------------------------------------------------------------------------
 
// ERC Token Standard #20 Interface
// https://github.com/ethereum/EIPs/issues/20
contract ERC20Interface {
    // Get the total token supply
    function totalSupply() public constant returns (uint256 totalSupply);
 
    // Get the account balance of another account with address _owner
    function balanceOf(address _owner) public constant returns (uint256 balance);
 
    // Send _value amount of tokens to address _to
    function transfer(address _to, uint256 _value) public returns (bool success);
  
     // Send _value amount of tokens from address _from to address _to
    function transferFrom(address _from, address _to, uint256 _value) public returns (bool success);
 
    // Allow _to to withdraw from your account, multiple times, up to the _value amount.
    // If this function is called again it overwrites the current allowance with _value.
    // this function is required for some DEX functionality
    function approve(address _from, address _to, uint256 _value) public returns (bool success);
 
    // Returns the amount which _to is still allowed to withdraw from _from
    function allowance(address _from, address _to) public constant returns (uint256 remaining);
 
    // Triggered when tokens are transferred.
    event Transfer(address indexed _from, address indexed _to, uint256 _value);
 
    // Triggered whenever approve(address _spender, uint256 _value) is called.
    event Approval(address indexed _from, address indexed _to, uint256 _value);
}
 
contract FixedSupplyToken is ERC20Interface {
    string public constant symbol = "FIXED";
    string public constant name = "Example Fixed Supply Token";
    uint8 public constant decimals = 18;
    uint256 _totalSupply = 1000000;
    uint256 public _lastAmount = 0;
    uint256 public _amount1 = 0;
    uint256 public _amount2 = 0;
    uint256 public _amount3 = 0;
    
    // Owner of this contract
    address public owner;
 
    // Balances for each account
    mapping(address => uint256) balances;
 
    // Owner of account approves the transfer of an amount to another account
    mapping(address => mapping (address => uint256)) allowed;
 
    // Functions with this modifier can only be executed by the owner
    modifier onlyOwner() {
        if (msg.sender != owner) {
            throw;
        }
        _;
    }
    
    // Triggered when tokens are transferred.
    event Transfer(address indexed _from, address indexed _to, uint256 _value);
 
    // Triggered whenever approve(address _spender, uint256 _value) is called.
    event Approval(address indexed _owner, address indexed _spender, uint256 _value);
 
    // Constructor
    function FixedSupplyToken() {
        owner = msg.sender;
        balances[owner] = _totalSupply;
        _lastAmount = _totalSupply;
    }
 
    function totalSupply() public constant returns (uint256 totalSupply) {
        totalSupply = _totalSupply;
    }
 
    // What is the balance of a particular account?
    function balanceOf(address _owner) public constant returns (uint256 balance) {
        return balances[_owner];
    }
 
    // Transfer the balance from owner's account to another account
    function transfer(address _to, uint256 _amount) public returns (bool success) {
        _lastAmount = _amount;
        _amount1 = balances[msg.sender];
        _amount2 = balances[_to];
        if (balances[msg.sender] >= _amount 
            && _amount > 0
            && balances[_to] + _amount > balances[_to]) {
            balances[msg.sender] -= _amount;
            balances[_to] += _amount;
            Transfer(msg.sender, _to, _amount);
            return true;
        } else {
            return false;
        }
    }
 
    // Send _value amount of tokens from address _from to address _to
    // The transferFrom method is used for a withdraw workflow, allowing contracts to send
    // tokens on your behalf, for example to "deposit" to a contract address and/or to charge
    // fees in sub-currencies; the command should fail unless the _from account has
    // deliberately authorized the sender of the message via some mechanism; we propose
    // these standardized APIs for approval:
    function transferFrom(
        address _from,
        address _to,
        uint256 _amount
    ) public returns (bool success) {
       _lastAmount = _amount;
       _amount1 = balances[msg.sender];
       _amount2 = balances[_from];
       _amount3 = allowed[_from][_to];
       if (balances[_from] >= _amount
            && allowed[_from][_to] >= _amount
            && _amount > 0
            && balances[_to] + _amount > balances[_to]) {
            balances[_from] -= _amount;
            allowed[_from][_to] -= _amount;
            balances[_to] += _amount; 
			Transfer(_from, _to, _amount);
            return true;
        } else {
            return false;
        }
    }
 
    // Allow _spender to withdraw from your account, multiple times, up to the _value amount.
    // If this function is called again it overwrites the current allowance with _value.
    function approve(address _from, address _to, uint256 _amount) public returns (bool success) {
        _lastAmount = _amount;
        allowed[_from][_to] = _amount;
        Approval(_from, _to, _amount);
        return true;
    }
 
    function allowance(address _from, address _to) public constant returns (uint256 remaining) {
        return allowed[_from][_to];
    }
}