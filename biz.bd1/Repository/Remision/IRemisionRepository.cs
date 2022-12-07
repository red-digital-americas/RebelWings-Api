using biz.bd1.Entities;
using biz.bd1.Models;
using biz.bd1.Repository.Generic;

namespace biz.bd1.Repository.Remision;

public interface IRemisionRepository : IGenericRepository<Albventacab>
{
    public List<Remisiones> GetRemisionesByBranch(string branchName, DateTime startDateTime, DateTime endDateTime);
}