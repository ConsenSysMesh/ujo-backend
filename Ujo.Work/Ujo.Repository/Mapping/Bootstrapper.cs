using AutoMapper;

namespace Ujo.Repository
{
    public static class MappingBootstrapper
    {
        public static void Initialise()
        {
            Mapper.Initialize(cfg => {
                cfg.AddProfile<MusicRecordingDTOMappingProfile>();
            });
        }
    }
}