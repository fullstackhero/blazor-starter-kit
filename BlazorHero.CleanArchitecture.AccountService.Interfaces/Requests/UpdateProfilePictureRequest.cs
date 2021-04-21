using BlazorHero.CleanArchitecture.Utils;
using BlazorHero.CleanArchitecture.Utils.Enums;

namespace BlazorHero.CleanArchitecture.AccountService.Interfaces.Requests
{
    public class UpdateProfilePictureRequest : IUploadRequest
    {
        public string FileName { get; set; }
        public string Extension { get; set; }
        public UploadType UploadType { get; set; }
        public byte[] Data { get; set; }
    }
}