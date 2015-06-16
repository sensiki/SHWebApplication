using System;
using System.Web;
using System.Web.SessionState;
using cn.com.farsight.SH.SHModel;
using cn.com.farsight.SH.SHDbVisit;
using System.IO;
using Newtonsoft.Json;
using System.Text;
using System.Collections.Generic;

namespace cn.com.farsight.SH.SHWebApplication.Admin
{
    /// <summary>
    /// login 的摘要说明
    /// </summary>
    public class login : IHttpHandler, IRequiresSessionState
    {
        UserManager um = new UserManager();
        public void ProcessRequest(HttpContext context)
        {
            bool isLogin = !string.IsNullOrEmpty(context.Request["login"]);
            if (isLogin)
            {
                context.Response.ContentType = "text/plain";
                Simple_Result lresult = new Simple_Result();
                string name = context.Request["name"];
                string pwd = context.Request["pwd"];
                if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(pwd))
                {
                    lresult.result = "false";
                    lresult.data = "用户名或密码不能为空(>_<)";
                    context.Response.Write(JsonConvert.SerializeObject(lresult));
                    return;
                }
                User d = new User() { Name = name, Pwd = pwd };
                if (um.getModel(d))
                {
                    if (d.Pwd.CompareTo(pwd) != 0)
                    {
                        lresult.result = "false";
                        lresult.data = "用户名不存在或密码错误!(>_<)";
                    }
                    else
                    {
                        lresult.result = "true";
                        context.Session["LoginUserPermisson"] = d.User_permisson;
                        context.Session["LoginUserName"] = d.Name;
                        d.Time = DateTime.Now;
                        um.updateModel(d);
                        if (d.Name == "aumin")
                            lresult.data = "../Admin/admin.ashx";
                        else
                            lresult.data = "../Admin/admin.ashx";
                    }
                }
                else
                {
                    lresult.result = "false";
                    lresult.data = "用户名不存在或密码错误!(>_<)";
                }
                context.Response.Write(JsonConvert.SerializeObject(lresult));
            }
            else
            {
                context.Response.ContentType = "text/html";
                StreamReader reader = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + "html/login.html");
                string html = reader.ReadToEnd();
                reader.Close();
                context.Response.Write(html);
            }
        }
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}