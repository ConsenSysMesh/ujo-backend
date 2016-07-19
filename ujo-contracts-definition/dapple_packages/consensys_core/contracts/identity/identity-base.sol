import '../auth/auth.sol';

contract CCIdentityBase is CCAuth{
    string name; // NOTE: This could be like DappSys storage 
    string imageHash;
    string metadataHash;
    string[] attachementHashes;
    
    // NOTE: This could be like DappSys storage 
    event SetName(string name);
    function setName(string name) auth() returns (bool success); 
    function getName() returns (string name); 
     
}