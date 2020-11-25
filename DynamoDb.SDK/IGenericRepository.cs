using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using DynamoDb.SDK.Configurations;
using DynamoDb.SDK.Models;

namespace DynamoDb.SDK
{
    public interface IGenericRepository<T, K> where T : DomainEntity<K>
    {
        Task<ResponseApiModel<bool>> AddAsync(T entity);

        Task<ResponseApiModel<bool>> DeleteAsync(K id);


        Task<ResponseApiModel<bool>> UpdateAsync(K id, T entity);

        Task<ResponseApiModel<T>> BulkDeleteAsync(IEnumerable<T> entities);

        Task<ResponseApiModel<T>> BulkSoftDeleteAsync(IEnumerable<T> entities);

        Task<ResponseApiModel<K>> BulkDeleteByKeyAsync(List<K> ids);

        Task<ResponseApiModel<T>> BulkInsertAsync(IEnumerable<T> entities);

        Task<ResponseApiModel<T>> GetAllDataTableAsync(ScanOperationConfig scanOps);

        Task<Table> GetCurrentTable();

        Task<DynamoDBContext> GetCurrentDbContext();
    }
}