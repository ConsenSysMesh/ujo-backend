contract CCAuth {
    modifier auth() {
        if( isAuthorized() ) {
            _
        } else {
            throw;
        }
    }
    
     function isAuthorized() internal returns (bool is_authorized) {
        return true;
    }
}