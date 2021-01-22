using System.Collections.Generic;
using AutoMapper;
using NovelWorld.Data.DTO;
using NovelWorld.Data.Entities;
using NovelWorld.Data.Responses;

namespace NovelWorld.Domain.Mappings
{
    public class BaseModelMapping : Profile
    {
        public BaseModelMapping()
        {
            CreateMap<Entity, EntityResponse>().IncludeAllDerived().ReverseMap();
            CreateMap(typeof(PagedData<>), typeof(PagedData<>)).ConvertUsing(typeof(PagingResponseConverter<,>));
        }
    }

    public class PagingResponseConverter<TSource, TDestination> : ITypeConverter<PagedData<TSource>, PagedData<TDestination>>
    {
        public PagedData<TDestination> Convert(PagedData<TSource> source, PagedData<TDestination> destination, ResolutionContext context)
        {
            return new PagedData<TDestination>()
            {
                PageSize = source.PageSize,
                Total = source.Total,
                TotalPage = source.TotalPage,
                Data = context.Mapper.Map<IEnumerable<TDestination>>(source.Data)
            };
        }
    }
}
