using HyperDimension.Infrastructure.Identity.Abstract;
using Microsoft.AspNetCore.Authentication.MicrosoftAccount;

namespace HyperDimension.Infrastructure.Identity.Options.Providers;

public class MicrosoftProviderOptions : OAuthProviderOptions, IIdentityProviderConfig
{
}
