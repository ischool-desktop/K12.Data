using System;
using System.Xml;

namespace K12.Data
{
    /// <summary>
    /// 教師資訊
    /// </summary>
    public class TeacherRecord : IComparable<TeacherRecord>
    {
        private string mTAPassword;

        public enum TeacherStatus
        {
            一般,刪除
        }

        /// <summary>
        /// 系統編號
        /// </summary>
        [Field(Caption = "編號", EntityName = "Teacher",EntityCaption="教師",IsEntityPrimaryKey=true)]
        public string ID { get;  set; }
        /// <summary>
        /// 姓名
        /// </summary>
        [Field(Caption = "名稱", EntityName = "Teacher", EntityCaption = "教師")]
        public string Name { get;  set; }
        /// <summary>
        /// 暱稱
        /// </summary>
        [Field(Caption = "暱稱", EntityName = "Teacher", EntityCaption = "教師")]
        public string Nickname { get;  set; }
        /// <summary>
        /// 狀態
        /// </summary>
        public TeacherStatus Status { get; set; }
        /// <summary>
        /// 狀態字串，若要設定請使用Status屬性
        /// </summary>
        [Field(Caption = "狀態", EntityName = "Teacher", EntityCaption = "教師")]
        public string StatusStr { get { return Status.ToString(); } }
        /// <summary>
        /// 姓別
        /// </summary>
        [Field(Caption = "性別", EntityName = "Teacher", EntityCaption = "教師")]
        public string Gender { get;  set; }
        /// <summary>
        /// 身份證號
        /// </summary>
        [Field(Caption = "身份證號", EntityName = "Teacher", EntityCaption = "教師")]
        public string IDNumber { get;  set; }
        /// <summary>
        /// 連絡電話
        /// </summary>
        [Field(Caption = "連絡電話", EntityName = "Teacher", EntityCaption = "教師")]
        public string ContactPhone { get;  set; }
        /// <summary>
        /// 分類
        /// </summary>
        [Field(Caption = "分類", EntityName = "Teacher", EntityCaption = "教師")]
        public string Category { get;  set; }

        /// <summary>
        /// Teacher Access的登入名稱
        /// </summary>
        [Field(Caption = "登入名稱", EntityName = "Teacher", EntityCaption = "教師")]
        public string TALoginName { get; set; }

        /// <summary>
        /// Teacher Access的登入密碼
        /// </summary>
        [Field(Caption = "登入密碼", EntityName = "Teacher", EntityCaption = "教師")]
        public string TAPassword 
        {
            get { return mTAPassword; }
            set { mTAPassword = string.IsNullOrEmpty(value) ? value : K12.Data.Utility.PasswordHash.Compute(value); }
        }

        /// <summary>
        /// 帳號類型
        /// </summary>
        [Field(Caption = "帳號類型", EntityName = "Teacher", EntityCaption = "教師")]
        public string AccountType { get; set; }

        /// <summary>
        /// 電子郵件
        /// </summary>
        [Field(Caption = "電子郵件", EntityName = "Teacher", EntityCaption = "教師")]
        public string Email { get; set; }

        /// <summary>
        /// 照片
        /// </summary>
        [Field(Caption = "照片", EntityName = "Teacher", EntityCaption = "教師")]
        public string Photo { get; set; }

        public TeacherRecord()
        {
            ID = "";
            Name = "";
            Status = TeacherStatus.一般;
        }

        public TeacherRecord(string Name) : this()
        {
            this.Name = Name;
        }

        public TeacherRecord(XmlElement element)
        {
            Load(element);
        }

        public void Load(XmlElement element)
        {
            XmlHelper helper = new XmlHelper(element);
            ID = helper.GetString("@ID");
            Name = helper.GetString("TeacherName");
            Nickname = helper.GetString("Nickname");
            Gender = helper.GetString("Gender");
            IDNumber = helper.GetString("IDNumber");
            ContactPhone = helper.GetString("ContactPhone");
            Category = helper.GetString("Category");
            TALoginName = helper.GetString("TALoginName");
            mTAPassword = helper.GetString("TAPassword"); //用屬性設定密碼會加密，但是用Service取得的已是加密的，所以直接設定私有欄位
            Email = helper.GetString("Email");
            AccountType = helper.GetString("AccountType");
            Photo = helper.GetString("Photo");

            string strStatus = helper.GetString("Status");

            if (strStatus.Equals("一般"))
                Status = TeacherStatus.一般;
            else if (strStatus.Equals("刪除"))
                Status = TeacherStatus.刪除;

        }

        #region IComparable<TeacherRecord> 成員

        public static event EventHandler<CompareTeacherRecordEventArgs> CompareTeacherRecord;

        public int CompareTo(TeacherRecord other)
        {
            if ( CompareTeacherRecord != null )
            {
                CompareTeacherRecordEventArgs args = new CompareTeacherRecordEventArgs(this, other);
                CompareTeacherRecord(null, args);
                return args.Result;
            }
            else
            {
                if ( this.ID.Length == other.ID.Length )
                    return this.ID.CompareTo(other.ID);
                else
                    return this.ID.Length.CompareTo(other.ID.Length);
            }
        }

        #endregion
    }

    public class CompareTeacherRecordEventArgs : EventArgs
    {
        internal CompareTeacherRecordEventArgs(TeacherRecord v1, TeacherRecord v2)
        {
            Value1 = v1;
            Value2 = v2;
        }
        public TeacherRecord Value1 { get; private set; }
        public TeacherRecord Value2 { get; private set; }
        public int Result { get; set; }
    }
}