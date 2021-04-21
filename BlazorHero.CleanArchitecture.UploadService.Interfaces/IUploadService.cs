using BlazorHero.CleanArchitecture.Utils;

namespace BlazorHero.CleanArchitecture.UploadService.Interfaces
{
    public interface IUploadService
    {
        string UploadAsync(IUploadRequest request);
    }
}