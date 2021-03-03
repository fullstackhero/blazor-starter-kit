namespace BlazorHero.CleanArchitecture.Application.Wrapper
{
    public interface IResult
    {
        string Message { get; set; }

        bool Succeeded { get; set; }
    }

    public interface IResult<out T> : IResult
    {
        T Data { get; }
    }
}