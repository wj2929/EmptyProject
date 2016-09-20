namespace Microsoft.VisualStudio.TextTemplating.Modeling
{
    using Microsoft.VisualStudio.Modeling;
    using Microsoft.VisualStudio.TextTemplating.Modeling.Properties;
    using System;
    using System.Collections.Generic;
    using System.Globalization;

    public static class SessionStoreCache
    {
        private static Dictionary<string, Store> sessionStoreMap = new Dictionary<string, Store>();

        static SessionStoreCache()
        {
            AppDomain.CurrentDomain.ProcessExit += new EventHandler(SessionStoreCache.CurrentDomain_Cleanup);
            AppDomain.CurrentDomain.DomainUnload += new EventHandler(SessionStoreCache.CurrentDomain_Cleanup);
        }

        public static void CacheSessionStore(string storeKey, Store store)
        {
            Store store2;
            if (storeKey == null)
            {
                throw new ArgumentNullException("storeKey");
            }
            if (store == null)
            {
                throw new ArgumentNullException("store");
            }
            if (sessionStoreMap.TryGetValue(storeKey, out store2))
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Microsoft.VisualStudio.TextTemplating.Modeling.Properties.Resources.ExisitingCacheEntry, new object[] { storeKey }), "storeKey");
            }
            sessionStoreMap[storeKey] = store;
        }

        private static void CurrentDomain_Cleanup(object sender, EventArgs args)
        {
            AppDomain.CurrentDomain.ProcessExit -= new EventHandler(SessionStoreCache.CurrentDomain_Cleanup);
            AppDomain.CurrentDomain.DomainUnload -= new EventHandler(SessionStoreCache.CurrentDomain_Cleanup);
            DisposeSessionStoreMap();
        }

        public static void DisposeSessionStoreMap()
        {
            foreach (Store store in sessionStoreMap.Values)
            {
                if (!store.Disposed)
                {
                    store.Dispose();
                }
            }
            sessionStoreMap.Clear();
        }

        public static Store GetSessionStore(string storeKey)
        {
            Store store;
            if (storeKey == null)
            {
                throw new ArgumentNullException("storeKey");
            }
            if (sessionStoreMap.TryGetValue(storeKey, out store))
            {
                return store;
            }
            return null;
        }
    }
}

