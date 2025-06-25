using SubjectRegister.Common;

namespace SubjectRegister.BLL.Interfaces
{
    /// <summary>
    /// Các chức năng quản lý cơ bản
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ICommon<T> where T : class
    {
        QueryResultModel Add(T model);
        QueryResultModel Update(T model);
    }
}
