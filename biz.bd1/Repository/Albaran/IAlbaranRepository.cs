using biz.bd1.Entities;
using biz.bd1.Models;
using biz.bd1.Repository.Generic;

namespace biz.bd1.Repository.Albaran;

public interface IAlbaranRepository : IGenericRepository<Albcompracab>
{
    List<Albaranes> getAlbaranesList(string branchName, int pageNumber, int pageSize);
}