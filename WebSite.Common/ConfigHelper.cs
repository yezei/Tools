using System;
using System.Configuration;
using System.Xml;

namespace WebSite.Common
{
    /// <summary>
    /// web.config操作类
    /// </summary>
    public class ConfigHelper
    {
        static string appPath = System.Web.HttpContext.Current.Request.ApplicationPath;

        #region 得到AppSettings中的配置字符串信息

        /// <summary>
        /// 得到AppSettings中的配置字符串信息
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetConfigString(string key)
        {
            string CacheKey = "AppSettings-" + key;
            object objModel = CacheHelper.GetCache(CacheKey);
            if (objModel == null)
            {
                try
                {
                    objModel = ConfigurationManager.AppSettings[key];
                    if (objModel != null)
                    {
                        CacheHelper.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(180), TimeSpan.Zero);
                    }
                }
                catch
                { }
            }
            return objModel.ToString();
        }

        #endregion

        #region 得到AppSettings中的配置Bool信息

        /// <summary>
        /// 得到AppSettings中的配置Bool信息
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool GetConfigBool(string key)
        {
            bool result = false;
            string cfgVal = GetConfigString(key);
            if (null != cfgVal && string.Empty != cfgVal)
            {
                try
                {
                    result = bool.Parse(cfgVal);
                }
                catch (FormatException)
                {
                    // Ignore format exceptions.
                }
            }
            return result;
        }

        #endregion

        #region 得到AppSettings中的配置Decimal信息

        /// <summary>
        /// 得到AppSettings中的配置Decimal信息
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static decimal GetConfigDecimal(string key)
        {
            decimal result = 0;
            string cfgVal = GetConfigString(key);
            if (null != cfgVal && string.Empty != cfgVal)
            {
                try
                {
                    result = decimal.Parse(cfgVal);
                }
                catch (FormatException)
                {
                    // Ignore format exceptions.
                }
            }

            return result;
        }

        #endregion

        #region 得到AppSettings中的配置int信息

        /// <summary>
        /// 得到AppSettings中的配置int信息
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static int GetConfigInt(string key)
        {
            int result = 0;
            string cfgVal = GetConfigString(key);
            if (null != cfgVal && string.Empty != cfgVal)
            {
                try
                {
                    result = int.Parse(cfgVal);
                }
                catch (FormatException)
                {
                    // Ignore format exceptions.
                }
            }

            return result;
        }

        #endregion

        #region 获取xml文档对象

        /// <summary>
        /// 获取xml文档对象
        /// </summary>
        /// <returns></returns>
        public static XmlDocument CreateXmlDoc()
        {
            string appPath = System.Web.HttpContext.Current.Request.ApplicationPath;
            XmlDocument xmlDoc = new XmlDocument();

            try
            {
                xmlDoc.Load(System.Web.HttpContext.Current.Server.MapPath(appPath + "/WebSite.Config"));
            }
            catch
            {

            }
            return xmlDoc;
        }

        #endregion

        #region 保存xml文档

        /// <summary>
        /// 保存xml文档
        /// </summary>
        /// <param name="xmlDoc"></param>
        public static void SaveXmlDoc(XmlDocument xmlDoc)
        {
            xmlDoc.Save(System.Web.HttpContext.Current.Server.MapPath(appPath + "/WebSite.Config"));
        }

        #endregion

        #region 获取一个节点

        /// <summary>
        /// 获取一个节点
        /// </summary>
        /// <param name="xPath"></param>
        /// <returns></returns>
        public static XmlNode GetNode(string xPath)
        {
            XmlDocument xmlDoc = CreateXmlDoc();
            XmlNode xn = xmlDoc.SelectSingleNode(xPath);
            return xn;
        }

        #endregion

        #region 获取一个节点

        /// <summary>
        /// 获取一个节点
        /// </summary>
        /// <param name="xPath"></param>
        /// <returns></returns>
        public static string GetNodeValue(string KeyName)
        {
            XmlDocument xmlDoc = CreateXmlDoc();
            XmlNode xn = xmlDoc.SelectSingleNode("config/" + KeyName + "/value");
            return xn.InnerText;
        }

        #endregion

        #region 获取所有节点

        /// <summary>
        /// 获取所有节点
        /// </summary>
        /// <param name="xPath"></param>
        /// <returns></returns>
        public static XmlNodeList GetNodes(string xPath)
        {
            XmlDocument xmlDoc = CreateXmlDoc();
            XmlNodeList xnl = xmlDoc.SelectNodes(xPath);
            return xnl;
        }

        public static XmlNodeList GetChildNodes(string xPath)
        {
            XmlDocument xmlDoc = CreateXmlDoc();
            XmlNodeList xnl = xmlDoc.SelectSingleNode(xPath).ChildNodes;
            return xnl;
        }

        #endregion

        #region 向一个节点写入值

        /// <summary>
        /// 向一个节点写入值
        /// </summary>
        public static void WriteInnerText(string xPath, string innerText)
        {
            XmlDocument xmlDoc = CreateXmlDoc();
            ((XmlElement)xmlDoc.SelectSingleNode(xPath)).InnerText = innerText;
            SaveXmlDoc(xmlDoc);
        }

        #endregion
    }
}