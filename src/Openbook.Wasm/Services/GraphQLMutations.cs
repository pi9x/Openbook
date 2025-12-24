using System;

namespace Openbook.Wasm.Services
{
    /// <summary>
    /// Centralized GraphQL mutations used by the Wasm client.
    /// Keep mutation strings here so pages/components can reference them and keep mutations consistent.
    /// </summary>
    public static class GraphQLMutations
    {
        // Reusable selection sets (duplicate of query file for independence)
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

        // Authors
        public static readonly string CreateAuthor = @"
mutation CreateAuthor($name: String, $bio: String) {
  createAuthor(name: $name, bio: $bio) {
    " + AuthorFields + @"
  }
}
";

        public static readonly string UpdateAuthor = @"
mutation UpdateAuthor($id: String!, $name: String, $bio: String) {
  updateAuthor(id: $id, name: $name, bio: $bio) {
    " + AuthorFields + @"
  }
}
";

        public static readonly string DeleteAuthor = @"
mutation DeleteAuthor($id: String!) {
  deleteAuthor(id: $id)
}
";

        // Books
        // Note: CreateBook uses required title and authorId in many implementations; adjust types to match your server if needed.
        public static readonly string CreateBook = @"
mutation CreateBook($title: String!, $authorId: String!) {
  createBook(title: $title, authorId: $authorId) {
    " + BookFields + @"
  }
}
";

        // UpdateBook currently sends id and title. If server accepts authorId for updates, extend variables and call accordingly.
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

        // Reviews
        public static readonly string CreateReview = @"
mutation CreateReview($bookId: String!, $rating: Int!, $content: String!) {
  createReview(bookId: $bookId, rating: $rating, content: $content) {
    " + ReviewFields + @"
  }
}
";

        public static readonly string UpdateReview = @"
mutation UpdateReview($id: String!, $rating: Int, $content: String) {
  updateReview(id: $id, rating: $rating, content: $content) {
    " + ReviewFields + @"
  }
}
";

        public static readonly string DeleteReview = @"
mutation DeleteReview($id: String!) {
  deleteReview(id: $id)
}
";
    }
}
