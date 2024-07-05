namespace DotNet8.PostgreSqlWithDapperSample.Queries
{
    public class BlogQuery
    {
        public static string GetAllBlogsQuery { get; } = @"SELECT ""BlogId"", ""BlogTitle"", ""BlogAuthor"", ""BlogContent""
FROM public.""Tbl_Blog"" ORDER BY ""BlogId"" DESC;";

        public static string CreateBlogQuery { get; } = @"INSERT INTO public.""Tbl_Blog""(
	""BlogTitle"", ""BlogAuthor"", ""BlogContent"")
	VALUES (@BlogTitle, @BlogAuthor, @BlogContent);";

        public static string PutBlogQuery { get; } = @"UPDATE public.""Tbl_Blog""
                SET ""BlogTitle"" = @BlogTitle, ""BlogAuthor"" = @BlogAuthor, ""BlogContent"" = @BlogContent
                WHERE ""BlogId"" = @BlogId;";

        public static string DeleteBlogQuery { get; } = @"DELETE FROM public.""Tbl_Blog""
	WHERE ""BlogId"" = @BlogId;";
    }
}
