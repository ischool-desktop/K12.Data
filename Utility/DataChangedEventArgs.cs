using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace K12.Data
{
    public enum ChangedSource
    {
        Local, Remote
    }

    /// <summary>
    /// 提供Insert、Update、Delete事件的資料
    /// </summary>
    public class DataChangedEventArgs : EventArgs
    {
        public List<string> PrimaryKeys { get; private set; }
        public ChangedSource Source { get; private set; }

        public DataChangedEventArgs(IEnumerable<string> primaryKeys, ChangedSource source)
        {
            PrimaryKeys = new List<string>(primaryKeys);
            Source = source;
        }
    }
}