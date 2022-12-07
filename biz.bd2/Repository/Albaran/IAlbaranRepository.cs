using biz.bd2.Entities;
using biz.bd2.Models;
using biz.bd2.Repository.Generic;

namespace biz.bd2.Repository.Albaran;

public interface IAlbaranRepository : IGenericRepository<Albcompracab>
{
    List<Albaranes> getAlbaranesList(string branchName, int pageNumber, int pageSize);
}