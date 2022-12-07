using biz.bd2.Paged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace biz.bd2.Repository.Generic
{
    public interface IGenericRepository<T>
    {
        void Save();
        void Dispose();
        Task<int> SaveAsync();

        T Add(T t);
        Task<T> AddAsyn(T t);

        T Get(int id);
        Task<T> GetAsync(int id);

        int Count();
        Task<int> CountAsync();

        void Delete(T entity);
        Task<int> DeleteAsyn(T entity);

        T Update(T t, object key);
        Task<T> UpdateAsync(T t, object key);

        T Find(Expression<Func<T, bool>> match);
        Task<T> FindAsync(Expression<Func<T, bool>> match);

        bool Exists(int id);
        Task<bool> ExistsAsync(int id);
        bool Exists(Expression<Func<T, bool>> match);
        Task<bool> ExistsAsync(Expression<Func<T, bool>> match);

        ICollection<T> FindAll(Expression<Func<T, bool>> match);
        Task<ICollection<T>> FindAllAsync(Expression<Func<T, bool>> match);

        IQueryable<T> FindBy(Expression<Func<T, bool>> predicate);
        Task<ICollection<T>> FindByAsyn(Expression<Func<T, bool>> predicate);

        IQueryable<T> GetAll();
        Task<ICollection<T>> GetAllAsyn();
        IQueryable<T> GetAllIncluding(params Expression<Func<T, object>>[] includeProperties);

        PagedList<T> GetAllPaged(int pageNumber, int pageSize);
        Task<PagedList<T>> GetAllPagedAsyn(int pageNumber, int pageSize);
        void BeginTransaction();
        void EndTransaction();
        void RollBack();
        string CompressString(string text);
        string DecompressString(string compressedText);
        string UploadImageBase64(string image, string ruta, string extension);
        Stream DeleteFile(string ruta);
        bool IsBase64(string base64String);
    }
}
