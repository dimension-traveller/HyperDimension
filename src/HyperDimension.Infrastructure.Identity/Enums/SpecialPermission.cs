using System.Diagnostics.CodeAnalysis;

namespace HyperDimension.Infrastructure.Identity.Enums;

[SuppressMessage("Naming", "CA1711:Identifiers should not have incorrect suffix")]
public enum SpecialPermission
{
    /// <summary>
    ///     Do not require any permission. The same as if the attribute is not applied.
    /// </summary>
    Anonymous,

    /// <summary>
    ///     The user must be authenticated. No permission is required.
    /// </summary>
    Authenticated,

    /// <summary>
    ///     The user must be authenticated and granted at least one permission.
    /// </summary>
    Any
}
