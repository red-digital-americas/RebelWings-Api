using biz.rebel_wings.Repository.Generic;
using biz.rebel_wings.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace biz.rebel_wings.Repository.User
{
    public interface IUserRepository : IGenericRepository<Entities.User>
    {
        string HashPassword(string password);
        bool VerifyPassword(string hash, string password);
        string BuildToken(Entities.User user);
        string VerifyEmail(string email);
        string SendMail(string emailTo, string body, string subject);
        List<Entities.CatState> CatStateList();
    }
}
