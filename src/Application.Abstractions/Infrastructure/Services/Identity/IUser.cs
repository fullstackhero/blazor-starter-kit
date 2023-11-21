namespace CleanBlazor.Application.Abstractions.Infrastructure.Services.Identity;

public interface IUser
{
    string Id { get; }
    string UserName { get; }
    string ProfilePictureDataUrl { get; }
    string FirstName { get; }
    string LastName { get; }
    string Email { get; }
    bool IsActive { get; }
    bool EmailConfirmed { get; }
}
