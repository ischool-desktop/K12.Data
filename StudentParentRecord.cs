using System;
using System.Collections.Generic;
using System.Xml;
using System.Data;

namespace K12.Data
{
    /// <summary>
    /// 家長資訊(student_parent_info)。
    /// </summary>
    public class StuentParentRecord
    {
        /// <summary>
        /// 系統編號
        /// </summary>
        [Field(Caption = "系統編號", EntityName = "StuentParent", EntityCaption = "家長", IsEntityPrimaryKey = true)]
        public string ID { get; set; }
        /// <summary>
        /// 登入帳號
        /// </summary>
        [Field(Caption = "登入帳號", EntityName = "StuentParent", EntityCaption = "家長")]
        public string Account { get; set; }
        /// <summary>
        /// 家長姓名
        /// </summary>
        [Field(Caption = "家長姓名", EntityName = "StuentParent", EntityCaption = "家長")]
        public string Name { get; set; }
        /// <summary>
        /// 行動電話
        /// </summary>
        [Field(Caption = "行動電話", EntityName = "StuentParent", EntityCaption = "家長")]
        public string CellPhone { get; set; }
        /// <summary>
        /// 電子郵件
        /// </summary>
        [Field(Caption = "電子郵件", EntityName = "StuentParent", EntityCaption = "家長")]
        public string Email { get; set; }
        /// <summary>
        /// 自定資料
        /// </summary>
        [Field(Caption = "自定資料", EntityName = "StuentParent", EntityCaption = "家長")]
        public string Extension { get; set; }

        /// <summary>
        /// 無參數建構式
        /// </summary>
        public StuentParentRecord()
        {
            Account = "";
        }

        /// <summary>
        /// 新增家長資料建構式，參數為新增記錄的必填欄位。
        /// </summary>
        /// <param name="Name">班級名稱</param>
        public StuentParentRecord(string account)
        {
            this.Account = account;
        }

        /// <summary>
        /// Xml參數建構式
        /// </summary>
        /// <param name="row"></param>
        public StuentParentRecord(DataRow row)
        {
            Load(row);
        }

        /// <summary>
        /// 載入XML方法
        /// </summary>
        /// <param name="row"></param>
        internal void Load(DataRow row)
        {
            ID = row["id"] + "";
            Account = row["account"] + "";
            Name = row["name"] + "";
            CellPhone = row["cell_phone"] + "";
            Email = row["email"] + "";
            Extension = row["extension"] + "";
        }
    }
}