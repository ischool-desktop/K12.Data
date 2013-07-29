using System;
using System.Collections.Generic;
using System.Text;

namespace K12.Data.Configuration
{
    internal interface IExtraProcesser
    {
        ExtraInformation[] Process(object instance);
    }
}
