using HyperDimension.Infrastructure.Identity.Options;
using Microsoft.AspNetCore.Authentication;

namespace HyperDimension.Infrastructure.Identity.Abstract;

public interface IAuthenticationProviderBuilder<in T> where T : IIdentityProviderConfig
{
    public bool CanAddSchema { get; }

    public void AddSchema(AuthenticationBuilder builder, IdentityProviderOptions metadata, T options);
}
