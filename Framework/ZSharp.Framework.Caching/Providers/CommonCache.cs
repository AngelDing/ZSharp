using System.Collections.Generic;
using System.Text.RegularExpressions;
using ZSharp.Framework.Utils;

namespace ZSharp.Framework.Caching
{
    public abstract class CommonCache : BaseCache
    {
        public abstract IEnumerable<KeyValuePair<string, object>> GetAllEntries();        

        public List<string> GetKeysToRemoveByPattern(string pattern)
        {
            GuardHelper.ArgumentNotEmpty(() => pattern);

            var regex = new Regex(pattern, RegexOptions.Singleline
                | RegexOptions.Compiled | RegexOptions.IgnoreCase);
            var keysToRemove = new List<string>();

            var allEntries = GetAllEntries();
            foreach (var item in allEntries)
            {
                if (regex.IsMatch(item.Key))
                {
                    keysToRemove.Add(item.Key);
                }
            }

           return keysToRemove;
        }

        public List<string> GetAllKeys()
        {
            var allKeys = new List<string>();
            var allEntries = GetAllEntries();
            foreach (var item in allEntries)
            {
                allKeys.Add(item.Key);
            }
            return allKeys;
        }

        public void RemoveByPattern(string pattern)
        {
            var keysToRemove = GetKeysToRemoveByPattern(pattern);
            RemoveByKeys(keysToRemove);
        }

        public void ClearAll()
        {
            var allKeys = GetAllKeys();

            RemoveByKeys(allKeys); 
        }

        private void RemoveByKeys(IList<string> keys)
        {
            using (EnterWriteLock())
            {
                foreach (string key in keys)
                {
                    Remove(key);
                }
            }
        }        
    }
}
