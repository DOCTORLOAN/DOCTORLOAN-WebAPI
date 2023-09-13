namespace DoctorLoan.Application.Models.Settings;

public class StorageConfiguration
{
    public StoreType Type { get; set; }
    public ImageSizes ImageSizes { get; set; }
    public AzureBlobStoreConfig AzureStorageConfig { get; set; }


}
public class ImageSizes
{
    public int ExtraExtraSmall { get; set; }
    public int ExtraSmall { get; set; }
    public int Small { get; set; }
    public int Medium { get; set; }
    public int Large { get; set; }
}
public class AzureBlobStoreConfig
{
    public string ContainerName { get; set; }

    public string BucketName { get; set; }

    public string AzureConnectionString { get; set; }

    public long MaximumImageSize { get; set; }

    public string SystemContainer { get; set; }

    public string DefaultContainer { get; set; }

    public string ImagesContainer { get; set; }

    public string AvatarFolder { get; set; }

    public string IdentityFolder { get; set; }

    public string ProductFolder { get; set; }

    public string FilesContainer { get; set; }

    public string NoImageFileName { get; set; }

    public string LibraryFolder { get; set; }


    public string ResizeAvatarFolder { get; set; }

    public string ResizeProductFolder { get; set; }

    public string ResizeIdentityFolder { get; set; }
    public string ResizeLibraryFolder { get; set; }
}
public enum StoreType
{
    Physical
}
