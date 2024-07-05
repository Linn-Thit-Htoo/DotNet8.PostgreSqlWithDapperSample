using Dapper;
using DotNet8.PostgreSqlWithDapperSample.Enums;
using DotNet8.PostgreSqlWithDapperSample.Queries;
using DotNet8.PostgreSqlWithDapperSample.Services;
using Npgsql;

namespace DotNet8.PostgreSqlWithDapperSample.Features.Blog
{
    public class DA_Blog
    {
        private readonly DapperService _dapperService;

        public DA_Blog(DapperService dapperService)
        {
            _dapperService = dapperService;
        }

        public async Task<Result<BlogListResponseModel>> GetBlogs()
        {
            Result<BlogListResponseModel> responseModel;
            try
            {
                string query = BlogQuery.GetAllBlogsQuery;
                List<BlogModel> lst = await _dapperService.QueryAsync<BlogModel>(query);
                var model = new BlogListResponseModel(lst);

                responseModel = Result<BlogListResponseModel>.SuccessResult(model);
            }
            catch (Exception ex)
            {
                responseModel = Result<BlogListResponseModel>.FailureResult(ex);
            }

            return responseModel;
        }

        public async Task<Result<BlogResponseModel>> CreateBlog(BlogRequestModel requestModel)
        {
            Result<BlogResponseModel> responseModel;
            try
            {
                string query = BlogQuery.CreateBlogQuery;
                var parameters = new
                {
                    requestModel.BlogTitle,
                    requestModel.BlogAuthor,
                    requestModel.BlogContent
                };
                int result = await _dapperService.ExecuteAsync(query, parameters);

                responseModel = Result<BlogResponseModel>.ExecuteResult(result, successStatusCode: EnumStatusCode.Created);
            }
            catch (Exception ex)
            {
                responseModel = Result<BlogResponseModel>.FailureResult(ex);
            }

            return responseModel;
        }

        public async Task<Result<BlogResponseModel>> UpdateBlog(BlogRequestModel requestModel, int id)
        {
            Result<BlogResponseModel> responseModel;
            try
            {
                string query = BlogQuery.PutBlogQuery;
                var parameters = new
                {
                    BlogId = id,
                    requestModel.BlogTitle,
                    requestModel.BlogAuthor,
                    requestModel.BlogContent
                };
                int result = await _dapperService.ExecuteAsync(query, parameters);

                responseModel = Result<BlogResponseModel>.ExecuteResult(result, successStatusCode: EnumStatusCode.Accepted);
            }
            catch (Exception ex)
            {
                responseModel = Result<BlogResponseModel>.FailureResult(ex);
            }

            return responseModel;
        }

        public async Task<Result<BlogResponseModel>> PatchBlog(BlogRequestModel requestModel, int id)
        {
            Result<BlogResponseModel> responseModel;
            try
            {
                string conditions = string.Empty;
                conditions = GetConditions(conditions, requestModel);

                if (conditions.Length == 0)
                {
                    responseModel = Result<BlogResponseModel>.FailureResult("Invalid Request.");
                    goto result;
                }

                string query = GetPatchBlogQuery(conditions);
                var parameters = new DynamicParameters();
                parameters = GetPatchBlogParams(parameters, requestModel, id);

                int result = await _dapperService.ExecuteAsync(query, parameters);

                responseModel = Result<BlogResponseModel>.ExecuteResult(result, successStatusCode: EnumStatusCode.Accepted);
            }
            catch (Exception ex)
            {
                responseModel = Result<BlogResponseModel>.FailureResult(ex);
            }

        result:
            return responseModel;
        }

        public async Task<Result<BlogResponseModel>> DeleteBlog(int id)
        {
            Result<BlogResponseModel> responseModel;
            try
            {
                string query = BlogQuery.DeleteBlogQuery;
                int result = await _dapperService.ExecuteAsync(query, new { BlogId = id });

                responseModel = Result<BlogResponseModel>.ExecuteResult(result, successStatusCode: EnumStatusCode.Accepted);
            }
            catch (Exception ex)
            {
                responseModel = Result<BlogResponseModel>.FailureResult(ex);
            }

            return responseModel;
        }

        private string GetConditions(string conditions, BlogRequestModel requestModel)
        {
            if (!requestModel.BlogTitle!.IsNullOrEmpty())
            {
                conditions += @"""BlogTitle"" = @BlogTitle, ";
            }

            if (!requestModel.BlogAuthor!.IsNullOrEmpty())
            {
                conditions += @"""BlogAuthor"" = @BlogAuthor, ";
            }

            if (!requestModel.BlogContent!.IsNullOrEmpty())
            {
                conditions += @"""BlogContent"" = @BlogContent, ";
            }

            return conditions;
        }

        private DynamicParameters GetPatchBlogParams(DynamicParameters parameters, BlogRequestModel requestModel, int id)
        {
            parameters.Add("@BlogId", id);

            if (!requestModel.BlogTitle!.IsNullOrEmpty())
            {
                parameters.Add("@BlogTitle", requestModel.BlogTitle);
            }

            if (!requestModel.BlogAuthor!.IsNullOrEmpty())
            {
                parameters.Add("@BlogAuthor", requestModel.BlogAuthor);
            }

            if (!requestModel.BlogContent!.IsNullOrEmpty())
            {
                parameters.Add("@BlogContent", requestModel.BlogContent);
            }

            return parameters;
        }

        private string GetPatchBlogQuery(string conditions)
        {
            return $@"UPDATE public.""Tbl_Blog""
                SET {conditions.Substring(0, conditions.Length - 2)}
                WHERE ""BlogId"" = @BlogId;";
        }
    }
}
