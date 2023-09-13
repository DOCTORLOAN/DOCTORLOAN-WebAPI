using DoctorLoan.Domain.Enums.Commons;
using DoctorLoan.Domain.Enums.Contents;

namespace DoctorLoan.Contents.Application.Dtos;
public class ContentDtos
{
    public int Id { get; set; }

    private string _code;
    public string Code
    {
        get
        {
            return string.IsNullOrEmpty(_code) ? string.Empty : _code.Trim();
        }
        set { _code = value; }
    }

    private string _name;
    public string Name
    {
        get
        {
            return string.IsNullOrEmpty(_name) ? string.Empty : _name.Trim();
        }
        set { _name = value; }
    }

    private string _description;
    public string Description
    {
        get
        {
            return string.IsNullOrEmpty(_description) ? string.Empty : _description.Trim();
        }
        set { _description = value; }
    }
    public StatusEnum Status { get; set; }
    public ContentTypeEnum Type { get; set; }
    public long? MediaId { get; set; }
}
