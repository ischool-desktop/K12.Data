using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace K12.Data
{
    /// <summary>
    /// 標示在Vlaue Object上的屬性Attribute
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class FieldAttribute : Attribute
    {
        /// <summary>
        /// 無參數建構式
        /// </summary>
        public FieldAttribute() 
        {
            Caption = "";
            EntityName = "";
            EntityCaption = "";
            Remark = "";
            IsEntityPrimaryKey = false;
        }

        /// <summary>
        /// 顯示名稱
        /// </summary>
        public string Caption { get; set; }

        /// <summary>
        /// 所屬Entity，例如Student、Class、Teacher、Course
        /// </summary>
        public string EntityName { get; set; }

        /// <summary>
        /// 所屬Entity顯示名稱，例如學生、班級、教師、課程
        /// </summary>
        public string EntityCaption { get; set; }

        /// <summary>
        /// 所屬的Entity中是否為主鍵
        /// </summary>
        public bool IsEntityPrimaryKey { get; set; }

        /// <summary>
        /// 備註資訊
        /// </summary>
        public string Remark { get; set; }
    }
}