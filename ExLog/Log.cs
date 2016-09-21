using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Linq;

namespace ExLog.Instance
{
    #region 日志类实现类
    /// <summary>
    /// 日志类实现类
    /// Create By Asen 
    /// 2016-09-21
    /// </summary>
    #endregion
    public class Log
    {
        /// <summary>
        /// 异常日志根目录
        /// </summary>
        private string RootPath;
        /// <summary>
        /// 异常日志信息目录
        /// </summary>
        private string DirectoryPath;
        /// <summary>
        /// 目录锁
        /// </summary>
        private static readonly object DirectoryLock = new object();
        /// <summary>
        /// 文件锁
        /// </summary>
        private static readonly object XmlLock = new object();


        public Log(string Root)
        {
            RootPath = Path.Combine(Root,"ExLog");
            if (Directory.Exists(RootPath) == false)
            {
                Directory.CreateDirectory(RootPath);
            }
        }

        #region 【方法】创建异常日志目录
        private void CreateLogXml(string Path) {
            if (File.Exists(Path) == false)
            {
                lock (XmlLock)
                {
                    XDocument XDoc = new XDocument(new XDeclaration("1.0", "utf-8", "yes"), new XElement("Ex"));
                    XDoc.Save(Path);                 
                }
            }
        }
        #endregion

        #region 【方法】创建目录的方法
        internal void Create() {
             DirectoryPath = Path.Combine(RootPath,DateTime.Now.ToString("yyyy-MM-dd"));
            if (Directory.Exists(DirectoryPath) == false)
            {
                lock (DirectoryLock)
                {
                    Directory.CreateDirectory(DirectoryPath);
                } 
            }
        }
        #endregion

        #region 【方法】写入异常日志
        /// <summary>
        /// 【方法】写入异常日志
        /// </summary>
        /// <param name="ExName">名称</param>
        /// <param name="Ex">异常</param>
        /// <param name="TypeName">异常分类</param>
        public void Write(string ExName, Exception Ex, string TypeName = "Default")
        {
            string XmlPath =Path.Combine(DirectoryPath, TypeName+".xml");
            CreateLogXml(XmlPath);
            XDocument XDoc = XDocument.Load(XmlPath);
            XElement XRoot = XDoc.Element("Ex");
            XElement Content = null;
            foreach (XElement XNode in XRoot.Elements(ExName))
            {
                if (XNode.Element("Row").Value == Ex.StackTrace&&XNode.Element("Content").Value==Ex.Message)
                {
                    Content = XNode;
                    break;
                }
            }
            lock (XmlLock)
            {
                if (Content == null)
                {
                    
                        Content = new XElement(ExName,
                        new XElement("Content", Ex.Message),
                        new XElement("Row",Ex.StackTrace),
                        new XElement("Method",Ex.TargetSite),
                        new XElement("Source", Ex.Source),
                        new XElement("InnerException",Ex.InnerException),
                        new XElement("Custom"),
                        new XElement("Time", DateTime.Now.ToString())
                        );

                        foreach (KeyValuePair<string, string> DataEx in Ex.Data)
                        {
                            Content.Element("Custom").Add(new XElement("Data",
                                new XElement("Name", DataEx.Key),
                                new XElement("Value", DataEx.Value)
                                ));
                        }
                        XRoot.Add(Content);
                }
                else
                {
                    XRoot.Element(ExName).Add(new XElement("Time", DateTime.Now.ToString()));
                }
                XDoc.Save(XmlPath);
            }
        }
        #endregion

        #region 【方法】读取当日异常信息列表
        /// <summary>
        /// 读取当日Xml文件列表
        /// </summary>
        /// <returns>字符串数组</returns>
        public string[] Read_ExListByDay() {
            string XmlPath = Path.Combine(RootPath, DateTime.Now.ToString("yyyy-MM-dd"));
            return Directory.GetFiles(XmlPath);  
        }
        #endregion

        #region 【方法】读取指定日期异常信息列表
        /// <summary>
        /// 读取指定日期异常信息列表
        /// </summary>
        /// <param name="Date">日期</param>
        /// <returns>字符串数组</returns>
        public string[] Read_ExListByDay(DateTime Date)
        {
            string XmlPath = Path.Combine(RootPath, Date.ToString("yyyy-MM-dd"));
            return Directory.GetFiles(XmlPath);  
        }
        #endregion

        #region 【方法】读取所有异常列表
        public string[] Read_ExAllPath() {
            string XmlPath = Path.Combine(RootPath);
            return Directory.GetFiles(XmlPath);         
        }
        #endregion

        #region 【方法】读取异常日志的详细信息
        /// <summary>
        /// 【方法】读取异常日志的详细信息
        /// </summary>
        /// <param name="Date">日期</param>
        /// <param name="Name">名称</param>
        /// <returns>List(Log_Obj)</returns>
        public List<Log_Obj> Read_ExDetial(DateTime Date, string Name="Default")
        {
            List<Log_Obj> Lst_Log = new List<Log_Obj>();
            string XmlPath = Path.Combine(RootPath, Date.ToString("yyyy-MM-dd"), Name+".xml");

            if (File.Exists(XmlPath) == false)
            {
                return Lst_Log;
            }

            XDocument XDoc = XDocument.Load(XmlPath);

            foreach (XElement Node in XDoc.Element("Ex").Elements())
            {
                Log_Obj LogObj = new Log_Obj();
                List<string> Lst_Time = new List<string>();
                List<KeyValuePair<string, string>> Lst_Custom = new List<KeyValuePair<string, string>>();
                LogObj.ExName = Node.Name.LocalName;
                LogObj.Method = Node.Element("Method").Value;
                LogObj.Row = Node.Element("Row").Value.Trim();
                LogObj.Source = Node.Element("Source").Value;
                LogObj.InnerException = Node.Element("InnerException").Value;
                foreach (XElement TimeNode in Node.Elements("Time"))
                {
                    Lst_Time.Add(TimeNode.Value);
                }

                foreach(XElement CustomNode in Node.Element("Custom").Elements("Data")){
                    KeyValuePair<string, string> Custom = new KeyValuePair<string,string>(
                       CustomNode.Attribute("Name").Value,
                       CustomNode.Attribute("Value").Value);

                    Lst_Custom.Add(Custom);
                }

                LogObj.Time = Lst_Time;
                LogObj.Custom = Lst_Custom;

                Lst_Log.Add(LogObj);
            }
            return Lst_Log;
        }
        #endregion






    }
}
