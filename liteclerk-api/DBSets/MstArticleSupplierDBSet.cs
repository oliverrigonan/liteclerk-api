﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DBSets
{
    public class MstArticleSupplierDBSet
    {
        public Int32 Id { get; set; }

        public Int32 ArticleId { get; set; }
        public virtual MstArticleDBSet MstArticle_ArticleId { get; set; }

        public String Supplier { get; set; }
        public String Address { get; set; }
        public String ContactPerson { get; set; }
        public String ContactNumber { get; set; }

        public Int32 PayableAccountId { get; set; }
        public virtual MstAccountDBSet MstAccount_PayableAccountId { get; set; }

        public Int32 TermId { get; set; }
        public virtual MstTermDBSet MstTerm_TermId { get; set; }
    }
}
