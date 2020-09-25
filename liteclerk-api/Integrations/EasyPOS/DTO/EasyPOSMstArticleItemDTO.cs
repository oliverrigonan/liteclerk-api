using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.Integrations.EasyPOS.DTO
{
    public class EasyPOSMstArticleItemDTO
    {
        public Int32 Id { get; set; }
        public Int32 ArticleId { get; set; }
        public EasyPOSMstArticleDTO Article { get; set; }
        public String ArticleManualCode { get; set; }
        public String SKUCode { get; set; }
        public String BarCode { get; set; }
        public String Description { get; set; }
        public Int32 UnitId { get; set; }
        public EasyPOSMstUnitDTO Unit { get; set; }
        public String Category { get; set; }
        public Boolean IsInventory { get; set; }
        public Decimal Price { get; set; }
        public Int32 RRVATId { get; set; }
        public EasyPOSMstTaxDTO RRVAT { get; set; }
        public Int32 SIVATId { get; set; }
        public EasyPOSMstTaxDTO SIVAT { get; set; }
        public List<EasyPOSMstArticleItemPriceDTO> ArticleItemPrices { get; set; }
    }
}
