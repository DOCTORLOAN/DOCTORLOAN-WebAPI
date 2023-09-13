using DoctorLoan.Application.Models.Commons;
using DoctorLoan.Contents.Application.Features.Contents.Admin.Dtos;
using DoctorLoan.Domain.Enums.Commons;
using DoctorLoan.Domain.Enums.Contents;
using MediatR;

namespace DoctorLoan.Contents.Application.Features.Contents.Admin.Queries;
public class FilterContentQuery : QueryParam, IRequest<Result<PaginatedList<ContentInfomationDto>>>
{
    private string _keyWord;
    public string KeyWord
    {
        get
        {
            return string.IsNullOrEmpty(_keyWord) ? string.Empty : _keyWord.ToLower().Trim();
        }
        set { _keyWord = value; }
    }

    private string _code;
    public string Code
    {
        get
        {
            return string.IsNullOrEmpty(_code) ? string.Empty : _code.ToLower().Trim();
        }
        set { _code = value; }
    }

    private string _name;
    public string Name
    {
        get
        {
            return string.IsNullOrEmpty(_name) ? string.Empty : _name.ToLower().Trim();
        }
        set { _name = value; }
    }

    public ContentTypeEnum? Type { get; set; }
    public StatusEnum? Status { get; set; }
}
