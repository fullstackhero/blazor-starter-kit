namespace CleanBlazor.Domain.Entities.Communication;

public interface IChatUser
{
    string FirstName { get; set; }

    string LastName { get; set; }

    string ProfilePictureDataUrl { get; set; }
}
