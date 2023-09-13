using Newtonsoft.Json;

namespace DoctorLoan.Application.Models.Commons;
public class SelectListItemModel
{
    public string Text { get; set; }
    public string Value { get; set; }
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public object CustomData { get; set; }
}
