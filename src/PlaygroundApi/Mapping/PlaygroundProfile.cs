using System;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
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

            CreateMap<BsonDocument, dynamic>()
                .ConvertUsing((document, y) =>
                {
                    if (document == null)
                        return null;

                    var json = document.ToJson(new JsonWriterSettings { OutputMode = JsonOutputMode.Strict });
                    return Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(json);
                });

            CreateMap<dynamic, BsonDocument>()
                .ConvertUsing((x, y) =>
                {
                    var json = (x == null) ? "{}" : Newtonsoft.Json.JsonConvert.SerializeObject(x);
                    BsonDocument document = BsonDocument.Parse(json);
                    return document;
                });

            CreateMap<ApiVersionDescription, VersionDescriptionDto>()
                .ForMember(dest => dest.Version, opt => opt.MapFrom(src => src.ApiVersion))
                .ForMember(dest => dest.IsDeprecated, opt => opt.MapFrom(src => src.IsDeprecated));
        }
    }
}
