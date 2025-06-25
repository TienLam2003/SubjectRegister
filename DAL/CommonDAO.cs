using Dapper;
using Microsoft.Data.SqlClient;
using SubjectRegister.Common;
using SubjectRegister.Comon;
using SubjectRegister.Models;
using System.Data;

namespace SubjectRegister.DAL
{
    public class CommonDAO<TModel> where TModel : class
    {
        protected readonly IDbConnection _dbConnection;
        protected CommonDAO(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        /// <summary>
        /// Hàm cơ sở để lấy dữ liệu phân trang
        /// </summary>
        public GetDataPaginationModel<TModel> GetPagination(int pageActive, string sql)
        {
            int pageSize = CData.Mode.PageSize;
            // Tính toán offset dựa trên trang hiện tại
            int offset = (pageActive - 1) * pageSize;
            var multiQuery = _dbConnection.QueryMultiple(sql, new { Offset = offset, PageSize = pageSize });
            // Đọc tổng số bản ghi và tính toán tổng số trang
            int totalCount = multiQuery.ReadFirst<int>();

            var data = new GetDataPaginationModel<TModel>
            {
                ListData = multiQuery.Read<TModel>().ToList(),
                TotalPage = (int)Math.Ceiling(totalCount / (double)pageSize),
                PageActive = pageActive
            };
            return data;
        }

        /// <summary>
        /// Hàm cơ sở lấy dữ liệu phân trang theo tìm kiếm
        /// </summary>
        public GetDataPaginationModel<TModel> GetBySearch(int pageActive, string sql, object modelSearch)
        {
            int pageSize = CData.Mode.PageSize;
            // Tính toán offset dựa trên trang hiện tại
            int offset = (pageActive - 1) * pageSize;
            // Gom cụm object phân trang và object tìm kiếm để truyền vào truy vấn
            var parameters = new DynamicParameters();
            parameters.AddDynamicParams(modelSearch);
            parameters.AddDynamicParams(new { Offset = offset, PageSize = pageSize });

            var multiQuery = _dbConnection.QueryMultiple(sql, parameters);
            // Đọc tổng số bản ghi và tính toán tổng số trang
            int totalCount = multiQuery.ReadFirst<int>();

            var data = new GetDataPaginationModel<TModel>
            {
                ListData = multiQuery.Read<TModel>().ToList(),
                TotalPage = (int)Math.Ceiling(totalCount / (double)pageSize),
                PageActive = pageActive
            };
            return data;
        }

        public QueryResultModel Add(string sql, object param, Func<SqlException, string> errorHandler)
        {
            var queryResult = new QueryResultModel();
            try
            {
                _dbConnection.Execute(sql, param);
            }
            catch (SqlException ex)
            {
                queryResult.message = errorHandler(ex);
                queryResult.result = false;
            }
            return queryResult;
        }

        public QueryResultModel Add(string sql1, object param1, 
            string sql2, object param2, Func<SqlException, string> errorHandler)
        {
            var queryResult = new QueryResultModel();
            _dbConnection.Open();
            var transaction = _dbConnection.BeginTransaction();
            try
            {
                //Thêm trước
                var code = _dbConnection.QuerySingle<int>(sql1, param1, transaction);
                //Thêm sau
                _dbConnection.Execute(sql2, param2, transaction);

                transaction.Commit();
            }
            catch (SqlException ex)
            {
                transaction.Rollback();
                queryResult.message = SqlErrorHandler.GetErrorMessage(ex);
                queryResult.result = false;
            }
            return queryResult;
        }

        public QueryResultModel Update(string sql, object param)
        {
            var queryResult = new QueryResultModel();
            try
            {
                _dbConnection.Execute(sql, param);
            }
            catch (SqlException ex)
            {
                queryResult.message = SqlErrorHandler.GetErrorMessage(ex);
                queryResult.result = false;
            }
            return queryResult;
        }

        public QueryResultModel Delete(string sql, object param)
        {
            var queryResult = new QueryResultModel();
            try
            {
                _dbConnection.Execute(sql, param);
            }
            catch (SqlException ex)
            {
                queryResult.message = SqlErrorHandler.GetErrorMessage(ex);
                queryResult.result = false;
            }
            return queryResult;
        }
    }
}