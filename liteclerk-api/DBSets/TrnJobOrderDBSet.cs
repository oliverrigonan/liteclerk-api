﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DBSets
{
    public class TrnJobOrderDBSet
    {
        public Int32 Id { get; set; }

        public Int32 BranchId { get; set; }
        public virtual MstCompanyBranchDBSet MstCompanyBranch_BranchId { get; set; }

        public Int32 CurrencyId { get; set; }
        public virtual MstCurrencyDBSet MstCurrency_CurrencyId { get; set; }

        public String JONumber { get; set; }
        public DateTime JODate { get; set; }
        public String ManualNumber { get; set; }
        public String DocumentReference { get; set; }

        public DateTime DateScheduled { get; set; }
        public DateTime DateNeeded { get; set; }

        public Int32? SIId { get; set; }
        public virtual TrnSalesInvoiceDBSet TrnSalesInvoice_SIId { get; set; }

        public Int32? SIItemId { get; set; }
        public virtual TrnSalesInvoiceItemDBSet TrnSalesInvoiceItem_SIItemId { get; set; }

        public Int32 ItemId { get; set; }
        public virtual MstArticleDBSet MstArticle_ItemId { get; set; }

        public Int32 ItemJobTypeId { get; set; }
        public virtual MstJobTypeDBSet MstJobType_ItemJobTypeId { get; set; }

        public Decimal Quantity { get; set; }

        public Int32 UnitId { get; set; }
        public virtual MstUnitDBSet MstUnit_UnitId { get; set; }

        public String Remarks { get; set; }

        public Decimal BaseQuantity { get; set; }
        public Int32 BaseUnitId { get; set; }
        public virtual MstUnitDBSet MstUnit_BaseUnitId { get; set; }

        public String CurrentDepartment { get; set; }
        public String CurrentDepartmentStatus { get; set; }
        public String CurrentDepartmentUserFullName { get; set; }

        public Int32 PreparedByUserId { get; set; }
        public virtual MstUserDBSet MstUser_PreparedByUserId { get; set; }

        public Int32 CheckedByUserId { get; set; }
        public virtual MstUserDBSet MstUser_CheckedByUserId { get; set; }

        public Int32 ApprovedByUserId { get; set; }
        public virtual MstUserDBSet MstUser_ApprovedByUserId { get; set; }

        public String Status { get; set; }
        public Boolean IsCancelled { get; set; }
        public Boolean IsPrinted { get; set; }
        public Boolean IsLocked { get; set; }

        public Int32 CreatedByUserId { get; set; }
        public virtual MstUserDBSet MstUser_CreatedByUserId { get; set; }
        public DateTime CreatedDateTime { get; set; }

        public Int32 UpdatedByUserId { get; set; }
        public virtual MstUserDBSet MstUser_UpdatedByUserId { get; set; }
        public DateTime UpdatedDateTime { get; set; }

        public virtual ICollection<TrnJobOrderAttachmentDBSet> TrnJobOrderAttachments_JOId { get; set; }
        public virtual ICollection<TrnJobOrderInformationDBSet> TrnJobOrderInformations_JOId { get; set; }
        public virtual ICollection<TrnJobOrderDepartmentDBSet> TrnJobOrderDepartments_JOId { get; set; }
        public virtual ICollection<TrnStockInItemDBSet> TrnStockInItems_JOId { get; set; }
    }
}
