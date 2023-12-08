using System.Reflection;

namespace HyperDimension.Domain.Resources;

public static class DomainAssemblyReference
{
    public static Assembly Assembly => typeof(DomainAssemblyReference).Assembly;
}
