using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DBSets
{
    public class MstArticleItemDBSet
    {
        public Int32 Id { get; set; }
        public Int32 ArticleId { get; set; }
        public virtual MstArticleDBSet MstArticle_Article { get; set; }
        public String SKUCode { get; set; }
        public String BarCode { get; set; }
        public String Description { get; set; }
        public Int32 UnitId { get; set; }
        public virtual MstUnitDBSet MstUnit_Unit { get; set; }
        public Boolean IsInventory { get; set; }
        public Int32 ArticleAccountGroupId { get; set; }
        public virtual MstArticleAccountGroupDBSet MstArticleAccountGroup_ArticleAccountGroup { get; set; }
        public Int32 AssetAccountId { get; set; }
        public virtual MstAccountDBSet MstAccount_AssetAccount { get; set; }
        public Int32 SalesAccountId { get; set; }
        public virtual MstAccountDBSet MstAccount_SalesAccount { get; set; }
        public Int32 CostAccountId { get; set; }
        public virtual MstAccountDBSet MstAccount_CostAccount { get; set; }
        public Int32 ExpenseAccountId { get; set; }
        public virtual MstAccountDBSet MstAccount_ExpenseAccount { get; set; }
        public Decimal Price { get; set; }
        public Int32 RRVATId { get; set; }
        public virtual MstTaxDBSet MstTax_RRVAT { get; set; }
        public Int32 SIVATId { get; set; }
        public virtual MstTaxDBSet MstTax_SIVAT { get; set; }
        public Int32 WTAXId { get; set; }
        public virtual MstTaxDBSet MstTax_WTAX { get; set; }
        public String Kitting { get; set; }
    }
}
