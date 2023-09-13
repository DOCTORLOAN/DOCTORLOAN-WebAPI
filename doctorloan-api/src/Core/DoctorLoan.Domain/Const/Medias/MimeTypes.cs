namespace DoctorLoan.Domain.Const.Medias;
public class MimeTypes
{
    #region Application
    public const string ApplicationForceDownload = "application/force-download";

    public const string ApplicationJson = "application/json";

    public const string ApplicationOctetStream = "application/octet-stream";

    public const string ApplicationPdf = "application/pdf";

    public const string ApplicationRssXml = "application/rss+xml";

    public const string ApplicationXWwwFormUrlencoded = "application/x-www-form-urlencoded";

    public const string ApplicationXZipCo = "application/x-zip-co";
    #endregion

    #region Image
    public const string ImageBmp = "image/bmp";

    public const string ImageGif = "image/gif";

    public const string ImageJpeg = "image/jpeg";

    public const string ImagePJpeg = "image/pjpeg";

    public const string ImagePng = "image/png";

    public const string ImageTiff = "image/tiff";

    public const string ImageSVG = "image/svg+xml";
    #endregion

    #region Text
    public const string TextCss = "text/css";

    public const string TextCsv = "text/csv";

    public const string TextJavascript = "text/javascript";

    public const string TextPlain = "text/plain";

    public const string TextXlsx = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
    #endregion

    #region Video
    public const string Mp4 = ".mp4";
    #endregion

    public static List<string> MimeImage => new() { ImageBmp, ImageGif, ImageJpeg, ImagePJpeg, ImagePng, ImageTiff, ImageSVG };
}
