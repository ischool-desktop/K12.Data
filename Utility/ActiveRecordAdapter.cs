using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace K12.Data
{
    public class ActiveRecordAdapter<T> where T : new()
    {
        private Dictionary<string, string> mFieldValue;

        public ActiveRecordAdapter()
        {
            ValueObject = new T();
            mFieldValue = new Dictionary<string, string>();
        }

        public ActiveRecordAdapter(T valueObject)
        {
            ValueObject = valueObject;
            mFieldValue = new Dictionary<string, string>();

            InitialReflection();
        }

        public List<string> Fields
        {
            get
            {
                return mFieldValue.Keys.ToList();
            }
        }

        public List<string> Values
        {
            get
            {
                return mFieldValue.Values.ToList();
            }
        }

        public string this[string FieldName]
        {
            get
            {
                return mFieldValue[FieldName];
            }
            set
            {
                ValueObject.GetType().GetProperty(FieldName).SetValue(ValueObject, value, null);
                mFieldValue[FieldName] = value;
            }
        }

        public T ValueObject { get; set; }

        private void InitialReflection()
        {
            foreach (System.Reflection.PropertyInfo Property in ValueObject.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                if (Property.PropertyType == typeof(string))
                {
                    object FieldValue = Property.GetValue(ValueObject, null);

                    mFieldValue[Property.Name] = FieldValue == null ? "" : FieldValue.ToString();
                }
                else if (Property.PropertyType == typeof(StudentRecord.StudentStatus))
                {
                    object FieldValue = Property.GetValue(ValueObject, null);

                    mFieldValue[Property.Name] = FieldValue == null ? "" : FieldValue.ToString(); 
                }
            }
        }
    }
}