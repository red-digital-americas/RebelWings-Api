using System;
using System.Collections.Generic;
using biz.rebel_wings.Models.Dashboard;
using biz.rebel_wings.Models.Transfer;
using biz.rebel_wings.Repository.Generic;

namespace biz.rebel_wings.Repository.Dashboard;

public interface IDashboardRepository : IGenericRepository<Entities.SatisfactionSurvey>
{
    DashboardAdmin GetAdmin(DateTime date, DateTime dateEnd, List<TransfersList>  transfersLists);
    DashboardSupervisor GetSupervisors(int id, DateTime date, DateTime dateEnd, int city);
    DashboardSupervisor GetSupervisorsV2(int id, DateTime date, DateTime dateEnd, int isDone, int city);
    DashboardRegional GetRegional(int id, DateTime date, DateTime dateEnd, int city);
    DashboardRegional GetRegionalV2(int id, DateTime date, DateTime dateEnd, int isDone, int city);
    DashboardAssistanceV2 GetAssistance(int id, DateTime date, DateTime dateEnd);
}