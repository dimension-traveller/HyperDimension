namespace HyperDimension.Application.Common.Interfaces.Database;

public interface IDatabaseOptionsBuilder<in TBuilder>
{
    public void Build(TBuilder builder);
}
