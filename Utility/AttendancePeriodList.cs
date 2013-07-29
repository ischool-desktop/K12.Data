using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace K12.Data
{
    public class AttendancePeriodList:IList<AttendancePeriod>
    {
        private List<AttendancePeriod> mAttendancePeriods;

        private void Sync()
        {
 
        }

        public  AttendancePeriodList(ref XmlElement data)
        {
            mAttendancePeriods = new List<AttendancePeriod>();
        }

        #region IList<AttendancePeriod> 成員

        public int IndexOf(AttendancePeriod item)
        {
            return mAttendancePeriods.IndexOf(item);
        }

        public void Insert(int index, AttendancePeriod item)
        {
            mAttendancePeriods.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            mAttendancePeriods.RemoveAt(index);
        }

        public AttendancePeriod this[int index]
        {
            get
            {
                return mAttendancePeriods[index];
            }
            set
            {
                mAttendancePeriods[index] = value;
            }
        }

        #endregion

        #region ICollection<AttendancePeriod> 成員

        public void Add(AttendancePeriod item)
        {
            mAttendancePeriods.Add(item);
        }

        public void Clear()
        {
            mAttendancePeriods.Clear();
        }

        public bool Contains(AttendancePeriod item)
        {
            return mAttendancePeriods.Contains(item);
        }

        public void CopyTo(AttendancePeriod[] array, int arrayIndex)
        {
            mAttendancePeriods.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return mAttendancePeriods.Count ; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(AttendancePeriod item)
        {
            return mAttendancePeriods.Remove(item);
        }

        #endregion

        #region IEnumerable<AttendancePeriod> 成員

        public IEnumerator<AttendancePeriod> GetEnumerator()
        {
            return mAttendancePeriods.GetEnumerator();
        }

        #endregion

        #region IEnumerable 成員

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return mAttendancePeriods.GetEnumerator();
        }

        #endregion
    }
}