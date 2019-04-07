using System;

namespace Demo.WCF.BasicHttpLoginSession.Services.Common
{
    public class CacheItemRemovedEventArgs : EventArgs
    {
        public string Key { get; set; }
        public object Value { get; set; }

        public CacheItemRemovedEventArgs(string key, object value)
        {
            this.Key = key;
            this.Value = value;
        }
    }
}