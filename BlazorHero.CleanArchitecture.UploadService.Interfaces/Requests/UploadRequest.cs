using BlazorHero.CleanArchitecture.Utils;
using BlazorHero.CleanArchitecture.Utils.Enums;

namespace BlazorHero.CleanArchitecture.UploadService.Interfaces.Requests
{
    public class UploadRequest : IUploadRequest
    {
        public string FileName { get; set; }
        public string Extension { get; set; }
        public UploadType UploadType { get; set; }
        public byte[] Data { get; set; }
    }
}