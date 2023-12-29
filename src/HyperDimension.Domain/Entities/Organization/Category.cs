using HyperDimension.Domain.Entities.Common;
using HyperDimension.Domain.Entities.Content;

namespace HyperDimension.Domain.Entities.Organization;

public class Category : BaseEntity
{
    public string Name { get; set; } = string.Empty;

    public string Slug { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string CoverImage { get; set; } = string.Empty;

    public string Icon { get; set; } = string.Empty;

    public Category? Parent { get; set; }

    public List<Category> Children { get; set; } = new List<Category>();

    public List<Article> Articles { get; set; } = new List<Article>();
}
