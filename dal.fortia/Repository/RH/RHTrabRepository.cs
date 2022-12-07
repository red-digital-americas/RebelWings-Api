using biz.fortia.Entities;
using biz.fortia.Models;
using biz.fortia.Repository.RH;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dal.fortia.Repository.RH
{
    public class RHTrabRepository : Generic.GenericRepository<RhTrab>, IRHTrabRepository
    {
        public RHTrabRepository(DBContext.BDFORTIAContext context) : base(context)
        {

        }

        public List<AttendanceList> GetAttendanceLists(int branch)
        {
            DateTime date = DateTime.Now.AddDays(-1);
            var list = (from t in _context.RhTrabs 
                        join s in _context.RhRegentsals on t.ClaTrab equals s.ClaTrab
                        join r in _context.RhRelojs on s.ClaReloj equals r.ClaReloj
                        join p in _context.RhPuestos on t.ClaPuesto equals p.ClaPuesto
                        join ct in _context.RhTurnos on t.ClaTurno equals ct.ClaTurno
                        where r.ClaReloj == branch && s.FechaEntsal > date
                        select new AttendanceList
                        {
                            FullName = $"{t.NomTrab} {t.ApPaterno} {t.ApPaterno}",
                            Id = t.ClaTrab,
                            JobTitle = p.NomPuesto,
                            Workshift = GetWorkshift(ct.HoraEnt1.GetValueOrDefault()),
                            Avatar = ""
                        }).ToList();
            return list;
        }

        public List<TransferList> GetBranchList()
        {
            var branchList = _context.RhRelojs.Select(s => new TransferList()
            {
                Name = s.NomReloj,
                Description = s.Mensaje,
                BranchId = s.ClaReloj,
            }).ToList();
            return branchList;
        }

        private static string GetWorkshift(double cla_turno)
        {
            string dayTime = "";
            var digits = cla_turno.ToString().Select(x => int.Parse(x.ToString())).ToArray();
            int l = digits.Count();
            if (l == 3)
            {
                dayTime = $"0{digits[0].ToString()}:{digits[1].ToString()}{digits[2].ToString()}";
            }
            else if (l == 4)
            {
                dayTime = $"{digits[0].ToString()}{digits[1].ToString()}:{digits[2].ToString()}{digits[3].ToString()}";
            }else if (l == 1)
            {
                dayTime = "00:00";
            }
            TimeSpan time = TimeSpan.Parse(dayTime);
            DateTime fullDateTime = DateTime.Today.Add(time);
            int workshift = fullDateTime.TimeOfDay.Hours;
            //Console.WriteLine(fullDateTime.ToString("hh:mm tt"));
            //Console.WriteLine(workshift);

            return fullDateTime.Hour > 12 ? "Turno Vespertino" : "Turno Matutino";
        }

        public string GetBranchNameById(int branchId)
        {
            string s = _context.RhRelojs.First(f => f.ClaReloj == branchId).NomReloj;

            return s;
        }

        public int GetBranchId(int clab_trab)
        {
            int a = _context.RhRegentsals.Where(x=>x.ClaTrab==clab_trab).OrderByDescending(o=>o.FechaEntsal).First().ClaReloj;
            return a;
        }

        public string GetBranchName(int clab_trab)
        {
            int a = _context.RhRegentsals.Where(x => x.ClaTrab == clab_trab).OrderByDescending(o => o.FechaEntsal).First().ClaReloj;
            string s = _context.RhRelojs.First(f => f.ClaReloj == a).NomReloj;

            return s;
        }
    }
}
