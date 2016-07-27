contract CCContractRegistry {
    
    public uint numberOfContractsRegistered;

    public mapping  (uint=>address) registeredContracts;
    
    function RegisterContract(address contract) returns bool {
       //todo
      ContractRegistered(contract);  
    }
    event ContractRegistered(indexed address contract);    
}
