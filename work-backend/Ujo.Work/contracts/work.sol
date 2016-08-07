contract UjoWork {

    //public attributes 
    mapping (uint=>string) public store;
    
    event DataChanged(uint indexed key, string value);
    
    function setAttribute(uint key, string value) returns (bool success) { //auth
        store[key] = value;
        DataChanged(key, value);
        return true;
    }
}