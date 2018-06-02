using AutoMapper;
using MongoDB.Bson;
using Playground.Domain;
using Playground.Dto;
using X.PagedList;

namespace PlaygroundApi.Mapping
{
    public class PlaygroundProfile : Profile
    {
        public override string ProfileName => nameof(PlaygroundProfile);

        public PlaygroundProfile()
        {
            CreateMap(typeof(IPagedList<>), typeof(PagedListDto<>)).ConvertUsing(typeof(PagedListToDtoConverter<,>));
            CreateMap<Item, ItemDto>();
            CreateMap<ItemDto, Item>();
            CreateMap<string, ObjectId>().ConvertUsing<StringToObjectIdConverter>();
            CreateMap<Operation, OperationDto>();
            CreateMap<Difference, DifferenceDto>();
        }
    }
}
