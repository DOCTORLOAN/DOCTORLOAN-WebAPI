namespace DoctorLoan.Application.Models.Medias;

public class BlobDto
{
    public string Uri { get; set; }
    public string OriginalName { get; set; }
    public string Name { get; set; }
    public string ContentType { get; set; }
    public string Extention { get; set; }
    public Stream Content { get; set; }
    public byte[] Data { get; set; }
}
