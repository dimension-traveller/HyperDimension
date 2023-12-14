namespace HyperDimension.Infrastructure.Identity.Exceptions;

public class EmptyExternalClaimsException(string schema, string valueType, string claimType)
    : Exception($"Failed to read {valueType} from claim type {claimType} using external authentication schema {schema}.");
