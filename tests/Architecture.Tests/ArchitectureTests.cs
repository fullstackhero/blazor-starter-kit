using CleanBlazor.Application.Abstractions.Messaging;
using CleanBlazor.Domain.Abstractions;
using FluentAssertions;
using MediatR;
using NetArchTest.Rules;
using Domain = CleanBlazor.Domain;
using Application = CleanBlazor.Application;
using Infrastructure = CleanBlazor.Infrastructure;
using Shared = CleanBlazor.Shared;
using Server = CleanBlazor.Server;
using Client = CleanBlazor.Client;

namespace Architecture.Tests;

public class ArchitectureTests
{
    private const string NamespacePrefix = "CleanBlazor";
    private const string DomainNamespace = $"{NamespacePrefix}.Domain";
    private const string ApplicationNamespace = $"{NamespacePrefix}.Application";
    private const string ApplicationAbstractionsNamespace = $"{NamespacePrefix}.Application.Abstractions";
    private const string InfrastructureNamespace = $"{NamespacePrefix}.Infrastucture";
    private const string ServerNamespace = $"{NamespacePrefix}.Server";
    private const string ClientNamespace = $"{NamespacePrefix}.Client";

    [Fact]
    public void Domain_Should_Not_HaveDependencyOnOtherProjects()
    {
        // Arrange
        var assembly = typeof(Domain.AssemblyReference).Assembly;

        var otherProjects = new[]
        {
            ApplicationNamespace,
            ApplicationAbstractionsNamespace,
            InfrastructureNamespace,
            ServerNamespace,
            ClientNamespace
        };

        // Act
        var result = Types
            .InAssembly(assembly)
            .Should()
            .NotHaveDependencyOnAny(otherProjects)
            .GetResult();

        // Assert
        result.IsSuccessful.Should().BeTrue($"{GetFailingTypes(result)}");
    }

    [Fact]
    public void Shared_Should_Not_HaveDependencyOnOtherProjects()
    {
        // Arrange
        var assembly = typeof(Shared.AssemblyReference).Assembly;

        var otherProjects = new[]
        {
            // DomainNamespace,
            ApplicationNamespace,
            ApplicationAbstractionsNamespace,
            InfrastructureNamespace,
            ServerNamespace,
            ClientNamespace
        };

        // Act
        var result = Types
            .InAssembly(assembly)
            .Should()
            .NotHaveDependencyOnAny(otherProjects)
            .GetResult();

        // Assert
        result.IsSuccessful.Should().BeTrue($"{GetFailingTypes(result)}");
    }

    [Fact]
    public void Application_Should_Not_HaveDependencyOnOtherProjects()
    {
        // Arrange
        var assembly = typeof(Application.AssemblyReference).Assembly;

        var otherProjects = new[]
        {
            InfrastructureNamespace,
            ServerNamespace,
            ClientNamespace
        };

        // Act
        var result = Types
            .InAssembly(assembly)
            .Should()
            .NotHaveDependencyOnAny(otherProjects)
            .GetResult();

        // Assert
        result.IsSuccessful.Should().BeTrue($"{GetFailingTypes(result)}");
    }

    [Fact]
    public void Infrastructure_Should_Not_HaveDependencyOnOtherProjects()
    {
        // Arrange
        var assembly = typeof(Infrastructure.AssemblyReference).Assembly;

        var otherProjects = new[]
        {
            ServerNamespace,
            ClientNamespace
        };

        // Act
        var result = Types
            .InAssembly(assembly)
            .Should()
            .NotHaveDependencyOnAny(otherProjects)
            .GetResult();

        // Assert
        result.IsSuccessful.Should().BeTrue($"{GetFailingTypes(result)}");
    }

    [Fact]
    public void Client_Should_Not_HaveDependencyOnOtherProjects()
    {
        // Arrange
        var assembly = typeof(Client.AssemblyReference).Assembly;

        var otherProjects = new[]
        {
            ApplicationNamespace,
            ApplicationAbstractionsNamespace,
            InfrastructureNamespace,
            ServerNamespace
        };

        // Act
        var result = Types
            .InAssembly(assembly)
            .Should()
            .NotHaveDependencyOnAny(otherProjects)
            .GetResult();

        // Assert
        result.IsSuccessful.Should().BeTrue($"{GetFailingTypes(result)}");
    }

    [Fact]
    public void Requests_Should_Not_HaveDependencyOnDataAnnotations()
    {
        // Arrange
        var assembly = typeof(Application.AssemblyReference).Assembly;
        const string dataAnnotationsNameSpace = "System.ComponentModel.DataAnnotations";

        var result = Types
            .InAssembly(assembly)
            .That()
            .ImplementInterface(typeof(IRequest<>))
            .Should()
            .NotHaveDependencyOn(dataAnnotationsNameSpace)
            .GetResult();

        // Assert
        result.IsSuccessful.Should().BeTrue($"FluentValidation should be used instead of DataAnnotations. {GetFailingTypes(result)}");
    }

    [Fact]
    public void Entities_Should_Not_HaveDependencyOnDataAnnotations()
    {
        // Arrange
        var domainAssembly = typeof(Domain.AssemblyReference).Assembly;
        var infrastructureAssembly = typeof(Infrastructure.AssemblyReference).Assembly;
        const string dataAnnotationsNameSpace = "System.ComponentModel.DataAnnotations";

        var result = Types
            .InAssemblies(new[] { domainAssembly, infrastructureAssembly })
            .That()
            .ImplementInterface(typeof(IEntity))
            .Should()
            .NotHaveDependencyOn(dataAnnotationsNameSpace)
            .GetResult();

        // Assert
        result.IsSuccessful.Should().BeTrue($"Fluent API configuration should be used instead of DataAnnotations. {GetFailingTypes(result)}");
    }

    [Fact]
    public void Requests_Should_BeSealed()
    {
        // Arrange
        var assembly = typeof(Application.AssemblyReference).Assembly;

        var result = Types
            .InAssembly(assembly)
            .That()
            .ImplementInterface(typeof(IRequest<>))
            .Should()
            .BeSealed()
            .GetResult();

        // Assert
        result.IsSuccessful.Should().BeTrue($"IRequests should be sealed. {GetFailingTypes(result)}");
    }

    [Fact]
    public void Requests_Should_ImplementIQueryOrICommand()
    {
        // Arrange
        var assembly = typeof(Application.AssemblyReference).Assembly;

        var result = Types
            .InAssembly(assembly)
            .That()
            .ImplementInterface(typeof(IRequest<>))
            .Should()
            .ImplementInterface(typeof(IQuery<>))
            .Or()
            .ImplementInterface(typeof(ICommand<>))
            .GetResult();

        // Assert
        result.IsSuccessful.Should().BeTrue($"IRequests should implement IQuery or ICommand. Failing types: {GetFailingTypes(result)}");
    }

    [Fact]
    public void Controllers_Should_HaveDependencyOnMediatR()
    {
        // Arrange
        var assembly = typeof(Server.AssemblyReference).Assembly;
        const string controllersNamespace = $"{ServerNamespace}.Controllers.v1";

        // TODO: Fix controllers that don't have dependency on MediatR
        // Act
        var result = Types
            .InAssembly(assembly)
            .That()
            .HaveNameEndingWith("Controller")
            .And()
            .DoNotResideInNamespaceStartingWith($"{controllersNamespace}.Utilities")
            .And()
            .DoNotResideInNamespaceStartingWith($"{controllersNamespace}.Identity")
            .Should()
            .HaveDependencyOn("MediatR")
            .GetResult();

        // Assert
        result.IsSuccessful.Should().BeTrue($"All controllers should be slim -- use MediatR to handle requests. {GetFailingTypes(result)}");
    }

    private static string GetFailingTypes(TestResult? result)
    {
        if (result == null || result.IsSuccessful || result.FailingTypeNames == null)
        {
            return string.Empty;
        }

        return $"Failing types:\n\t {string.Join("\n\t", result.FailingTypeNames)}\n";
    }
}
