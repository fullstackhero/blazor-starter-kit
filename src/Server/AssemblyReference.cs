using System.Reflection;

namespace CleanBlazor.Server;

public static class AssemblyReference
{
    public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;
}
