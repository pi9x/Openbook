using HotChocolate.Execution.Configuration;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Openbook.Domain;
using Openbook.Domain.Aggregates.Authors;
using Openbook.Domain.Aggregates.Books;
using Openbook.Domain.Aggregates.Reviews;
using Openbook.GraphQL.Authors;
using Openbook.GraphQL.Books;
using Openbook.GraphQL.Reviews;

namespace Openbook.GraphQL;

public static class ServiceRegistration
{
    public static void AddGraphQLConfiguration(this IRequestExecutorBuilder builder)
    {
        builder
            .RegisterDbContextFactory<ApplicationDbContext>()
            .AddFiltering()
            .AddSorting()
            .AddProjections()
            .AddQueryType(d => d.Name("Query"))
            .AddMutationType(d => d.Name("Mutation"));
        
        builder
            .BindRuntimeType<AuthorId, StringType>()
            .AddTypeConverter<string, AuthorId>(value => new AuthorId(Guid.Parse(value)))
            .AddTypeConverter<AuthorId, string>(id => id.Value.ToString())
            .AddTypeExtension<AuthorQueries>()
            .AddTypeExtension<AuthorMutations>();
        
        builder
            .BindRuntimeType<BookId, StringType>()
            .AddTypeConverter<string, BookId>(value => new BookId(Guid.Parse(value)))
            .AddTypeConverter<BookId, string>(id => id.Value.ToString())
            .AddTypeExtension<BookQueries>()
            .AddTypeExtension<BookMutations>();
        
        builder
            .BindRuntimeType<ReviewId, StringType>()
            .AddTypeConverter<string, ReviewId>(value => new ReviewId(Guid.Parse(value)))
            .AddTypeConverter<ReviewId, string>(id => id.Value.ToString())
            .AddTypeExtension<ReviewQueries>()
            .AddTypeExtension<ReviewMutations>();
    }
}