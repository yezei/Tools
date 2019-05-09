using System.Text;
using System.Web.UI;
using System.Web;

namespace WebSite.Common
{
    //页面中弹出对话框
    public class MessageBox
    {
        private MessageBox()
        {
        }

        /// <summary>
        /// 输出自定义脚本信息
        /// </summary>
        /// <param name="page">当前页面指针，一般为this</param>
        /// <param name="script">输出脚本</param>
        public static void ResponseScript(System.Web.UI.Page page, string script)
        {
            page.ClientScript.RegisterStartupScript(page.GetType(), "message", "<script language=\"javascript\" defer=\"defer\">" + script + "</script>");
        }

        #region 后台操作提示
        /// <summary>
        /// 显示消息提示对话框
        /// </summary>
        /// <param name="page">当前页面指针，一般为this</param>
        /// <param name="msg">提示信息</param>
        public static void Show(System.Web.UI.Page page, string msg)
        {
            string msbox = "parent.message(\"warning\",\"" + msg + "\")";
            page.ClientScript.RegisterStartupScript(page.GetType(), "message", msbox, true);
        }

        /// <summary>
        /// 显示消息提示对话框，并进行页面跳转
        /// </summary>
        /// <param name="page">当前页面指针，一般为this</param>
        /// <param name="msg">提示信息</param>
        /// <param name="url">跳转的目标URL</param>
        public static void ShowRedirect(System.Web.UI.Page page, string msg, string url)
        {
            string msbox = "parent.message(\"success\",\"" + msg + "\", \"" + url + "\");parent.$('#mainframe').attr('src', 'about:blank');";
            page.ClientScript.RegisterStartupScript(page.GetType(), "message", msbox, true);
        }

        /// <summary>
        /// 显示消息提示对话框，并刷新页面
        /// </summary>
        /// <param name="page">当前页面指针，一般为this</param>
        /// <param name="msg">提示信息</param>
        public static void ShowRedirect(System.Web.UI.Page page, string msg)
        {
            ShowRedirect(page, msg, System.Web.HttpContext.Current.Request.RawUrl.ToString());
        }
        #endregion

        #region 前台操作提示
        /// <summary>
        /// 显示消息提示对话框
        /// </summary>
        /// <param name="page">当前页面指针，一般为this</param>
        /// <param name="msg">提示信息</param>
        public static void ShowMsg(System.Web.UI.Page page, string msg)
        {
            page.ClientScript.RegisterStartupScript(page.GetType(), "message", "<script language='javascript' defer>alert('" + msg.ToString() + "');</script>");
        }
        /// <summary>
        /// 显示消息提示对话框，并进行页面跳转
        /// </summary>
        /// <param name="page">当前页面指针，一般为this</param>
        /// <param name="msg">提示信息</param>
        /// <param name="url">跳转的目标URL</param>
        public static void ShowMsgAndRedirect(System.Web.UI.Page page, string msg, string url)
        {
            StringBuilder Builder = new StringBuilder();
            Builder.Append("<script language='javascript' defer>");
            Builder.AppendFormat("alert('{0}');", msg);
            Builder.AppendFormat("location.href='{0}'", url);
            Builder.Append("</script>");
            page.ClientScript.RegisterStartupScript(page.GetType(), "message", Builder.ToString());

        }
        #endregion

    }
}