using System.Collections.Generic;

namespace K12.Data.Utility
{
    public class ActiveTableAdapter<T> : List<ActiveRecordAdapter<T>> where T : new()
    {
        public ActiveTableAdapter(List<T> valueObjects)
        {
            FieldNames = new List<string>();

            foreach (T valueObject in valueObjects)
            {
                ActiveRecordAdapter<T> ActiveRecord = new ActiveRecordAdapter<T>(valueObject);

                foreach (string FieldName in ActiveRecord.Fields)
                    if (!FieldNames.Contains(FieldName))
                        FieldNames.Add(FieldName);

                Add(ActiveRecord);
            }
        }

        public List<string> FieldNames { get; set; }
    }
}