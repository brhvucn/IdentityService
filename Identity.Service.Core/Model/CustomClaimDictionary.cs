using System;
using System.Collections.Generic;
using System.Text;

namespace Identity.Service.Core.Model
{
    /// <summary>
    /// Simple derived class for giving a proper development experience when adding custom claims
    /// </summary>
    public class CustomClaimDictionary : Dictionary<string, string>
    {
        public new void Add(string key, string value)
        {
            base.Add(key, value);
        }

        public new string this[string title]
        {
            get { return base[title]; }
            set { base[title] = value; }
        }
    }
}
