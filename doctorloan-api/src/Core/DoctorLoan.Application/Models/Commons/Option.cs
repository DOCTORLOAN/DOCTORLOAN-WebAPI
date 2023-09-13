using DoctorLoan.Application.Common.Helpers;
using DoctorLoan.Domain.Entities.Contents;
using DoctorLoan.Domain.Entities.Roles;
using DoctorLoan.Domain.Enums.Contents;
using Newtonsoft.Json;

namespace DoctorLoan.Application.Models.Commons;
public class Option
{
    public Option() { }

    public object Value { get; set; }

    public string Label { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string Other { get; set; }


    public Option(Role data)
    {
        Value = data.Id;
        Label = data.Name;
    }

    public Option(Content content)
    {
        Value = content.Id;
        Label = $"[{content.Code}] {content.Name}";
        if (content.Type == ContentTypeEnum.Video)
        {
            if (content.Media != null && !content.Media.HasStorage)
                Other = content.Media.Path;
        }
        else if (content.Type == ContentTypeEnum.PDF)
        {
            Other = CommonHelper.GetMediaUrl(content.MediaId, isImage: false, includeHostName: true);
        }
        else if (content.Type == ContentTypeEnum.Assessment)
        {
            Other = CommonHelper.GetMediaUrl(content.MediaId, isImage: false);
        }
    }
}
