using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace K12.Data
{
    /// <summary>
    /// 當沒有該欄位時的處理方式。
    /// </summary>
    public enum PaddingMethod
    {
        /// <summary>
        /// 給該欄位的第一個值，例如「秒」就是 0。
        /// </summary>
        First,
        /// <summary>
        /// 給該欄位的最後一個值，例如「秒」就是 59。
        /// </summary>
        Last,
        /// <summary>
        /// 給該欄位「現在」值(自已思考是什麼意思)。
        /// </summary>
        Now,
        /// <summary>
        /// 不給該欄位值。
        /// </summary>
        None
    }
}
