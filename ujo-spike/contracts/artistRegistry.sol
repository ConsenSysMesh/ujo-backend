contract ArtistEntityRegistry{
    
    struct ArtistEntity {
       string name;
       bool isAGroup;
       string category;      
    }
    
    event ArtistEntityAdded(uint artityEntityId, string name, bool isAGroup, string category);
    
    function numberOfArtistEntities() constant returns (uint _numberOfArtists) {
        return artistEntities.length;
    } 
    
    ArtistEntity[] public artistEntities;
    
    function registerArtistEntity(string name, bool isAGroup, string category) returns (bool success) {
        var _artistId = artistEntities.length++;
        ArtistEntity _artist = artistEntities[_artistId];
        _artist.name = name;
        _artist.isAGroup = isAGroup;
        _artist.category = category;
        ArtistEntityAdded(_artistId, name, isAGroup, category);
        return true;
    }
}