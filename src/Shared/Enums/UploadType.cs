using System.ComponentModel;

namespace CleanBlazor.Shared.Enums;

public enum UploadType : byte
{
    [Description("Images/Products")] Product,

    [Description("Images/ProfilePictures")]
    ProfilePicture,

    [Description("Documents")] Document
}
