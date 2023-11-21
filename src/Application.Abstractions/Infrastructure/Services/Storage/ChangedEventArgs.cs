using System.Diagnostics.CodeAnalysis;

namespace CleanBlazor.Application.Abstractions.Infrastructure.Services.Storage;

[ExcludeFromCodeCoverage]
public class ChangedEventArgs
{
    public string Key { get; set; }
    public object OldValue { get; set; }
    public object NewValue { get; set; }
}
