using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExLog
{
    #region 日志实体类
    /// <summary>
    /// 日志实体类
    /// Create By Asen
    /// 2016-09-21
    /// </summary>
    #endregion 
    public class Log_Obj
    {
        /// <summary>
        /// 自定义的异常名称
        /// </summary>
        public string ExName;
        /// <summary>
        /// 所在行数
        /// </summary>
        public string Row;
        /// <summary>
        /// 所在方法
        /// </summary>
        public string Method;
        /// <summary>
        /// 所在源
        /// </summary>
        public string Source;
        /// <summary>
        /// 引发当前异常的实例
        /// </summary>
        public string InnerException;
        /// <summary>
        /// 自定义的错误(Exception.Data)
        /// </summary>
        public List<KeyValuePair<string,string>> Custom;
        /// <summary>
        /// 异常时间集
        /// </summary>
        public List<string> Time;
    }
   

    //public class Log_Custom {
    //    /// <summary>
    //    /// 名称
    //    /// </summary>
    //    public string Name;
    //    /// <summary>
    //    /// 值
    //    /// </summary>
    //    public string Value;
    //}
}
