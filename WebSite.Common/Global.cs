using System;

namespace WebSite.Common
{
    /// <summary>
    /// 全局使用方法
    /// </summary>
    public class Global
    {
        #region 初始化model

        /// <summary>
        /// 初始化model
        /// </summary>
        public static void InitModel(object model)
        {
            #region
            foreach (System.Reflection.PropertyInfo property in model.GetType().GetProperties())
            {

                string typeString = property.PropertyType.ToString();
                switch (typeString)
                {
                    case "System.String":
                        property.SetValue(model, "", null);
                        break;
                    case "System.DateTime":
                        property.SetValue(model, new DateTime(1900, 1, 1), null);
                        break;
                    case "System.Byte[]":
                        property.SetValue(model, new System.Byte[0], null);
                        break;
                    case "System.Int32":
                        property.SetValue(model, 0, null);
                        break;
                    case "System.Nullable`1[System.Int32]":
                        property.SetValue(model, 0, null);
                        break;
                    case "System.Nullable`1[System.DateTime]":
                        property.SetValue(model, new DateTime(1900, 1, 1), null);
                        break;
                }
            }
            #endregion
        }

        #endregion
    }
}