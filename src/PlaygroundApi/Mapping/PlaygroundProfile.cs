using AutoMapper;
using Playground.Domain;
using Playground.Dto;

namespace PlaygroundApi.Mapping
{
    public class PlaygroundProfile : Profile
    {
        public override string ProfileName => nameof(PlaygroundProfile);

        public PlaygroundProfile()
        {
            CreateMap<Item, ItemDto>();
            CreateMap<ItemDto, Item>();
        }
    }
}
