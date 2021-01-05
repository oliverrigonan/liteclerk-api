using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DBSets
{
    public class TrnStockInItemDBSet
    {
        public Int32 Id { get; set; }

        public Int32 INId { get; set; }
        public virtual TrnStockInDBSet TrnStockIn_INId { get; set; }

        public Int32? JOId { get; set; }
        public virtual TrnJobOrderDBSet TrnJobOrder_JOId { get; set; }

        public Int32 ItemId { get; set; }
        public virtual MstArticleDBSet MstArticle_ItemId { get; set; }

        public String Particulars { get; set; }

        public Decimal Quantity { get; set; }

        public Int32 UnitId { get; set; }
        public virtual MstUnitDBSet MstUnit_UnitId { get; set; }

        public Decimal Cost { get; set; }
        public Decimal Amount { get; set; }

        public Decimal BaseQuantity { get; set; }
        public Int32 BaseUnitId { get; set; }
        public virtual MstUnitDBSet MstUnit_BaseUnitId { get; set; }
        public Decimal BaseCost { get; set; }
    }
}
