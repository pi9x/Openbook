using System;

namespace Openbook.Wasm.Services
{
    /// <summary>
    /// Centralized GraphQL queries and mutations used by the Wasm client.
    /// Place all GraphQL text here so pages/components can reference them and keep queries consistent.
    /// </summary>
    public static class GraphQLQueries
    {
        // Reusable selection sets
        public static readonly string AuthorFields = @"
            authorId
            name
            bio
        ";

        public static readonly string ReviewFields = @"
            reviewId
            bookId
            rating
            content
        ";

        public static readonly string BookFields = @"
            bookId
            title
            author {
                " + AuthorFields + @"
            }
            reviews {
                " + ReviewFields + @"
            }
        ";

        // Queries
        public static readonly string Authors = @"
query Authors {
    authors {
        nodes {
            " + AuthorFields + @"
        }
    }
}
";

        public static readonly string AuthorById = @"
query AuthorById($authorId: String!) {
    authorById(authorId: $authorId) {
        " + AuthorFields + @"
    }
}
";

        public static readonly string Books = @"
query Books {
    books {
        nodes {
            " + BookFields + @"
        }
    }
}
";

        public static readonly string BookById = @"
query BookById($id: String!) {
    bookById(id: $id) {
        " + BookFields + @"
    }
}
";

        public static readonly string Reviews = @"
query Reviews {
    reviews {
        nodes {
            " + ReviewFields + @"
        }
    }
}
";

        public static readonly string ReviewById = @"
query ReviewById($id: String!) {
    reviewById(id: $id) {
        " + ReviewFields + @"
    }
}
";

        // Mutations
        public static readonly string CreateBook = @"
mutation CreateBook($title: String!, $authorId: String!) {
  createBook(title: $title, authorId: $authorId) {
    " + BookFields + @"
  }
}
";

        // Note: the server implementation may accept only title (and not authorId) for update.
        // This mutation sends id and title. If the server accepts author updates, you can
        // extend this mutation to include an $authorId variable and pass it through.
        public static readonly string UpdateBook = @"
mutation UpdateBook($id: String!, $title: String) {
  updateBook(id: $id, title: $title) {
    " + BookFields + @"
  }
}
";

        public static readonly string DeleteBook = @"
mutation DeleteBook($id: String!) {
  deleteBook(id: $id)
}
";

        // Optionally expose combined schema query (the user provided a full schema query).
        // Useful for fetching many things at once.
        public static readonly string FullQuerySchema = @"
query FullQuerySchema(
    $authorById: ID,
    $bookById: ID,
    $reviewById: ID
) {
    authors {
        nodes {
            " + AuthorFields + @"
        }
    }
    authorById(authorId: $authorById) {
        " + AuthorFields + @"
    }
    books {
        nodes {
            " + BookFields + @"
        }
    }
    bookById(id: $bookById) {
        " + BookFields + @"
    }
    reviews {
        nodes {
            " + ReviewFields + @"
        }
    }
    reviewById(id: $reviewById) {
        " + ReviewFields + @"
    }
}
";
    }
}
