import '../auth/auth.sol';

contract CCIdentityBase is CCAuth{
    string metadataHash; //includes everything like image, name, attachements any extra information
    //any changes are notified and we deserialise the json ipld, and look for changes 
    // (or not just force update) , re resize images and any post processing there.
    //potentially we can use mediachain as they have a database / which writes to ipfs
}