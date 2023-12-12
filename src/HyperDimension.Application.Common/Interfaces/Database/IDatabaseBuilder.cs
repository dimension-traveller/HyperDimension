using Microsoft.EntityFrameworkCore;

namespace HyperDimension.Application.Common.Interfaces.Database;

public interface IDatabaseBuilder
{
    public void Build(DbContextOptionsBuilder optionsBuilder);
}
