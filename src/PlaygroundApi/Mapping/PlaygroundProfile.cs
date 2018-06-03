using System;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Playground.Domain;
using Playground.Dto;
using PlaygroundApi.Controllers;
using X.PagedList;

namespace PlaygroundApi.Mapping
{
    public class PlaygroundProfile : Profile
    {
        public override string ProfileName => nameof(PlaygroundProfile);

        public PlaygroundProfile(IServiceProvider serviceProvider)
        {
            CreateMap(typeof(IPagedList<>), typeof(PagedListDto<>)).ConvertUsing(typeof(PagedListToDtoConverter<,>));

            CreateMap<Item, ItemDto>()
                .ForMember(dest => dest.Links, opt => opt.Ignore())
                .AfterMap((src, dest) =>
                {
                    if (dest.Id != null)
                    {
                        var urlHelper = (IUrlHelper)serviceProvider.GetService(typeof(IUrlHelper));
                        dest.Links[ResourceBase.RelationNameOperations] = urlHelper.Link(OperationsController.RouteNameGetAsync, new OperationSearchParameter { EntityId = dest.Id });
                        dest.Links[ResourceBase.RelationNameSelf] = urlHelper.Link(ItemsController.RouteNameGetById, new { id = dest.Id });
                    }
                });

            CreateMap<ItemDto, Item>();
            CreateMap<string, ObjectId>().ConvertUsing<StringToObjectIdConverter>();
            CreateMap<Operation, OperationDto>();
            CreateMap<Difference, DifferenceDto>();
        }
    }
}
