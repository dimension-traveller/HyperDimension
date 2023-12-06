using HyperDimension.Infrastructure.Identity.Abstract;
using Microsoft.AspNetCore.Authentication.Google;

namespace HyperDimension.Infrastructure.Identity.Options.Providers;

public class GoogleProviderOptions : OAuthProviderOptions, IIdentityProviderConfig
{
}
