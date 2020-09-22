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
        public virtual MstArticleDBSet MstArticle_ArticleId { get; set; }
        public String SKUCode { get; set; }
        public String BarCode { get; set; }
        public String Description { get; set; }
        public Int32 UnitId { get; set; }
        public virtual MstUnitDBSet MstUnit_UnitId { get; set; }
        public String Category { get; set; }
        public Boolean IsInventory { get; set; }
        public Int32 ArticleAccountGroupId { get; set; }
        public virtual MstArticleAccountGroupDBSet MstArticleAccountGroup_ArticleAccountGroupId { get; set; }
        public Int32 AssetAccountId { get; set; }
        public virtual MstAccountDBSet MstAccount_AssetAccountId { get; set; }
        public Int32 SalesAccountId { get; set; }
        public virtual MstAccountDBSet MstAccount_SalesAccountId { get; set; }
        public Int32 CostAccountId { get; set; }
        public virtual MstAccountDBSet MstAccount_CostAccountId { get; set; }
        public Int32 ExpenseAccountId { get; set; }
        public virtual MstAccountDBSet MstAccount_ExpenseAccountId { get; set; }
        public Decimal Price { get; set; }
        public Int32 RRVATId { get; set; }
        public virtual MstTaxDBSet MstTax_RRVATId { get; set; }
        public Int32 SIVATId { get; set; }
        public virtual MstTaxDBSet MstTax_SIVATId { get; set; }
        public Int32 WTAXId { get; set; }
        public virtual MstTaxDBSet MstTax_WTAXId { get; set; }
        public String Kitting { get; set; }
    }
}
