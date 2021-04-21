using BlazorHero.CleanArchitecture.Utils.Enums;

namespace BlazorHero.CleanArchitecture.Utils
{
    public interface IUploadRequest
    {
        public string FileName { get; set; }
        public string Extension { get; set; }
        public UploadType UploadType { get; set; }
        public byte[] Data { get; set; }
    }
}
