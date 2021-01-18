using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DTO
{
    public class MstArticleItemDTO
    {
        public Int32 Id { get; set; }
        public Int32 ArticleId { get; set; }
        public MstArticleDTO Article { get; set; }
        public String ArticleManualCode { get; set; }
        public String ArticleParticulars { get; set; }
        public String SKUCode { get; set; }
        public String BarCode { get; set; }
        public String Description { get; set; }
        public Int32 UnitId { get; set; }
        public MstUnitDTO Unit { get; set; }
        public String Category { get; set; }
        public Boolean IsInventory { get; set; }
        public Int32 ArticleAccountGroupId { get; set; }
        public MstArticleAccountGroupDTO ArticleAccountGroup { get; set; }
        public Int32 AssetAccountId { get; set; }
        public MstAccountDTO AssetAccount { get; set; }
        public Int32 SalesAccountId { get; set; }
        public MstAccountDTO SalesAccount { get; set; }
        public Int32 CostAccountId { get; set; }
        public MstAccountDTO CostAccount { get; set; }
        public Int32 ExpenseAccountId { get; set; }
        public MstAccountDTO ExpenseAccount { get; set; }
        public Decimal Price { get; set; }
        public Int32 RRVATId { get; set; }
        public MstTaxDTO RRVAT { get; set; }
        public Int32 SIVATId { get; set; }
        public MstTaxDTO SIVAT { get; set; }
        public Int32 WTAXId { get; set; }
        public MstTaxDTO WTAX { get; set; }
        public String Kitting { get; set; }
        public Decimal ProductionCost { get; set; }
        public Boolean IsLocked { get; set; }
        public MstUserDTO CreatedByUser { get; set; }
        public String CreatedDateTime { get; set; }
        public MstUserDTO UpdatedByUser { get; set; }
        public String UpdatedDateTime { get; set; }
    }
}
