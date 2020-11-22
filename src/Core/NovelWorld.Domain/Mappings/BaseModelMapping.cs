using System.Collections.Generic;
using AutoMapper;
using NovelWorld.Data.Entities;
using NovelWorld.Data.Responses;

namespace NovelWorld.Domain.Mappings
{
    public class BaseModelMapping : Profile
    {
        public BaseModelMapping()
        {
            CreateMap<Entity, EntityResponse>().IncludeAllDerived().ReverseMap();
            CreateMap(typeof(PagingResponse<>), typeof(PagingResponse<>)).ConvertUsing(typeof(PagingResponseConverter<,>));
        }
    }

    public class PagingResponseConverter<TSource, TDestination> : ITypeConverter<PagingResponse<TSource>, PagingResponse<TDestination>>
    {
        public PagingResponse<TDestination> Convert(PagingResponse<TSource> source, PagingResponse<TDestination> destination, ResolutionContext context)
        {
            return new PagingResponse<TDestination>()
            {
                PageSize = source.PageSize,
                Total = source.Total,
                TotalPage = source.TotalPage,
                Data = context.Mapper.Map<IEnumerable<TDestination>>(source.Data)
            };
        }
    }
}
