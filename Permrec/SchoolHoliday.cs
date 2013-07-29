using K12.Data.Configuration;

namespace K12.Data
{
    /// <summary>
    /// 學校放假日類別，用來取得與設定學校的放假日資訊
    /// </summary>
    public class SchoolHoliday
    {
        private static string SchoolHodidayConfigString = "SCHOOL_HOLIDAY_CONFIG_STRING";
        private static string configString = "CONFIG_STRING";

        /// <summary>
        /// 取得學校放假日的組態值
        /// </summary>
        /// <returns></returns>
        [SelectMethod("K12.SchoolHoliday.Select", "學籍.學校放假")]
        public static SchoolHolidayRecord SelectSchoolHolidayRecord()
        {
            SchoolHolidayRecord result = null;
            ConfigData cd = School.Configuration[SchoolHoliday.SchoolHodidayConfigString];

            string xmlContent = cd[configString];

            if (xmlContent != "")
                result = new SchoolHolidayRecord(xmlContent);

            return result;
        }

        /// <summary>
        /// 設定學校放假日的組態值
        /// </summary>
        /// <param name="schoolHolidayRecord"></param>
        public static void SetSchoolHolidayRecord(SchoolHolidayRecord schoolHolidayRecord)
        {
            if (schoolHolidayRecord == null)
                return;

            ConfigData cd = School.Configuration[SchoolHodidayConfigString];
            cd[configString] = schoolHolidayRecord.GetXmlString();

            cd.Save();
        }
    }
}