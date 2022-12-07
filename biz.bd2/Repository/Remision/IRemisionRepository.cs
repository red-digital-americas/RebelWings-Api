using biz.bd2.Entities;
using biz.bd2.Models;
using biz.bd2.Repository.Generic;

namespace biz.bd2.Repository.Remision;

public interface IRemisionRepository : IGenericRepository<Albventacab>
{
    public List<Remisiones> GetRemisionesByBranch(string branchName, DateTime startDateTime, DateTime endDateTime);
}