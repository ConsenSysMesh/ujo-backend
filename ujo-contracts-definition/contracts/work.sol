import 'consensys_core/identity/identity-base.sol';

contract UjoWork is CCIdentityBase {
      address creatorIdentity;//
      address administatorIdentity; //??

}

//contain all the licenses that a work has
contract UjoWorkLicenceRegistry {


}

//contains all the works with the licenses
contract UjoGlobalLicenseRegistry {
    //work  ->  license* 
    mapping (address=>address) licenses;
}

contract UjoLicense {
    //identities split
    //types
    //etc
}