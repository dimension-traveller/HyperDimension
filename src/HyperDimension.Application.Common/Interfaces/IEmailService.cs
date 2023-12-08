using HyperDimension.Common;
using HyperDimension.Domain.Abstract;

namespace HyperDimension.Application.Common.Interfaces;

public interface IEmailService
{
    public Task<Result<bool>> SendEmailAsync<T>(string to, string subjectKey, T model) where T : class, IEmailTemplate;
}
