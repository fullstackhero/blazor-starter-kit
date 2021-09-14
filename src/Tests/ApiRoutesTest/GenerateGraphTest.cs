using BlazorHero.CleanArchitecture.Server;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Routing.Internal;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using Xunit;
using Xunit.Abstractions;

namespace ApiRoutesTest
{
    public class GenerateGraphTest
        : IClassFixture<WebApplicationFactory<Startup>>
    {
        // Inject the factory and the output helper
        private readonly WebApplicationFactory<Startup> _factory;
        private readonly ITestOutputHelper _output;

        public GenerateGraphTest(
        WebApplicationFactory<Startup> factory, ITestOutputHelper output)
        {
            _factory = factory;
            _output = output;
        }

        [Fact]
        public void GenerateGraph()
        {
            // fetch the required services from the root container of the app
            var graphWriter = _factory.Services.GetRequiredService<DfaGraphWriter>();
            var endpointData = _factory.Services.GetRequiredService<EndpointDataSource>();

            Directory.CreateDirectory("Files");

            // build the graph
            using (var sw = new StringWriter())
            {
                graphWriter.Write(endpointData, sw);
                var graph = sw.ToString();

                // write the graph to the test output
                _output.WriteLine(graph);
            }
        }
    }
}
