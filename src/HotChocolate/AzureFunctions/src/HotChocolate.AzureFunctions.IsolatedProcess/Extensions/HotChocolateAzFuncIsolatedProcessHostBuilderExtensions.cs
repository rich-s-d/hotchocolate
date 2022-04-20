using HotChocolate.AzureFunctions;
using HotChocolate.Execution.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Microsoft.Azure.Functions.Extensions.DependencyInjection;

public static class HotChocolateAzFuncIsolatedProcessHostBuilderExtensions
{
    /// <summary>
    /// Adds a GraphQL server and Azure Functions integration services for Azure Functions Isolated processing model.
    /// </summary>
    /// <param name="hostBuilder">
    /// The <see cref="IFunctionsHostBuilder"/>.
    /// </param>
    /// <param name="graphqlConfigureFunc">
    /// The GraphQL Configuration function that will be invoked, for chained configuration, when the Host is built.
    /// </param>
    /// <param name="maxAllowedRequestSize">
    /// The max allowed GraphQL request size.
    /// </param>
    /// <param name="apiRoute">
    /// The API route that was used in the GraphQL Azure Function.
    /// </param>
    /// <returns>
    /// Returns the <see cref="IHostBuilder"/> so that host configuration can be chained.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// The <see cref="IServiceCollection"/> is <c>null</c>.
    /// </exception>
    public static IHostBuilder AddGraphQLFunction(
        this IHostBuilder hostBuilder,
        Action<IRequestExecutorBuilder> graphqlConfigureFunc,
        int maxAllowedRequestSize = GraphQLAzureFunctionsConstants.DefaultMaxRequests,
        string apiRoute = GraphQLAzureFunctionsConstants.DefaultGraphQLRoute
    )
    {
        if (hostBuilder is null)
            throw new ArgumentNullException(nameof(hostBuilder));

        if (graphqlConfigureFunc is null)
            throw new ArgumentNullException(nameof(graphqlConfigureFunc));

        hostBuilder.ConfigureServices(services =>
        {
            var executorBuilder = services.AddGraphQLFunction(maxAllowedRequestSize, apiRoute);
            graphqlConfigureFunc(executorBuilder);
        });

        return hostBuilder;
    }
}
