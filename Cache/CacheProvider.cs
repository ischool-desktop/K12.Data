
namespace K12.Data
{
    public class CacheManager
    {
        private static ICacheProvider mDiskCacheProvider = null;

        public static ICacheProvider DiskCacheProvider
        {
            get
            {
                if (mDiskCacheProvider == null)
                    mDiskCacheProvider = new DiskCacheProvider();

                return mDiskCacheProvider;
            }
        }
    }
}