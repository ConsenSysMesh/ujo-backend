contract WorkRegistry {

    uint public numRecords;

    uint public maxId;
     
    struct Record {
        address registeredAddress;
        // Keeps the address of this record creator.
        address owner;
        // Keeps the time when this record was created.
        uint time;
        // Keeps the id of the record
        uint Id;
    }

    mapping(address => Record) public records;
    mapping(uint => address) public workRegistered;

    function WorkRegistry() {
        maxId=0;
        numRecords=0;
    }

     event Registered(address indexed registeredAddress,  uint indexed id,  address indexed owner, uint time);
     event Unregistered( address indexed registeredAddress,  uint indexed id);

     function register(address registeredAddress) {
        if (records[registeredAddress].time == 0) {
            records[registeredAddress].time = now;
            records[registeredAddress].owner = msg.sender;
            maxId = maxId + 1;
            records[registeredAddress].Id = maxId;
            workRegistered[maxId] = registeredAddress;
            numRecords = numRecords + 1;
            Registered(registeredAddress, records[registeredAddress].Id, records[registeredAddress].owner, records[registeredAddress].time);
        }
     }
    
    function unregister(address registeredAddress) {
        if (records[registeredAddress].owner == msg.sender) {
            uint id = records[registeredAddress].Id;
            delete records[registeredAddress];
            numRecords--;
            delete workRegistered[id];
            Unregistered(registeredAddress, id);
        }
    }
}