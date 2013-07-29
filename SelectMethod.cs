using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace K12.Data
{
    [AttributeUsage(AttributeTargets.Method)]
    public class SelectMethod : Attribute
    {
        public SelectMethod(string Name, string Caption)
        {
            this.Name = Name;
            this.Caption = Caption;
        }

        public string Name { get; set; }
        public string Caption { get; set; }
    }
}