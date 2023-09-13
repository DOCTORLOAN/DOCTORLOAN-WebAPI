using NSwag.Annotations;

namespace DoctorLoan.Application.Models.Commons;
public class QueryParam
{
    public QueryParam()
    {
        Page = 1;
        Take = 10;
        SortBy = string.Empty;
    }
    private int _page;
    public int Page
    {
        get
        {
            if (_page <= 0) _page = 1;
            return _page;
        }
        set { _page = value; }
    }


    private int _take;
    public int Take
    {
        get
        {
            if (_take <= 0) _take = 10;
            return _take;
        }
        set { _take = value; }
    }

    private string _sortBy; // _sort
    public string SortBy
    {
        get { return _sortBy?.Trim().ToLower(); }
        set { _sortBy = value; }
    }

    public bool SortAsc { get; set; }

    [SwaggerIgnore]
    public (int page, int take, string sort, bool asc) Params => (
        Page,
        Take,
        SortBy?.Trim()?.ToLower() ?? string.Empty,
        SortAsc);
}
