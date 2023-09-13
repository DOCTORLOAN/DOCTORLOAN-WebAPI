using AutoMapper;

namespace DoctorLoan.Application.Common.Extentions;
public static class MapperExtension
{
    public static TTo MapperTo<TFrom, TTo>(this TFrom from) where TFrom : class, new()
    {
        return EngineContext.GetService<IMapper>().Map<TFrom,TTo>(from);
    }
    public static TTo MapperTo<TFrom, TTo>(this TFrom from, TTo des) where TFrom : class, new()
    {
        return EngineContext.GetService<IMapper>().Map(from, des);
    }
}
