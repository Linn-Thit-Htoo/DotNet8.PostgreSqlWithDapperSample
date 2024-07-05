using DotNet8.PostgreSqlWithDapperSample.Enums;
using DotNet8.PostgreSqlWithDapperSample.Models;
using DotNet8.PostgreSqlWithDapperSample.Queries;
using DotNet8.PostgreSqlWithDapperSample.Services;

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
    }
}
