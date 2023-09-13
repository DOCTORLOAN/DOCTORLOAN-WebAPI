namespace DoctorLoan.Domain.Const.Medias;
public class MediaConst
{
    /// <summary>
    /// {0} is media id
    /// {1} is slug
    /// </summary>
    public const string ImageURL = "/api/Media/{0}/{1}";


    /// <summary>
    /// {0} is media id
    /// </summary>
    public const string MediaNoSlugURL = "/api/Media/{0}";

    public const string NoImageURL = "/api/Media/no-image";
}
