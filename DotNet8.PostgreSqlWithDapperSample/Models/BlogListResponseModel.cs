namespace DotNet8.PostgreSqlWithDapperSample.Models;

public class BlogListResponseModel
{
    public List<BlogModel> DataLst { get; set; }

    public BlogListResponseModel(List<BlogModel> dataLst)
    {
        DataLst = dataLst;
    }
}
