﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DTO
{
    public class MstArticleDTO
    {
        public Int32 Id { get; set; }

        public String ArticleCode { get; set; }
        public String ManualCode { get; set; }
        public String Article { get; set; }

        public Int32 ArticleTypeId { get; set; }
        public MstArticleTypeDTO ArticleType { get; set; }

        public String ImageURL { get; set; }
        public String Particulars { get; set; }

        public Boolean IsLocked { get; set; }

        public MstUserDTO CreatedByUser { get; set; }
        public String CreatedDateTime { get; set; }
        public MstUserDTO UpdatedByUser { get; set; }
        public String UpdatedDateTime { get; set; }
    }
}
