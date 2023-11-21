using CleanBlazor.Contracts;

namespace CleanBlazor.Application.Abstractions.Infrastructure.Services;

public interface IUploadService
{
    string UploadAsync(UploadRequest request);
}
