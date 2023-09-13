using System.Linq.Expressions;
using DoctorLoan.Domain.Common;
using DoctorLoan.Domain.Enums.Commons;
using DoctorLoan.Domain.Interfaces;

namespace DoctorLoan.Application.Interfaces.Commons;

public interface ILocalizedEntityService
{
    Task<TPropType> GetLocalizedAsync<TEntity, TPropType>(TEntity entity, Expression<Func<TEntity, TPropType>> keySelector,
             LanguageEnum? language = null)
            where TEntity : IBaseEntity<int>, ILocalizedEntity;

    Task<string> GetLocalizedValueAsync(int languageId, int entityId, string localeKeyGroup, string localeKey);
    Task SaveLocalizedPropertiesAsync(int id, string localeKeyGroup, int languageId, string localeKey, string localeValueStr);
    Task SaveLocalizedValueAsync<T>(T entity,
        Expression<Func<T, string>> keySelector,
        string localeValue,
        int languageId) where T : IBaseEntity<int>, ILocalizedEntity;
    Task SaveLocalizedValueAsync<T, TPropType>(T entity,
       Expression<Func<T, TPropType>> keySelector,
       TPropType localeValue,
       int languageId) where T : IBaseEntity<int>, ILocalizedEntity;

    Task<string> GetLocalizedValueEnumAsync<TEnum>(TEnum @enum, LanguageEnum language = LanguageEnum.VN) where TEnum : struct, IConvertible;
}

