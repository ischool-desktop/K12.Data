using System.Collections.Generic;
using FISCA.Synchronization;

namespace K12.Data.Utility
{
    public class K12DBChangMonitor
    {
        private static DBChangeMonitor Monitor = null;
        private static IChangeSetProvider Provider = null;
        private static int mMonitorInterval = 5;

        public static DBChangeMonitor GetMonitor()
        {
            if (Monitor == null)
            {
                List<IChangeSetProvider> Providers = new List<IChangeSetProvider>();

                Provider = new K12.Data.Utility.PTChangeSetProvider();

                Providers.Add(Provider);

                Monitor = new FISCA.Synchronization.DBChangeMonitor(mMonitorInterval, Providers.ToArray());

                Monitor.SetBaseLine();
 
                Monitor.Start();
            }

            return Monitor;
        }

        public static void NotifyClientChangeEntries(IEnumerable<ChangeEntry> ChangeEntries)
        {
            if (Provider!=null)
                Provider.SetClientChangeSet(ChangeEntries);
        }
    }
}