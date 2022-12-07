using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace biz.rebel_wings.Models.StockChicken
{
    public class StockChickenGet
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public decimal Amount { get; set; }
        public string Status { get; set; }
        public DateTime Created { get; set; }
    }
}
