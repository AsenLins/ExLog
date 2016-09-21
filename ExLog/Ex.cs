using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using ExLog.Instance;
namespace ExLog
{
    #region 日志工厂(单列模式)
    /// <summary>
    /// 日志工厂(单列模式)
    /// Create By Asen
    /// 2016-09-20
    /// </summary>
    #endregion
    public class Ex
    {
        /// <summary>
        /// 日志类
        /// </summary>
        private static Log _Log;

        /// <summary>
        /// 日志类实例的锁
        /// </summary>
        private static readonly object InstanceLock = new object();

        /// <summary>
        /// 没有构造函数
        /// </summary>
        private Ex() { }

        /// <summary>
        /// 程序的实例日志
        /// </summary>
        public static Log AppLog {
            get {
                return GetInstance(Environment.CurrentDirectory); 
            }
        }

        /// <summary>
        /// 网站的实例日志
        /// </summary>
        public static Log WebLog {
            get {
                return GetInstance(HttpRuntime.AppDomainAppPath);  
            }
        }

        #region 【方法】获取日志实例
        /// <summary>
        /// 获取日志实例
        /// </summary>
        /// <param name="RootPath">日志根目录</param>
        /// <returns>日志类</returns>
        private static Log GetInstance(string RootPath) {
            if (_Log == null)
            {
                lock (InstanceLock)
                {
                    _Log = new Log(RootPath);
                }
            }
            _Log.Create();
            return _Log;
        }
        #endregion
    }
}
