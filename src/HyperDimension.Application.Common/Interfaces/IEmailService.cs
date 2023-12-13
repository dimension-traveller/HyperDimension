using HyperDimension.Common;
using HyperDimension.Domain.Abstract;
using HyperDimension.Domain.Entities.Identity;

namespace HyperDimension.Application.Common.Interfaces;

public interface IEmailService
{
    public Task<Result<bool>> SendEmailAsync<T>(string to, T model) where T : class, IEmailTemplate;
}
