namespace Shop.Application.Helpers;

using Domain.Models.Common;
using Infrastructure.Data;

public interface ITransactionScopeService {
    Task<Response<T>> ExecuteAsync<T>(Func<Task<Response<T>>> action);
}

public class TransactionScopeService:ITransactionScopeService {
    private readonly IUnitOfWork _unitOfWork;

    public TransactionScopeService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Response<T>> ExecuteAsync<T>(Func<Task<Response<T>>> action)
    {
        await _unitOfWork.BeginTransactionAsync();
        try
        {
            var response = await action();

            if (response.Success) 
                await _unitOfWork.CommitTransactionAsync();
            else
                await _unitOfWork.RollbackAsync();

            return response;
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync();
            return new Response<T>(ex.Message);
        }
    }
}
