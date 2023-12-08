using HyperDimension.Application.Common.Interfaces;
using HyperDimension.Common;
using HyperDimension.Domain.Abstract;

namespace HyperDimension.Infrastructure.Common.Services;

public class DummyEmailService : IEmailService
{
    public Task<Result<bool>> SendEmailAsync<T>(string to, string subjectKey, T model) where T : class, IEmailTemplate
    {
        return Task.FromResult(Result<bool>.Success(true));
    }
}
