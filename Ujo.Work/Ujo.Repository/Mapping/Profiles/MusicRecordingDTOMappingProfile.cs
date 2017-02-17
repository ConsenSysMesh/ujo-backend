using AutoMapper;
using Ujo.Messaging;
using Ujo.Model;

namespace Ujo.Repository
{
    public class MusicRecordingDTOMappingProfile : Profile
    {
        public MusicRecordingDTOMappingProfile()
        {
            CreateMap<MusicRecordingDTO, MusicRecording>()
                .ForMember(x => x.ObjectState, opt => opt.Ignore())
                .ForMember(x => x.ByArtist, opt => opt.Ignore())
                .ForMember(x => x.CreativeWorksIn, opt => opt.Ignore())
                .ForMember(x => x.CreativeWorksUsed, opt => opt.Ignore());
            CreateMap<CreativeWorkDTO, CreativeWork>()
                .ForMember(x => x.ObjectState, opt => opt.Ignore())
                .ForMember(x => x.ByArtist, opt => opt.Ignore())
                .ForMember(x => x.CreativeWorksIn, opt => opt.Ignore())
                .ForMember(x => x.CreativeWorksUsed, opt => opt.Ignore());
            CreateMap<CreativeWorkArtistDTO, CreativeWorkArtist>()
                .ForMember(x => x.ObjectState, opt => opt.Ignore())
                .ForMember(x => x.Artist, opt => opt.Ignore())
                .ForMember(x => x.CreativeWork, opt => opt.Ignore())
                .ForMember(x => x.CreativeWorkArtistId, opt => opt.Ignore());
        }
    }
}