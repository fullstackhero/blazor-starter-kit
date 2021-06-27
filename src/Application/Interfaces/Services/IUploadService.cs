using BlazorHero.CleanArchitecture.Application.Requests;

namespace BlazorHero.CleanArchitecture.Application.Interfaces.Services
{
    public interface IUploadService
    {
        string UploadAsync(UploadRequest request);
    }
}