using HyperDimension.Domain.Abstract;
using HyperDimension.Domain.Entities.Content;

namespace HyperDimension.Domain.Search;

public class PostDocument : SearchableDocument<Post>
{
    public string Title { get; set; } = string.Empty;

    public string Content { get; set; } = string.Empty;

    public string Summary { get; set; } = string.Empty;
}
