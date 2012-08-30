﻿using System;
using Com.Mobeelizer.Mobile.Wp7.Api;
using System.Data.Linq.Mapping;

namespace wp7_api_demos.Model.MobeelizerModels
{
    [Table]
    public class conflictsEntity : MobeelizerWp7Model
    {
        [Column(IsPrimaryKey = true)]
        public override String Guid { get; set; }

        [Column()]
        public override String Owner { get; set; }

        [Column()]
        public override bool Conflicted { get; set; }

        [Column()]
        public override bool Deleted { get; set; }

        [Column()]
        public override bool Modified { get; set; }

        [Column()]
        public int Score { get; set; }

        [Column()]
        public String Title { get; set; }
    }
}
