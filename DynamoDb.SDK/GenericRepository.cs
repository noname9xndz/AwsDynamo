using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using DynamoDb.SDK.Configurations;
using DynamoDb.SDK.Models;

namespace DynamoDb.SDK
{
    public class GenericRepository<T, K> : DynamoDBContext, IGenericRepository<T, K> where T : DomainEntity<K>
    {
        private readonly DynamoDBContext _context;

        public GenericRepository(IAmazonDynamoDB client) : base(client)
        {
            _context = new DynamoDBContext(client);
        }


        #region Add Update Delete

        public async Task<ResponseApiModel<bool>> AddAsync(T entity)
        {
            ResponseApiModel<bool> responseApi = new ResponseApiModel<bool>();
            try
            {
                await _context.SaveAsync<T>(entity);
                responseApi.Result = true;
                responseApi.Status = true;
                return responseApi;
            }
            catch (Exception e)
            {
                responseApi.ErrorMessage.Add(e.Message);
                if (e.InnerException != null)
                {
                    responseApi.ErrorMessage.Add(e.InnerException?.Message);
                }
            }

            return responseApi;
        }

        public async Task<ResponseApiModel<T>> BulkInsertAsync(IEnumerable<T> entities)
        {
            ResponseApiModel<T> response = new ResponseApiModel<T>();
            var domainEntities = entities.ToList();
            response.ResultList = domainEntities;
            try
            {
                var batchWrite = _context.CreateBatchWrite<T>();
                batchWrite.AddPutItems(domainEntities);
                await batchWrite.ExecuteAsync();
                response.Status = true;
            }
            catch (Exception e)
            {
                response.ErrorMessage.Add(e.Message);
                if (e.InnerException != null)
                {
                    response.ErrorMessage.Add(e.InnerException?.Message);
                }
            }
            return response;
        }

        public async Task<ResponseApiModel<bool>> UpdateAsync(K id, T entity)
        {
            ResponseApiModel<bool> responseApi = new ResponseApiModel<bool>();
            try
            {
                entity.DateModified = DateTime.UtcNow;
                await _context.SaveAsync<T>(entity);
                responseApi.Result = true;
                responseApi.Status = true;
            }
            catch (Exception e)
            {
                responseApi.ErrorMessage.Add(e.Message);
                if (e.InnerException != null)
                {
                    responseApi.ErrorMessage.Add(e.InnerException?.Message);
                }
                responseApi.Status = false;
            }
            return responseApi;
        }

        public async Task<ResponseApiModel<K>> BulkDeleteByKeyAsync(List<K> ids)
        {
            ResponseApiModel<K> response = new ResponseApiModel<K>();
            response.ResultList = ids;
            try
            {
                var batchWrite = _context.CreateBatchWrite<T>();
                batchWrite.AddDeleteKey(ids);
                await batchWrite.ExecuteAsync();
                response.Status = true;
            }
            catch (Exception e)
            {
                response.ErrorMessage.Add(e.Message);
                if (e.InnerException != null)
                {
                    response.ErrorMessage.Add(e.InnerException?.Message);
                }
            }
            return response;
        }

        public async Task<ResponseApiModel<T>> BulkDeleteAsync(IEnumerable<T> entities)
        {
            ResponseApiModel<T> response = new ResponseApiModel<T>();
            var domainEntities = entities.ToList();
            response.ResultList = domainEntities;
            try
            {
                var batchWrite = _context.CreateBatchWrite<T>();
                batchWrite.AddDeleteItems(domainEntities);
                await batchWrite.ExecuteAsync();
                response.Status = true;
            }
            catch (Exception e)
            {
                response.ErrorMessage.Add(e.Message);
                if (e.InnerException != null)
                {
                    response.ErrorMessage.Add(e.InnerException?.Message);
                }
                response.Status = false;
            }
            return response;
        }

        public async Task<ResponseApiModel<T>> BulkSoftDeleteAsync(IEnumerable<T> entities)
        {
            ResponseApiModel<T> response = new ResponseApiModel<T>();
            entities = entities.Select(x =>
            {
                x.StatusEntity = StatusEntity.InActive;
                x.DateDeleted = DateTime.UtcNow;
                return x;
            }).ToList();
            response.ResultList = entities;
            try
            {
                var batchWrite = _context.CreateBatchWrite<T>();
                batchWrite.AddDeleteItems(entities);
                await batchWrite.ExecuteAsync();
                response.Status = true;
            }
            catch (Exception e)
            {
                response.ErrorMessage.Add(e.Message);
                if (e.InnerException != null)
                {
                    response.ErrorMessage.Add(e.InnerException?.Message);
                }
                response.Status = false;
            }
            return response;
        }


        public async Task<ResponseApiModel<bool>> DeleteAsync(K id)
        {
            ResponseApiModel<bool> responseApi = new ResponseApiModel<bool>();
            try
            {
                await _context.DeleteAsync<T>(id);
                responseApi.Result = true;
                responseApi.Status = true;
                return responseApi;
            }
            catch (Exception e)
            {
                responseApi.ErrorMessage.Add(e.Message);
                if (e.InnerException != null)
                {
                    responseApi.ErrorMessage.Add(e.InnerException?.Message);
                }
                responseApi.Status = false;
            }

            return responseApi;
        }

        #endregion

        #region GET

        public async Task<ResponseApiModel<T>> GetAllDataTableAsync(ScanOperationConfig scanOps)
        {
            ResponseApiModel<T> responseApi = new ResponseApiModel<T>();
            var table = _context.GetTargetTable<T>();
            if(scanOps == null) scanOps = new ScanOperationConfig();
            var results = table.Scan(scanOps);
            List<Document> data = await results.GetNextSetAsync();
            if (data != null && data.Count > 0)
            {
                responseApi.ResultList = _context.FromDocuments<T>(data);
                responseApi.PaginationToken = results.PaginationToken;
            }
            return responseApi;
        }

       

        #endregion

        #region Dynamo

        public async Task<Table> GetCurrentTable()
        {
            return await Task.FromResult(_context.GetTargetTable<T>());
        }

        public async Task<DynamoDBContext> GetCurrentDbContext()
        {
            return await Task.FromResult(_context);
        }


        #endregion
    }
}