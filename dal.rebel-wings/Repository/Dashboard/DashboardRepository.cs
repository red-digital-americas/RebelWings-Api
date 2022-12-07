using biz.rebel_wings.Models.Dashboard;
using biz.rebel_wings.Models.Transfer;
using biz.rebel_wings.Repository.Dashboard;
using dal.rebel_wings.DBContext;
using dal.rebel_wings.Repository.Generic;
using Microsoft.EntityFrameworkCore;

namespace dal.rebel_wings.Repository.Dashboard;

public class DashboardRepository : GenericRepository<biz.rebel_wings.Entities.SatisfactionSurvey>, IDashboardRepository
{
    public DashboardRepository(Db_Rebel_WingsContext context) : base(context)
    {
    }

    public DashboardAdmin GetAdmin(DateTime date, DateTime dateEnd, List<TransfersList> transfersLists)
    {
        // **Start** Activity Report
        var activities = new ActivityReportAdmin();
        #region Tickets
        
        activities.Tickets = _context.Ticketings.Count(x =>
            x.CreatedDate.Date > date.Date && x.CreatedDate.Date < dateEnd && x.Status.Equals(true));

        #endregion
        #region Average Number Attendances

        int attendance = _context.ValidateAttendances.Count(x =>
            x.CreatedDate.Date > date.Date && x.CreatedDate.Date < dateEnd);
        int noAttendance = _context.ValidateAttendances.Count(x =>
            x.CreatedDate.Date > date.Date && x.CreatedDate.Date < dateEnd && x.Attendence == 2);
        activities.AverageNumberAttendances = noAttendance > 0 ? (decimal)noAttendance * 100 / attendance : 0;

        #endregion
        #region Average Evaluation

        var surveys = _context.SatisfactionSurveys
            .Where(x => x.CreatedDate.Date > date.Date && x.CreatedDate.Date < dateEnd).Select(s => s.Review);
        activities.AverageEvaluation = surveys.Any() ? surveys.Average() : 0;

        #endregion
        #region Omission Activities

        List<bool> list = new List<bool>();
        list.Add(_context.ValidateAttendances.Any(x =>
            x.CreatedDate >= date && x.CreatedDate <= dateEnd));
        list.Add(_context.ValidationGas.Any(x=> x.CreatedDate >= date && x.CreatedDate <= dateEnd));
        list.Add(_context.StockChickens.Any(x=> x.CreatedDate >= date && x.CreatedDate <= dateEnd));
        list.Add(_context.Salons.Any(x=> x.CreatedDate >= date && x.CreatedDate <= dateEnd));
        // list.Add(_context.a.Any(x=> x.CreatedDate.Date > date.Date && x.CreatedDate.Date < date.Date && x.BranchId == branchId));
        list.Add(_context.RiskProducts.Any(x=> x.CreatedDate >= date && x.CreatedDate <= dateEnd));
        list.Add(_context.RequestTransfers.Any(x=> x.CreatedDate >= date && x.CreatedDate <= dateEnd));
        list.Add(_context.CashRegisterShortages.Any(x=> x.CreatedDate >= date && x.CreatedDate <= dateEnd));
        list.Add(_context.Tips.Any(x=> x.CreatedDate >= date && x.CreatedDate <= dateEnd));
        list.Add(_context.LivingRoomBathroomCleanings.Any(x=> x.CreatedDate >= date && x.CreatedDate <= dateEnd));
        list.Add(_context.TabletSafeKeepings.Any(x=> x.CreatedDate >= date && x.CreatedDate <= dateEnd));
        list.Add(_context.Alarms.Any(x=> x.CreatedDate >= date && x.CreatedDate <= dateEnd));
        list.Add(_context.WaitlistTables.Any(x=> x.CreatedDate >= date && x.CreatedDate <= dateEnd));
        // list.Add(_context.Albarans.Any(x=> x.CreatedDate.Date > date.Date && x.CreatedDate.Date < date.Date && x. == branchId));
        activities.OmissionActivities = (decimal)list.Count(c => c.Equals(true)) * 100 / list.Count;

        #endregion
        // **End** Activity Report

        // Start Branches With Most Omissions
        var branches = new List<BranchWithMostOmissions>();
        #region Branches

        foreach (var transfersList in transfersLists)
        {
            List<bool> omissions = new List<bool>();
            omissions.Add(_context.ValidateAttendances.Any(x =>
                x.CreatedDate >= date && x.CreatedDate <= dateEnd && x.BranchId == transfersList.BranchId));
            omissions.Add(_context.ValidationGas.Any(x=> x.CreatedDate >= date && x.CreatedDate <= dateEnd && x.Branch == transfersList.BranchId));
            omissions.Add(_context.StockChickens.Any(x=> x.CreatedDate >= date && x.CreatedDate <= dateEnd && x.Branch == transfersList.BranchId));
            omissions.Add(_context.Salons.Any(x=> x.CreatedDate >= date && x.CreatedDate <= dateEnd && x.BranchId == transfersList.BranchId));
            // list.Add(_context.a.Any(x=> x.CreatedDate.Date > date.Date && x.CreatedDate.Date < date.Date && x.BranchId == branchId));
            omissions.Add(_context.RiskProducts.Any(x=> x.CreatedDate >= date && x.CreatedDate <= dateEnd && x.BranchId == transfersList.BranchId));
            omissions.Add(_context.RequestTransfers.Any(x =>
                x.CreatedDate >= date && x.CreatedDate <= dateEnd && (x.FromBranchId == transfersList.BranchId ||
                                                                      x.ToBranchId == transfersList.BranchId)));
            omissions.Add(_context.CashRegisterShortages.Any(x=> x.CreatedDate >= date && x.CreatedDate <= dateEnd && x.BranchId == transfersList.BranchId));
            omissions.Add(_context.Tips.Any(x=> x.CreatedDate >= date && x.CreatedDate <= dateEnd && x.BranchId == transfersList.BranchId));
            omissions.Add(_context.LivingRoomBathroomCleanings.Any(x=> x.CreatedDate >= date && x.CreatedDate <= dateEnd && x.BranchId == transfersList.BranchId));
            omissions.Add(_context.TabletSafeKeepings.Any(x=> x.CreatedDate >= date && x.CreatedDate <= dateEnd && x.BranchId == transfersList.BranchId));
            omissions.Add(_context.Alarms.Any(x=> x.CreatedDate >= date && x.CreatedDate <= dateEnd && x.BranchId == transfersList.BranchId));
            omissions.Add(_context.WaitlistTables.Any(x=> x.CreatedDate >= date && x.CreatedDate <= dateEnd && x.Branch == transfersList.BranchId));
            
            branches.Add(new BranchWithMostOmissions()
            {
                Id = transfersList.BranchId,
                BranchName = transfersList.Name,
                Percentage = (decimal)omissions.Count(c => c.Equals(false)) * 100 / omissions.Count
            });
        }

        #endregion
        // End Branches With Most Omissions
        
        // Start Omissions Per Shifts
        
        DateTime middleDay = date.AddHours(12);
        List<Tuple<int, string>> listPerShifts = new List<Tuple<int, string>>();
        List<Tuple<int, string>> listMorning = new List<Tuple<int, string>>();
        listMorning.Add(new Tuple<int, string>(_context.ValidateAttendances.Count(x =>
            x.CreatedDate >= date && x.CreatedDate <= dateEnd), "Validación de asistencias"));
        listMorning.Add(new Tuple<int, string>(_context.ValidationGas.Count(x=> x.CreatedDate >= date && x.CreatedDate <= middleDay), ""));
        listMorning.Add(new Tuple<int, string>(_context.StockChickens.Count(x=> x.CreatedDate >= date && x.CreatedDate <= middleDay), ""));
        listMorning.Add(new Tuple<int, string>(_context.Salons.Count(x=> x.CreatedDate >= date && x.CreatedDate <= middleDay), ""));
        listMorning.Add(new Tuple<int, string>(_context.RiskProducts.Count(x=> x.CreatedDate >= date && x.CreatedDate <= middleDay), ""));
        listMorning.Add(new Tuple<int, string>(_context.RequestTransfers.Count(x=> x.CreatedDate >= date && x.CreatedDate <= middleDay), ""));
        listMorning.Add(new Tuple<int, string>(_context.CashRegisterShortages.Count(x=> x.CreatedDate >= date && x.CreatedDate <= middleDay), ""));
        listMorning.Add(new Tuple<int, string>(_context.Tips.Count(x=> x.CreatedDate >= date && x.CreatedDate <= middleDay), ""));
        listMorning.Add(new Tuple<int, string>(_context.LivingRoomBathroomCleanings.Count(x=> x.CreatedDate >= date && x.CreatedDate <= middleDay), ""));
        listMorning.Add(new Tuple<int, string>(_context.TabletSafeKeepings.Count(x=> x.CreatedDate >= date && x.CreatedDate <= middleDay), ""));
        listMorning.Add(new Tuple<int, string>(_context.Alarms.Count(x=> x.CreatedDate >= date && x.CreatedDate <= middleDay), ""));
        listMorning.Add(new Tuple<int, string>(_context.WaitlistTables.Count(x=> x.CreatedDate >= date && x.CreatedDate <= middleDay), ""));
        
        listPerShifts.Add(new Tuple<int, string>(listMorning.Sum(s=>s.Item1), "Matutino"));
        
        List<Tuple<int, string>> listAfternoon = new List<Tuple<int, string>>();
        listAfternoon.Add(new Tuple<int, string>(_context.ValidateAttendances.Count(x =>
            x.CreatedDate >= middleDay && x.CreatedDate <= dateEnd), ""));
        listAfternoon.Add(new Tuple<int, string>(_context.ValidationGas.Count(x=> x.CreatedDate >= middleDay && x.CreatedDate <= dateEnd), ""));
        listAfternoon.Add(new Tuple<int, string>(_context.StockChickens.Count(x=> x.CreatedDate >= middleDay && x.CreatedDate <= dateEnd), ""));
        listAfternoon.Add(new Tuple<int, string>(_context.Salons.Count(x=> x.CreatedDate >= middleDay && x.CreatedDate <= dateEnd), ""));
        listAfternoon.Add(new Tuple<int, string>(_context.RiskProducts.Count(x=> x.CreatedDate >= middleDay && x.CreatedDate <= dateEnd), ""));
        listAfternoon.Add(new Tuple<int, string>(_context.RequestTransfers.Count(x=> x.CreatedDate >= middleDay && x.CreatedDate <= dateEnd), ""));
        listAfternoon.Add(new Tuple<int, string>(_context.CashRegisterShortages.Count(x=> x.CreatedDate >= middleDay && x.CreatedDate <= dateEnd), ""));
        listAfternoon.Add(new Tuple<int, string>(_context.Tips.Count(x=> x.CreatedDate >= middleDay && x.CreatedDate <= dateEnd), ""));
        listAfternoon.Add(new Tuple<int, string>(_context.LivingRoomBathroomCleanings.Count(x=> x.CreatedDate >= date && x.CreatedDate <= dateEnd), ""));
        listAfternoon.Add(new Tuple<int, string>(_context.TabletSafeKeepings.Count(x=> x.CreatedDate >= middleDay && x.CreatedDate <= dateEnd), ""));
        listAfternoon.Add(new Tuple<int, string>(_context.Alarms.Count(x=> x.CreatedDate >= middleDay && x.CreatedDate <= dateEnd), ""));
        listAfternoon.Add(new Tuple<int, string>(_context.WaitlistTables.Count(x=> x.CreatedDate >= middleDay && x.CreatedDate <= dateEnd), ""));
        
        listPerShifts.Add(new Tuple<int, string>(listAfternoon.Sum(s=>s.Item1), "Vespertino"));

        List<OmissionsPerShift> omissionsPerShifts = new List<OmissionsPerShift>();
        omissionsPerShifts.Add(new OmissionsPerShift(){Count = listPerShifts.FirstOrDefault(x=>x.Item2.Equals("Matutino")).Item1, Shift = 1});
        omissionsPerShifts.Add(new OmissionsPerShift(){Count = listPerShifts.FirstOrDefault(x=>x.Item2.Equals("Vespertino")).Item1, Shift = 2});
        // End Omissions Per Shifts
        
        // Start Most Omitted Activities
        
        var listMostOmitteds = new List<MostOmittedActivities>();
        var validate = (decimal) _context.ValidateAttendances.Count(x => x.CreatedDate >= date && x.CreatedDate <= dateEnd) * 100 / 12 ;
        listMostOmitteds.Add(new MostOmittedActivities()
            { Id = 1, Name = "Validación de asistencias", Percentage = validate });
        listMostOmitteds.Add(new MostOmittedActivities()
            { Id = 2, Name = "validación de gas", Percentage = (decimal) _context.ValidationGas.Count(x=> x.CreatedDate >= date && x.CreatedDate <= dateEnd) * 100 / 12 });
        listMostOmitteds.Add(new MostOmittedActivities()
            { Id = 3, Name = "Stock de Pollo", Percentage = (decimal) _context.StockChickens.Count(x=> x.CreatedDate >= date && x.CreatedDate <= dateEnd) * 100 / 12 });
        listMostOmitteds.Add(new MostOmittedActivities()
            { Id = 4, Name = "Salon", Percentage = (decimal) _context.Salons.Count(x=> x.CreatedDate >= date && x.CreatedDate <= dateEnd) * 100 / 12 });
        listMostOmitteds.Add(new MostOmittedActivities()
            { Id = 5, Name = "Productos en riesgo", Percentage = (decimal) _context.RiskProducts.Count(x=> x.CreatedDate >= date && x.CreatedDate <= dateEnd) * 100 / 12 });
        listMostOmitteds.Add(new MostOmittedActivities()
            { Id = 6, Name = "Petición de transferencias", Percentage = (decimal) _context.RequestTransfers.Count(x=> x.CreatedDate >= date && x.CreatedDate <= dateEnd) * 100 / 12 });
        listMostOmitteds.Add(new MostOmittedActivities()
            { Id = 7, Name = "Corte de caja", Percentage = (decimal) _context.CashRegisterShortages.Count(x=> x.CreatedDate >= date && x.CreatedDate <= dateEnd) * 100 / 12 });
        listMostOmitteds.Add(new MostOmittedActivities()
            { Id = 8, Name = "Tip", Percentage = (decimal) _context.Tips.Count(x=> x.CreatedDate >= date && x.CreatedDate <= dateEnd) * 100 / 12 });
        listMostOmitteds.Add(new MostOmittedActivities()
            { Id = 9, Name = "Limpieza de Salon y baños", Percentage = (decimal) _context.LivingRoomBathroomCleanings.Count(x=> x.CreatedDate >= date && x.CreatedDate <= dateEnd) * 100 / 12 });
        listMostOmitteds.Add(new MostOmittedActivities()
            { Id = 10, Name = "Resguardo de tabletas", Percentage = (decimal) _context.TabletSafeKeepings.Count(x=> x.CreatedDate >= date && x.CreatedDate <= dateEnd) * 100 / 12 });
        listMostOmitteds.Add(new MostOmittedActivities()
            { Id = 11, Name = "Alarmas", Percentage = (decimal) _context.Alarms.Count(x=> x.CreatedDate >= date && x.CreatedDate <= dateEnd) * 100 / 12 });
        listMostOmitteds.Add(new MostOmittedActivities()
            { Id = 12, Name = "Lista de espera", Percentage = (decimal) _context.WaitlistTables.Count(x=> x.CreatedDate >= date && x.CreatedDate <= dateEnd) * 100 / 12 });

        // End Most Omitted Activities
        
        var dashboard = new DashboardAdmin()
        {
            ActivityReportAdmin = activities,
            BranchWithMostOmissionsCollection = branches.OrderByDescending(o=>o.Percentage).Take(5).ToList(),
            OmissionsPerShifts = omissionsPerShifts,
            MostOmittedActivitiesCollection = listMostOmitteds.Where(x=>x.Percentage !=0).ToList()
        };
        return dashboard;
    }

    public DashboardSupervisor GetSupervisors(int id, DateTime date, DateTime dateEnd)
    {
        var dashboardSupervisor = new DashboardSupervisor();

        #region Tickets

        dashboardSupervisor.Tickets = _context.Ticketings.Count(c => c.BranchId == id && c.Status == true);

        #endregion

        #region Average Attendance

        int total = _context.ValidateAttendances.Count(x =>
            x.BranchId == id && x.CreatedDate >= date && x.CreatedDate <= dateEnd);
        int validateAttendances = _context.ValidateAttendances.Count(x =>
            x.BranchId == id && x.CreatedDate >= date && x.CreatedDate <= dateEnd && x.Attendence == 1);
        dashboardSupervisor.AverageAttendance = total != 0 ? (decimal)validateAttendances * 100 / total : 0;

        #endregion

        #region Omissions Activities

        List<bool> list = new List<bool>();
        list.Add(_context.ValidateAttendances.Any(x =>
            x.CreatedDate >= date && x.CreatedDate <= dateEnd && x.BranchId == id));
        list.Add(_context.ValidationGas.Any(x=> x.CreatedDate >= date && x.CreatedDate <= dateEnd && x.Branch == id));
        list.Add(_context.StockChickens.Any(x=> x.CreatedDate >= date && x.CreatedDate <= dateEnd && x.Branch == id));
        list.Add(_context.Salons.Any(x=> x.CreatedDate >= date && x.CreatedDate <= dateEnd && x.BranchId == id));
        list.Add(_context.RiskProducts.Any(x=> x.CreatedDate >= date && x.CreatedDate <= dateEnd && x.BranchId == id));
        list.Add(_context.RequestTransfers.Any(x=> x.CreatedDate >= date && x.CreatedDate <= dateEnd && (x.ToBranchId == id || x.FromBranchId == id)));
        list.Add(_context.CashRegisterShortages.Any(x=> x.CreatedDate >= date && x.CreatedDate <= dateEnd && x.BranchId == id));
        list.Add(_context.Tips.Any(x=> x.CreatedDate >= date && x.CreatedDate <= dateEnd && x.BranchId == id));
        list.Add(_context.LivingRoomBathroomCleanings.Any(x=> x.CreatedDate >= date && x.CreatedDate <= dateEnd && x.BranchId == id));
        list.Add(_context.TabletSafeKeepings.Any(x=> x.CreatedDate >= date && x.CreatedDate <= dateEnd && x.BranchId == id));
        list.Add(_context.Alarms.Any(x=> x.CreatedDate >= date && x.CreatedDate <= dateEnd && x.BranchId == id));
        list.Add(_context.WaitlistTables.Any(x=> x.CreatedDate >= date && x.CreatedDate <= dateEnd && x.Branch == id));
        var percetage = (decimal)list.Count(c => c.Equals(true)) * 100 / 12;
        var resPercentage = (decimal)percetage - 100;
        dashboardSupervisor.OmissionsActivities = Decimal.Negate(resPercentage);

        #endregion

        #region Tasks

        DateTime dateMiddle = date.AddHours(12);
        #region Start Morning
        var taskPerShiftsMorningList = new List<TaskPerShifts>();
        var validateAttendanceList = _context.ValidateAttendances.Where(x =>
            x.CreatedDate >= date && x.CreatedDate <= dateMiddle && x.BranchId == id).Select(s => new TaskPerShifts()
        {
            Date = s.CreatedDate,
            Detail = s.Id,
            Status = true,
            Supervisor =
                $"{s.CreatedByNavigation.Name} {s.CreatedByNavigation.LastName} {s.CreatedByNavigation.MotherName}",
            NameTask = "Validación de asistencias",
            PercentageComplete = 100
        }).ToList();

        taskPerShiftsMorningList.AddRange(validateAttendanceList.Any()
            ? validateAttendanceList
            : new TaskPerShifts[] { new TaskPerShifts()
            {
                Date = dateMiddle,
                Detail = 0,
                Status = false,
                Supervisor = "",
                NameTask = "Validación de asistencias",
                PercentageComplete = 0
            } });
        var validationGasList= _context.ValidationGas.Where(x =>
            x.CreatedDate >= date && x.CreatedDate <= dateMiddle && x.Branch == id).Select(s => new TaskPerShifts()
        {
            Date = s.CreatedDate,
            Detail = s.Id,
            Status = true,
            Supervisor = "",
            NameTask = "Validación de gas",
            PercentageComplete = 100
        }).ToList();
        taskPerShiftsMorningList.AddRange(validationGasList.Any()
            ? validationGasList
            : new TaskPerShifts[] { new TaskPerShifts()
            {
                Date = dateMiddle,
                Detail = 0,
                Status = false,
                Supervisor = "",
                NameTask = "Validación de gas",
                PercentageComplete = 0
            } });

        var stockChickensList = _context.StockChickens.Where(x =>
            x.CreatedDate >= date && x.CreatedDate <= dateMiddle && x.Branch == id).Select(s => new TaskPerShifts()
        {
            Date = s.CreatedDate,
            Detail = s.Id,
            Status = true,
            Supervisor =
                $"{s.CreatedByNavigation.Name} {s.CreatedByNavigation.LastName} {s.CreatedByNavigation.MotherName}",
            NameTask = "Stock de pollo",
            PercentageComplete = 100
        }).ToList();
        taskPerShiftsMorningList.AddRange(stockChickensList.Any()
            ? stockChickensList
            : new TaskPerShifts[] { new TaskPerShifts()
            {
                Date = dateMiddle,
                Detail = 0,
                Status = false,
                Supervisor = "",
                NameTask = "Stock de pollo",
                PercentageComplete = 0
            } });

        var salonsList = _context.ToSetTables.Where(x =>
            x.CreatedDate.Date == date.Date && x.Branch == id).Select(s => new TaskPerShifts()
        {
            Date = s.CreatedDate,
            Detail = s.Id,
            Status = true,
            Supervisor = $"",
            NameTask = "SALÓN MONTADO",
            PercentageComplete = 100
        }).ToList();
        taskPerShiftsMorningList.AddRange(salonsList.Any()
            ? salonsList
            : new TaskPerShifts[] { new TaskPerShifts()
                {
                    Date = dateMiddle,
                    Detail = 0,
                    Status = false,
                    Supervisor = "",
                    NameTask = "SALÓN MONTADO",
                    PercentageComplete = 0
                } });

        var waitListTablesList = _context.WaitlistTables.Where(x =>
            x.CreatedDate >= date && x.CreatedDate <= dateMiddle && x.Branch == id).Select(s => new TaskPerShifts()
        {
            Date = s.CreatedDate,
            Detail = s.Id,
            Status = true,
            Supervisor =
                $"{s.CreatedByNavigation.Name} {s.CreatedByNavigation.LastName} {s.CreatedByNavigation.MotherName}",
            NameTask = "Mesas en espera",
            PercentageComplete = 100
        }).ToList();
        taskPerShiftsMorningList.AddRange(waitListTablesList.Any()
            ? waitListTablesList
            : new TaskPerShifts[] { new TaskPerShifts()
            {
                Date = dateMiddle,
                Detail = 0,
                Status = false,
                Supervisor = "",
                NameTask = "Mesas en espera",
                PercentageComplete = 0
            } });
        
        dashboardSupervisor.TasksMorningsCollection = taskPerShiftsMorningList;
        #endregion
        #region Start Evening
        var taskPerShiftsEveningList = new List<TaskPerShifts>();
        var validateAttendancesList = _context.ValidateAttendances.Where(x =>
            x.CreatedDate >= dateMiddle && x.CreatedDate <= dateEnd && x.BranchId == id).Select(s => new TaskPerShifts()
        {
            Date = s.CreatedDate,
            Detail = s.Id,
            Status = true,
            Supervisor =
                $"{s.CreatedByNavigation.Name} {s.CreatedByNavigation.LastName} {s.CreatedByNavigation.MotherName}",
            NameTask = "Validación de asistencias",
            PercentageComplete = 100
        }).ToList();
        taskPerShiftsEveningList.AddRange(validateAttendancesList.Any()
            ? validateAttendancesList
            : new TaskPerShifts[]
            {
                new TaskPerShifts()
                {
                    Date = dateMiddle,
                    Detail = 0,
                    Status = false,
                    Supervisor = "",
                    NameTask = "Validación de asistencias",
                    PercentageComplete = 0
                }
            });

        var stockChickensListMorning = _context.StockChickens.Where(x =>
            x.CreatedDate >= dateMiddle && x.CreatedDate <= dateEnd && x.Branch == id).Select(s => new TaskPerShifts()
        {
            Date = s.CreatedDate,
            Detail = s.Id,
            Status = true,
            Supervisor =
                $"{s.CreatedByNavigation.Name} {s.CreatedByNavigation.LastName} {s.CreatedByNavigation.MotherName}",
            NameTask = "Stock de pollo",
            PercentageComplete = 100
        }).ToList();
        taskPerShiftsEveningList.AddRange(stockChickensListMorning.Any()
            ? stockChickensListMorning
            : new TaskPerShifts[]
            {
                new TaskPerShifts()
                {
                    Date = dateMiddle,
                    Detail = 0,
                    Status = false,
                    Supervisor = "",
                    NameTask = "Stock de pollo",
                    PercentageComplete = 0
                }
            });

        var riskProductsList = _context.RiskProducts.Where(x =>
            x.CreatedDate >= dateMiddle && x.CreatedDate <= dateEnd && x.BranchId == id).Select(s => new TaskPerShifts()
        {
            Date = s.CreatedDate,
            Detail = s.Id,
            Status = true,
            Supervisor =
                $"{s.CreatedByNavigation.Name} {s.CreatedByNavigation.LastName} {s.CreatedByNavigation.MotherName}",
            NameTask = "Producto en riesgo",
            PercentageComplete = 100
        }).ToList();
        taskPerShiftsEveningList.AddRange(riskProductsList.Any()
            ? riskProductsList
            : new TaskPerShifts[]
            {
                new TaskPerShifts()
                {
                    Date = dateMiddle,
                    Detail = 0,
                    Status = false,
                    Supervisor = "",
                    NameTask = "Producto en riesgo",
                    PercentageComplete = 0
                }
            });

        var tipsList = _context.Tips.Where(x =>
            x.CreatedDate >= dateMiddle && x.CreatedDate <= dateEnd && x.BranchId == id).Select(s => new TaskPerShifts()
        {
            Date = s.CreatedDate,
            Detail = s.Id,
            Status = true,
            Supervisor =
                $"{s.CreatedByNavigation.Name} {s.CreatedByNavigation.LastName} {s.CreatedByNavigation.MotherName}",
            NameTask = "RESGUARDO DE PROPINA",
            PercentageComplete = 100
        }).ToList();
        taskPerShiftsEveningList.AddRange(tipsList.Any()
            ? tipsList
            : new TaskPerShifts[]
            {
                new TaskPerShifts()
                {
                    Date = dateMiddle,
                    Detail = 0,
                    Status = false,
                    Supervisor = "",
                    NameTask = "RESGUARDO DE PROPINA",
                    PercentageComplete = 0
                }
            });

        var livingRoomBathroomCleaningsList = _context.LivingRoomBathroomCleanings.Where(x =>
            x.CreatedDate >= dateMiddle && x.CreatedDate <= dateEnd && x.BranchId == id).Select(s => new TaskPerShifts()
        {
            Date = s.CreatedDate,
            Detail = s.Id,
            Status = true,
            Supervisor =
                $"{s.CreatedByNavigation.Name} {s.CreatedByNavigation.LastName} {s.CreatedByNavigation.MotherName}",
            NameTask = "Limpieza de salón y baños",
            PercentageComplete = 100
        }).ToList();
        taskPerShiftsEveningList.AddRange(livingRoomBathroomCleaningsList.Any()
            ? livingRoomBathroomCleaningsList
            : new TaskPerShifts[]
            {
                new TaskPerShifts()
                {
                    Date = dateMiddle,
                    Detail = 0,
                    Status = false,
                    Supervisor = "",
                    NameTask = "Limpieza de salón y baños",
                    PercentageComplete = 0
                }
            });

        var tabletSafeKeepingsList = _context.TabletSafeKeepings.Where(x =>
            x.CreatedDate >= dateMiddle && x.CreatedDate <= dateEnd && x.BranchId == id).Select(s => new TaskPerShifts()
        {
            Date = s.CreatedDate,
            Detail = s.Id,
            Status = true,
            Supervisor =
                $"{s.CreatedByNavigation.Name} {s.CreatedByNavigation.LastName} {s.CreatedByNavigation.MotherName}",
            NameTask = "Resguardo de tabletas",
            PercentageComplete = 100
        }).ToList();
        taskPerShiftsEveningList.AddRange(tabletSafeKeepingsList.Any()
            ? tabletSafeKeepingsList
            : new TaskPerShifts[]
            {
                new TaskPerShifts()
                {
                    Date = dateMiddle,
                    Detail = 0,
                    Status = false,
                    Supervisor = "",
                    NameTask = "Resguardo de tabletas",
                    PercentageComplete = 0
                }
            });

        var alarmsList = _context.Alarms.Where(x =>
            x.CreatedDate >= dateMiddle && x.CreatedDate <= dateEnd && x.BranchId == id).Select(s => new TaskPerShifts()
        {
            Date = s.CreatedDate,
            Detail = s.Id,
            Status = true,
            Supervisor =
                $"{s.CreatedByNavigation.Name} {s.CreatedByNavigation.LastName} {s.CreatedByNavigation.MotherName}",
            NameTask = "Alarmas",
            PercentageComplete = 100
        }).ToList();
        taskPerShiftsEveningList.AddRange(alarmsList.Any()
            ? alarmsList
            : new TaskPerShifts[]
            {
                new TaskPerShifts()
                {
                    Date = dateMiddle,
                    Detail = 0,
                    Status = false,
                    Supervisor = "",
                    NameTask = "Alarmas",
                    PercentageComplete = 0
                }
            });

        var waitListTablesListM = _context.WaitlistTables.Where(x =>
            x.CreatedDate >= dateMiddle && x.CreatedDate <= dateEnd && x.Branch == id).Select(s => new TaskPerShifts()
        {
            Date = s.CreatedDate,
            Detail = s.Id,
            Status = true,
            Supervisor =
                $"{s.CreatedByNavigation.Name} {s.CreatedByNavigation.LastName} {s.CreatedByNavigation.MotherName}",
            NameTask = "Mesas en espera",
            PercentageComplete = 100
        }).ToList();
        taskPerShiftsEveningList.AddRange(waitListTablesListM.Any()
            ? waitListTablesListM
            : new TaskPerShifts[]
            {
                new TaskPerShifts()
                {
                    Date = dateMiddle,
                    Detail = 0,
                    Status = false,
                    Supervisor = "",
                    NameTask = "Mesas en espera",
                    PercentageComplete = 0
                }
            });

        var requestTransferList = _context.RequestTransfers.Where(x =>
                x.CreatedDate >= dateMiddle && x.CreatedDate <= dateEnd && (x.FromBranchId == id || x.ToBranchId == id))
            .Select(s => new TaskPerShifts()
            {
                Date = s.CreatedDate,
                Detail = s.Id,
                Status = true,
                Supervisor =
                    $"{s.CreatedByNavigation.Name} {s.CreatedByNavigation.LastName} {s.CreatedByNavigation.MotherName}",
                NameTask = "Mesas en espera",
                PercentageComplete = 100
            }).ToList();
        taskPerShiftsEveningList.AddRange(requestTransferList.Any()
            ? requestTransferList
            : new TaskPerShifts[]
            {
                new TaskPerShifts()
                {
                    Date = dateMiddle,
                    Detail = 0,
                    Status = false,
                    Supervisor = "",
                    NameTask = "Mesas en espera",
                    PercentageComplete = 0
                }
            });

        var cashRegisterShortagesList = _context.CashRegisterShortages.Where(x =>
            x.CreatedDate >= dateMiddle && x.CreatedDate <= dateEnd && x.BranchId == id).Select(s => new TaskPerShifts()
        {
            Date = s.CreatedDate,
            Detail = s.Id,
            Status = true,
            Supervisor =
                $"{s.CreatedByNavigation.Name} {s.CreatedByNavigation.LastName} {s.CreatedByNavigation.MotherName}",
            NameTask = "Volado de efectivo",
            PercentageComplete = 100
        }).ToList();
        taskPerShiftsEveningList.AddRange(cashRegisterShortagesList.Any()
            ? cashRegisterShortagesList
            : new TaskPerShifts[]
            {
                new TaskPerShifts()
                {
                    Date = dateMiddle,
                    Detail = 0,
                    Status = false,
                    Supervisor = "",
                    NameTask = "Volado de efectivo",
                    PercentageComplete = 0
                }
            });

        var albaransList = _context.Albarans.Where(x =>
            x.CreatedDate >= dateMiddle && x.CreatedDate <= dateEnd && x.BranchId == id).Select(s => new TaskPerShifts()
        {
            Date = s.CreatedDate,
            Detail = s.Id,
            Status = true,
            Supervisor = $"",
            NameTask = "ALBARANES",
            PercentageComplete = 100
        }).ToList();
        taskPerShiftsEveningList.AddRange(albaransList.Any()
            ? albaransList
            : new TaskPerShifts[]
            {
                new TaskPerShifts()
                {
                    Date = dateMiddle,
                    Detail = 0,
                    Status = false,
                    Supervisor = "",
                    NameTask = "ALBARANES",
                    PercentageComplete = 0
                }
            });
        
        dashboardSupervisor.TasksEveningsCollection = taskPerShiftsEveningList;
        #endregion
        #endregion
        return dashboardSupervisor;
    }

    public DashboardRegional GetRegional(int id, DateTime date, DateTime dateEnd)
    {
        var dashboardRegional = new DashboardRegional();

        #region Tickets

        dashboardRegional.Tickets = _context.Ticketings.Count(c =>
            c.BranchId == id && c.Status == true && c.CreatedDate >= date && c.CreatedDate <= dateEnd);

        #endregion

        #region Average Evaluation

        var success = _context.SatisfactionSurveys.Where(x => x.BranchId == id && x.CreatedDate >= date && x.CreatedDate <= dateEnd).Sum(s=>s.Review);
        var totals = _context.SatisfactionSurveys.Count(x => x.BranchId == id && x.CreatedDate >= date && x.CreatedDate <= dateEnd);
        dashboardRegional.AverageEvaluation = totals > 0 ? (decimal)success / totals : 0;
            ;

        #endregion

        #region Omissions Activities

        var activities = new List<bool>();
        activities.Add(_context.Orders.Any(a=>a.BranchId == id && a.CreatedDate >= date && a.CreatedDate <= dateEnd));
        activities.Add(_context.Fridges.Any(a=>a.BranchId == id && a.CreatedDate >= date && a.CreatedDate <= dateEnd));
        activities.Add(_context.PrecookedChickens.Any(a=>a.BranchId == id && a.CreatedDate >= date && a.CreatedDate <= dateEnd));
        activities.Add(_context.CompleteProductsInOrders.Any(a=>a.BranchId == id && a.CreatedDate >= date && a.CreatedDate <= dateEnd));
        activities.Add(_context.FryerCleanings.Any(a=>a.BranchId == id && a.CreatedDate >= date && a.CreatedDate <= dateEnd));
        
        activities.Add(_context.PeopleCountings.Any(a=>a.BranchId == id && a.CreatedDate >= date && a.CreatedDate <= dateEnd));
        activities.Add(_context.SatisfactionSurveys.Any(a=>a.BranchId == id && a.CreatedDate >= date && a.CreatedDate <= dateEnd));
        activities.Add(_context.GeneralCleanings.Any(a=>a.BranchId == id && a.CreatedDate >= date && a.CreatedDate <= dateEnd));
        activities.Add(_context.Stations.Any(a=>a.BranchId == id && a.CreatedDate >= date && a.CreatedDate <= dateEnd));
        activities.Add(_context.DrinksTemperatures.Any(a=>a.BranchId == id && a.CreatedDate >= date && a.CreatedDate <= dateEnd));
        activities.Add(_context.AudioVideos.Any(a=>a.BranchId == id && a.CreatedDate >= date && a.CreatedDate <= dateEnd));
        activities.Add(_context.Spotlights.Any(a=>a.BranchId == id && a.CreatedDate >= date && a.CreatedDate <= dateEnd));
        activities.Add(_context.BarCleanings.Any(a=>a.BranchId == id && a.CreatedDate >= date && a.CreatedDate <= dateEnd));
        activities.Add(_context.FridgeSalons.Any(a=>a.BranchId == id && a.CreatedDate >= date && a.CreatedDate <= dateEnd));
        
        activities.Add(_context.BathRoomsOverallStatuses.Any(a=>a.BranchId == id && a.CreatedDate >= date && a.CreatedDate <= dateEnd));
        activities.Add(_context.WashBasinWithSoapPapers.Any(a=>a.BranchId == id && a.CreatedDate >= date && a.CreatedDate <= dateEnd));
        
        activities.Add(_context.TicketTables.Any(a=>a.BranchId == id && a.CreatedDate >= date && a.CreatedDate <= dateEnd));
        activities.Add(_context.EntriesChargedAsDeliveryNotes.Any(a=>a.BranchId == id && a.CreatedDate >= date && a.CreatedDate <= dateEnd));
        activities.Add(_context.OrderScheduleReviews.Any(a=>a.BranchId == id && a.CreatedDate >= date && a.CreatedDate <= dateEnd));
        activities.Add(_context.CheckTables.Any(a=>a.BranchId == id && a.CreatedDate >= date && a.CreatedDate <= dateEnd));
        
        activities.Add(_context.Kitchens.Any(a=>a.BranchId == id && a.CreatedDate >= date && a.CreatedDate <= dateEnd));
        activities.Add(_context.Salons.Any(a=>a.BranchId == id && a.CreatedDate >= date && a.CreatedDate <= dateEnd));
        activities.Add(_context.Bathrooms.Any(a=>a.BranchId == id && a.CreatedDate >= date && a.CreatedDate <= dateEnd));
        activities.Add(_context.Bars.Any(a=>a.BranchId == id && a.CreatedDate >= date && a.CreatedDate <= dateEnd));
        var totalActivities = activities.Count;
        var validateActivities = activities.Count(c => c.Equals(true));
        var percentage = (decimal)validateActivities * 100 / totalActivities;
        var resPercentage = percentage - 100;
        dashboardRegional.OmissionsActivities = Decimal.Negate(resPercentage);

        #endregion

        #region Tasks

        var tasks = new List<biz.rebel_wings.Models.Dashboard.Task>();

        #region Tasks Kitchen

        var ordersList = _context.Orders.Where(a => a.BranchId == id && a.CreatedDate >= date && a.CreatedDate <= dateEnd)
            .Select(s => new biz.rebel_wings.Models.Dashboard.Task
            {
                Date = s.CreatedDate,
                Detail = s.Id,
                Name = "Órdenes",
                Regional = $"{_context.Users.First(f => f.Id == s.CreatedBy).Name}",
                Status = 1,
                PercentageComplete = 100
            }).ToList();
        tasks.AddRange(ordersList.Any()
            ? ordersList
            : new biz.rebel_wings.Models.Dashboard.Task[]
            {
                new biz.rebel_wings.Models.Dashboard.Task()
                {
                    Date = date,
                    Detail = 0,
                    Status = 0,
                    Name = "Órdenes",
                    Regional = "",
                    PercentageComplete = 0
                }
            });

        var fridgesList = _context.Fridges.Where(a =>
                a.BranchId == id && a.CreatedDate >= date && a.CreatedDate <= dateEnd)
            .Select(s => new biz.rebel_wings.Models.Dashboard.Task
            {
                Date = s.CreatedDate,
                Detail = s.Id,
                Name = "Refrigeradores",
                Regional = $"{_context.Users.First(f => f.Id == s.CreatedBy).Name}",
                Status = 1,
                PercentageComplete = 100
            }).ToList();
        tasks.AddRange(fridgesList.Any()
            ? fridgesList
            : new biz.rebel_wings.Models.Dashboard.Task[]
            {
                new biz.rebel_wings.Models.Dashboard.Task()
                {
                    Date = date,
                    Detail = 0,
                    Status = 0,
                    Name = "Refrigeradores",
                    Regional = "",
                    PercentageComplete = 0
                }
            });

        var precookedChickensList = _context.PrecookedChickens.Where(a => a.BranchId == id && a.CreatedDate >= date && a.CreatedDate <= dateEnd)
            .Select(s => new biz.rebel_wings.Models.Dashboard.Task
            {
                Date = s.CreatedDate,
                Detail = s.Id,
                Name = "Pollo con precocción",
                Regional = $"{_context.Users.First(f => f.Id == s.CreatedBy).Name}",
                Status = 1,
                PercentageComplete = 100
            }).ToList();
        tasks.AddRange(precookedChickensList.Any()
            ? precookedChickensList
            : new biz.rebel_wings.Models.Dashboard.Task[]
            {
                new biz.rebel_wings.Models.Dashboard.Task()
                {
                    Date = date,
                    Detail = 0,
                    Status = 0,
                    Name = "Pollo con precocción",
                    Regional = "",
                    PercentageComplete = 0
                }
            });

        var completeProductInOrdersList = _context.CompleteProductsInOrders
            .Where(a => a.BranchId == id && a.CreatedDate >= date && a.CreatedDate <= dateEnd)
            .Select(s => new biz.rebel_wings.Models.Dashboard.Task
            {
                Date = s.CreatedDate,
                Detail = s.Id,
                Name = "Productos completos y en Órden",
                Regional = $"{_context.Users.First(f => f.Id == s.CreatedBy).Name}",
                Status = 1,
                PercentageComplete = 100
            }).ToList();
        tasks.AddRange(completeProductInOrdersList.Any()
            ? completeProductInOrdersList
            : new biz.rebel_wings.Models.Dashboard.Task[]
            {
                new biz.rebel_wings.Models.Dashboard.Task()
                {
                    Date = date,
                    Detail = 0,
                    Status = 0,
                    Name = "Productos completos y en Órden",
                    Regional = "",
                    PercentageComplete = 0
                }
            });

        var fryerCleaningsList = _context.FryerCleanings
            .Where(a => a.BranchId == id && a.CreatedDate >= date && a.CreatedDate <= dateEnd)
            .Select(s => new biz.rebel_wings.Models.Dashboard.Task
            {
                Date = s.CreatedDate,
                Detail = s.Id,
                Name = "Limpieza de freidoras",
                Regional = $"{_context.Users.First(f => f.Id == s.CreatedBy).Name}",
                Status = 1,
                PercentageComplete = 100
            }).ToList();
        tasks.AddRange(fryerCleaningsList.Any()
            ? fryerCleaningsList
            : new biz.rebel_wings.Models.Dashboard.Task[]
            {
                new biz.rebel_wings.Models.Dashboard.Task()
                {
                    Date = date,
                    Detail = 0,
                    Status = 0,
                    Name = "Limpieza de freidoras",
                    Regional = "",
                    PercentageComplete = 0
                }
            });
        dashboardRegional.TasksKitchenCollection = tasks.OrderBy(o=>o.Date).ToList();
        tasks.Clear();

        #endregion

        #region Tasks Salon

        var peopleCountingList = _context.PeopleCountings.Where(a => a.BranchId == id && a.CreatedDate >= date && a.CreatedDate <= dateEnd)
            .Select(s => new biz.rebel_wings.Models.Dashboard.Task
            {
                Date = s.CreatedDate,
                Detail = s.Id,
                Name = "Conteo de personas",
                Regional = $"{_context.Users.First(f => f.Id == s.CreatedBy).Name}",
                Status = 1,
                PercentageComplete = 100
            }).ToList();
        tasks.AddRange(peopleCountingList.Any()
            ? peopleCountingList
            : new biz.rebel_wings.Models.Dashboard.Task[]
            {
                new biz.rebel_wings.Models.Dashboard.Task()
                {
                    Date = date,
                    Detail = 0,
                    Status = 0,
                    Name = "Conteo de personas",
                    Regional = "",
                    PercentageComplete = 0
                }
            });

        var satisfactionSurveysList = _context.SatisfactionSurveys.Where(a => a.BranchId == id && a.CreatedDate >= date && a.CreatedDate <= dateEnd)
            .Select(s => new biz.rebel_wings.Models.Dashboard.Task
            {
                Date = s.CreatedDate,
                Detail = s.Id,
                Name = "Encuesta",
                Regional = $"{_context.Users.First(f => f.Id == s.CreatedBy).Name}",
                Status = 1,
                PercentageComplete = 100
            }).ToList();
        tasks.AddRange(satisfactionSurveysList.Any()
            ? satisfactionSurveysList
            : new biz.rebel_wings.Models.Dashboard.Task[]
            {
                new biz.rebel_wings.Models.Dashboard.Task()
                {
                    Date = date,
                    Detail = 0,
                    Status = 0,
                    Name = "Encuesta",
                    Regional = "",
                    PercentageComplete = 0
                }
            });

        var generalCleaningsList = _context.GeneralCleanings
            .Where(a => a.BranchId == id && a.CreatedDate >= date && a.CreatedDate <= dateEnd)
            .Select(s => new biz.rebel_wings.Models.Dashboard.Task
            {
                Date = s.CreatedDate,
                Detail = s.Id,
                Name = "Limpieza general",
                Regional = $"{_context.Users.First(f => f.Id == s.CreatedBy).Name}",
                Status = 1,
                PercentageComplete = 100
            }).ToList();
        tasks.AddRange(generalCleaningsList.Any()
            ? generalCleaningsList
            : new biz.rebel_wings.Models.Dashboard.Task[]
            {
                new biz.rebel_wings.Models.Dashboard.Task()
                {
                    Date = date,
                    Detail = 0,
                    Status = 0,
                    Name = "Limpieza general",
                    Regional = "",
                    PercentageComplete = 0
                }
            });

        var stationsList = _context.Stations
            .Where(a => a.BranchId == id && a.CreatedDate >= date && a.CreatedDate <= dateEnd)
            .Select(s => new biz.rebel_wings.Models.Dashboard.Task
            {
                Date = s.CreatedDate,
                Detail = s.Id,
                Name = "Estación",
                Regional = $"{_context.Users.First(f => f.Id == s.CreatedBy).Name}",
                Status = 1,
                PercentageComplete = 100
            }).ToList();
        tasks.AddRange(stationsList.Any()
            ? stationsList
            : new biz.rebel_wings.Models.Dashboard.Task[]
            {
                new biz.rebel_wings.Models.Dashboard.Task()
                {
                    Date = date,
                    Detail = 0,
                    Status = 0,
                    Name = "Estación",
                    Regional = "",
                    PercentageComplete = 0
                }
            });

        var drinksTemperaturesList = _context.DrinksTemperatures
            .Where(a => a.BranchId == id && a.CreatedDate >= date && a.CreatedDate <= dateEnd)
            .Select(s => new biz.rebel_wings.Models.Dashboard.Task
            {
                Date = s.CreatedDate,
                Detail = s.Id,
                Name = "Temperatura de Bebidas",
                Regional = $"{_context.Users.First(f => f.Id == s.CreatedBy).Name}",
                Status = 1,
                PercentageComplete = 100
            }).ToList();
        tasks.AddRange(drinksTemperaturesList.Any()
            ? drinksTemperaturesList
            : new biz.rebel_wings.Models.Dashboard.Task[]
            {
                new biz.rebel_wings.Models.Dashboard.Task()
                {
                    Date = date,
                    Detail = 0,
                    Status = 0,
                    Name = "Temperatura de Bebidas",
                    Regional = "",
                    PercentageComplete = 0
                }
            });

        var audioVideosList = _context.AudioVideos.Where(a => a.BranchId == id && a.CreatedDate >= date && a.CreatedDate <= dateEnd)
            .Select(s => new biz.rebel_wings.Models.Dashboard.Task
            {
                Date = s.CreatedDate,
                Detail = s.Id,
                Name = "Audio y Video",
                Regional = $"{_context.Users.First(f => f.Id == s.CreatedBy).Name}",
                Status = 1,
                PercentageComplete = 100
            }).ToList();
        tasks.AddRange(audioVideosList.Any()
            ? audioVideosList
            : new biz.rebel_wings.Models.Dashboard.Task[]
            {
                new biz.rebel_wings.Models.Dashboard.Task()
                {
                    Date = date,
                    Detail = 0,
                    Status = 0,
                    Name = "Audio y Video",
                    Regional = "",
                    PercentageComplete = 0
                }
            });

        var spotlightsList = _context.Spotlights.Where(a => a.BranchId == id && a.CreatedDate >= date && a.CreatedDate <= dateEnd)
            .Select(s => new biz.rebel_wings.Models.Dashboard.Task
            {
                Date = s.CreatedDate,
                Detail = s.Id,
                Name = "Focos",
                Regional = $"{_context.Users.First(f => f.Id == s.CreatedBy).Name}",
                Status = 1,
                PercentageComplete = 100
            }).ToList();
        tasks.AddRange(spotlightsList.Any()
            ? spotlightsList
            : new biz.rebel_wings.Models.Dashboard.Task[]
            {
                new biz.rebel_wings.Models.Dashboard.Task()
                {
                    Date = date,
                    Detail = 0,
                    Status = 0,
                    Name = "Audio y Video",
                    Regional = "",
                    PercentageComplete = 0
                }
            });

        var barCleaningsList = _context.BarCleanings.Where(a => a.BranchId == id && a.CreatedDate >= date && a.CreatedDate <= dateEnd)
            .Select(s => new biz.rebel_wings.Models.Dashboard.Task
            {
                Date = s.CreatedDate,
                Detail = s.Id,
                Name = "Limpieza en barra",
                Regional = $"{_context.Users.First(f => f.Id == s.CreatedBy).Name}",
                Status = 1,
                PercentageComplete = 100
            }).ToList();
        tasks.AddRange(barCleaningsList.Any()
            ? barCleaningsList
            : new biz.rebel_wings.Models.Dashboard.Task[]
            {
                new biz.rebel_wings.Models.Dashboard.Task()
                {
                    Date = date,
                    Detail = 0,
                    Status = 0,
                    Name = "Limpieza en barra",
                    Regional = "",
                    PercentageComplete = 0
                }
            });

        var fridgeSalonsList = _context.FridgeSalons.Where(a => a.BranchId == id && a.CreatedDate >= date && a.CreatedDate <= dateEnd)
            .Select(s => new biz.rebel_wings.Models.Dashboard.Task
            {
                Date = s.CreatedDate,
                Detail = s.Id,
                Name = "Refrigeradores",
                Regional = $"{_context.Users.First(f => f.Id == s.CreatedBy).Name}",
                Status = 1,
                PercentageComplete = 100
            }).ToList();
        tasks.AddRange(fridgeSalonsList.Any()
            ? fridgeSalonsList
            : new biz.rebel_wings.Models.Dashboard.Task[]
            {
                new biz.rebel_wings.Models.Dashboard.Task()
                {
                    Date = date,
                    Detail = 0,
                    Status = 0,
                    Name = "Refrigeradores",
                    Regional = "",
                    PercentageComplete = 0
                }
            });
        dashboardRegional.TasksSalonCollection = tasks.OrderBy(o=>o.Date).ToList();
        tasks.Clear();

        #endregion

        #region Tasks Bathrooms

        var bathRoomsOverallStatusesList = _context.BathRoomsOverallStatuses
            .Where(a => a.BranchId == id && a.CreatedDate >= date && a.CreatedDate <= dateEnd)
            .Select(s => new biz.rebel_wings.Models.Dashboard.Task
            {
                Date = s.CreatedDate,
                Detail = s.Id,
                Name = "Estado General",
                Regional = $"{_context.Users.First(f => f.Id == s.CreatedBy).Name}",
                Status = 1,
                PercentageComplete = 100
            }).ToList();
        tasks.AddRange(bathRoomsOverallStatusesList.Any()
            ? bathRoomsOverallStatusesList
            : new biz.rebel_wings.Models.Dashboard.Task[]
            {
                new biz.rebel_wings.Models.Dashboard.Task()
                {
                    Date = date,
                    Detail = 0,
                    Status = 0,
                    Name = "Estado General",
                    Regional = "",
                    PercentageComplete = 0
                }
            });

        var washBasinSoapPapersList = _context.WashBasinWithSoapPapers
            .Where(a => a.BranchId == id && a.CreatedDate >= date && a.CreatedDate <= dateEnd)
            .Select(s => new biz.rebel_wings.Models.Dashboard.Task
            {
                Date = s.CreatedDate,
                Detail = s.Id,
                Name = "Lavabos con jabón y papel",
                Regional = $"{_context.Users.First(f => f.Id == s.CreatedBy).Name}",
                Status = 1,
                PercentageComplete = 100
            }).ToList();
        tasks.AddRange(washBasinSoapPapersList.Any()
            ? washBasinSoapPapersList
            : new biz.rebel_wings.Models.Dashboard.Task[]
            {
                new biz.rebel_wings.Models.Dashboard.Task()
                {
                    Date = date,
                    Detail = 0,
                    Status = 0,
                    Name = "Lavabos con jabón y papel",
                    Regional = "",
                    PercentageComplete = 0
                }
            });
        dashboardRegional.TasksBathroomsCollection = tasks.OrderBy(o=>o.Date).ToList();
        tasks.Clear();

        #endregion

        #region Task Systems

        var ticketTablesList = _context.TicketTables
            .Where(a => a.BranchId == id && a.CreatedDate >= date && a.CreatedDate <= dateEnd)
            .Select(s => new biz.rebel_wings.Models.Dashboard.Task
            {
                Date = s.CreatedDate,
                Detail = s.Id,
                Name = "Ticket y Mesa",
                Regional = $"{_context.Users.First(f => f.Id == s.CreatedBy).Name}",
                Status = 1,
                PercentageComplete = 100
            }).ToList();
        tasks.AddRange(ticketTablesList.Any()
            ? ticketTablesList
            : new biz.rebel_wings.Models.Dashboard.Task[]
            {
                new biz.rebel_wings.Models.Dashboard.Task()
                {
                    Date = date,
                    Detail = 0,
                    Status = 0,
                    Name = "Ticket y Mesa",
                    Regional = "",
                    PercentageComplete = 0
                }
            });

        var entriesChargedAsDeliveryNotesList = _context.EntriesChargedAsDeliveryNotes
            .Where(a => a.BranchId == id && a.CreatedDate >= date && a.CreatedDate <= dateEnd)
            .Select(s => new biz.rebel_wings.Models.Dashboard.Task
            {
                Date = s.CreatedDate,
                Detail = s.Id,
                Name = "ENTRADAS CARGADAS COMO ALBARÁN ",
                Regional = $"{_context.Users.First(f => f.Id == s.CreatedBy).Name}",
                Status = 1,
                PercentageComplete = 100
            }).ToList();
        tasks.AddRange(entriesChargedAsDeliveryNotesList.Any()
            ? entriesChargedAsDeliveryNotesList
            : new biz.rebel_wings.Models.Dashboard.Task[]
            {
                new biz.rebel_wings.Models.Dashboard.Task()
                {
                    Date = date,
                    Detail = 0,
                    Status = 0,
                    Name = "ENTRADAS CARGADAS COMO ALBARÁN ",
                    Regional = "",
                    PercentageComplete = 0
                }
            });

        var orderScheduleReviewsList = _context.OrderScheduleReviews
            .Where(a => a.BranchId == id && a.CreatedDate >= date && a.CreatedDate <= dateEnd)
            .Select(s => new biz.rebel_wings.Models.Dashboard.Task
            {
                Date = s.CreatedDate,
                Detail = s.Id,
                Name = "Revisión de Pedido vs calendario",
                Regional = $"{_context.Users.First(f => f.Id == s.CreatedBy).Name}",
                Status = 1,
                PercentageComplete = 100
            }).ToList();
        tasks.AddRange(orderScheduleReviewsList.Any()
            ? orderScheduleReviewsList
            : new biz.rebel_wings.Models.Dashboard.Task[]
            {
                new biz.rebel_wings.Models.Dashboard.Task()
                {
                    Date = date,
                    Detail = 0,
                    Status = 0,
                    Name = "Revisión de Pedido vs calendario",
                    Regional = "",
                    PercentageComplete = 0
                }
            });

        var checkTablesList = _context.CheckTables
            .Where(a => a.BranchId == id && a.CreatedDate >= date && a.CreatedDate <= dateEnd)
            .Select(s => new biz.rebel_wings.Models.Dashboard.Task
            {
                Date = s.CreatedDate,
                Detail = s.Id,
                Name = "Revisión de Mesas",
                Regional = $"{_context.Users.First(f => f.Id == s.CreatedBy).Name}",
                Status = 1,
                PercentageComplete = 100
            }).ToList();
        tasks.AddRange(checkTablesList.Any()
            ? checkTablesList
            : new biz.rebel_wings.Models.Dashboard.Task[]
            {
                new biz.rebel_wings.Models.Dashboard.Task()
                {
                    Date = date,
                    Detail = 0,
                    Status = 0,
                    Name = "Revisión de Mesas",
                    Regional = "",
                    PercentageComplete = 0
                }
            });
        dashboardRegional.TasksSystemCollection = tasks.OrderBy(o=>o.Date).ToList();
        tasks.Clear();

        #endregion

        #region Maintenance

        var kitchensList = _context.Kitchens
            .Where(a => a.BranchId == id && a.CreatedDate >= date && a.CreatedDate <= dateEnd)
            .Select(s => new biz.rebel_wings.Models.Dashboard.Task
            {
                Date = s.CreatedDate,
                Detail = s.Id,
                Name = "Cocina",
                Regional = $"{_context.Users.First(f => f.Id == s.CreatedBy).Name}",
                Status = 1,
                PercentageComplete = 100
            }).ToList();
        tasks.AddRange(kitchensList.Any()
            ? kitchensList
            : new biz.rebel_wings.Models.Dashboard.Task[]
            {
                new biz.rebel_wings.Models.Dashboard.Task()
                {
                    Date = date,
                    Detail = 0,
                    Status = 0,
                    Name = "Cocina",
                    Regional = "",
                    PercentageComplete = 0
                }
            });

        var salonsList = _context.Salons.Where(a => a.BranchId == id && a.CreatedDate >= date && a.CreatedDate <= dateEnd)
            .Select(s => new biz.rebel_wings.Models.Dashboard.Task
            {
                Date = s.CreatedDate,
                Detail = s.Id,
                Name = "Salon",
                Regional = $"{_context.Users.First(f => f.Id == s.CreatedBy).Name}",
                Status = 1,
                PercentageComplete = 100
            }).ToList();
        tasks.AddRange(salonsList.Any()
            ? salonsList
            : new biz.rebel_wings.Models.Dashboard.Task[]
            {
                new biz.rebel_wings.Models.Dashboard.Task()
                {
                    Date = date,
                    Detail = 0,
                    Status = 0,
                    Name = "Salon",
                    Regional = "",
                    PercentageComplete = 0
                }
            });

        var bathroomsList = _context.Bathrooms.Where(a => a.BranchId == id && a.CreatedDate >= date && a.CreatedDate <= dateEnd)
            .Select(s => new biz.rebel_wings.Models.Dashboard.Task
            {
                Date = s.CreatedDate,
                Detail = s.Id,
                Name = "Baños",
                Regional = $"{_context.Users.First(f => f.Id == s.CreatedBy).Name}",
                Status = 1,
                PercentageComplete = 100
            }).ToList();
        tasks.AddRange(bathroomsList.Any()
            ? bathroomsList
            : new biz.rebel_wings.Models.Dashboard.Task[]
            {
                new biz.rebel_wings.Models.Dashboard.Task()
                {
                    Date = date,
                    Detail = 0,
                    Status = 0,
                    Name = "Baños",
                    Regional = "",
                    PercentageComplete = 0
                }
            });

        var barsList = _context.Bars.Where(a => a.BranchId == id && a.CreatedDate >= date && a.CreatedDate <= dateEnd)
            .Select(s => new biz.rebel_wings.Models.Dashboard.Task
            {
                Date = s.CreatedDate,
                Detail = s.Id,
                Name = "Barra",
                Regional = $"{_context.Users.First(f => f.Id == s.CreatedBy).Name}",
                Status = 1,
                PercentageComplete = 100
            }).ToList();
        tasks.AddRange(barsList.Any()
            ? barsList
            : new biz.rebel_wings.Models.Dashboard.Task[]
            {
                new biz.rebel_wings.Models.Dashboard.Task()
                {
                    Date = date,
                    Detail = 0,
                    Status = 0,
                    Name = "Barra",
                    Regional = "",
                    PercentageComplete = 0
                }
            });
        
        dashboardRegional.TasksMaintenanceCollection = tasks.OrderBy(o=>o.Date).ToList();
        tasks.Clear();

        #endregion

        #endregion

        return dashboardRegional;
    }

    public DashboardAssistance GetAssistance(int id, DateTime date, DateTime dateEnd)
    {
        var dashboardAssistance = new DashboardAssistance();
        var dateMiddle = date.AddHours(12);
        #region Percentange Assistance

        var assistanceTotals = _context.ValidateAttendances.Count(c =>
            c.BranchId == id && c.Time >= date && c.Time <= dateEnd && (c.Attendence == 1 || c.Attendence == 3));
        var totals = _context.ValidateAttendances.Count(c =>
            c.BranchId == id && c.Time >= date && c.Time <= dateEnd);
        dashboardAssistance.PercentageAssistance = totals != 0 ? (decimal)assistanceTotals * 100 / totals : 0;

        #endregion

        #region Delays

        dashboardAssistance.Delays = _context.ValidateAttendances.Count(c =>
            c.BranchId == id && c.Time >= date && c.Time <= dateEnd && c.Attendence == 3);

        #endregion

        #region Absences

        dashboardAssistance.Absences = _context.ValidateAttendances.Count(c =>
            c.BranchId == id && c.Time >= date && c.Time <= dateEnd && c.Attendence == 2);

        #endregion

        #region Assitances

        dashboardAssistance.AssistanceMorningsCollection = _context.ValidateAttendances
            .Where(c => c.BranchId == id && c.Time >= date && c.Time <= dateMiddle).Select(s =>
                new Assistance
                {
                    Name = s.ClabTrab.ToString(),
                    JobTittle = "",
                    Date = s.CreatedDate,
                    TimeArrive = s.AttendenceNavigation.Status,
                    AssistanceStatus = s.Attendence,
                    Detail = s.Id
                }).ToList();
        dashboardAssistance.AssistanceEveningsCollection = _context.ValidateAttendances
            .Where(c => c.BranchId == id && c.Time >= dateMiddle && c.Time <= dateEnd).Select(s =>
                new Assistance
                {
                    Name = s.ClabTrab.ToString(),
                    JobTittle = "",
                    Date = s.CreatedDate,
                    TimeArrive = s.AttendenceNavigation.Status,
                    AssistanceStatus = s.Attendence,
                    Detail = s.Id
                }).ToList();

        #endregion
        return dashboardAssistance;
    }
}