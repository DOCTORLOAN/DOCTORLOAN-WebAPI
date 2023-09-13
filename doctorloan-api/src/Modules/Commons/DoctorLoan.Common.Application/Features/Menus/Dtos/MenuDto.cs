using Newtonsoft.Json;

namespace DoctorLoan.Common.Application.Features.Menus.Dtos;
public class MenuDto
{
    public string Name { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public string IconName { get; set; } = string.Empty;

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public List<MenuDto> Childs { get; set; }
}
