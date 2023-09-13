using DoctorLoan.Domain.Interfaces;

namespace DoctorLoan.Application.Interfaces.Settings;

public interface ISettingBaseService
{
    Task<ISettings> LoadSettingAsync(Type type, int systemId = 0);

    Task<T> GetSettingByKeyAsync<T>(string key, T defaultValue = default, int systemId = 0, bool loadSharedValueIfNotFound = false);

    Task SaveSettingAsync<T>(T settings, int systemId = 0) where T : ISettings, new();
}
