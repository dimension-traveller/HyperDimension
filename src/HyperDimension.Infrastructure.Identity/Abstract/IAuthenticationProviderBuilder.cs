using Microsoft.AspNetCore.Authentication;

namespace HyperDimension.Infrastructure.Identity.Abstract;

public interface IAuthenticationProviderBuilder
{
    public bool CanAddSchema { get; }

    public void AddSchema(AuthenticationBuilder builder, string id, string name, object options);
}
