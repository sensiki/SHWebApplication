using cn.com.farsight.SH.SHDbVisit;
using cn.com.farsight.SH.SHModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Web;
using System.Xml;

namespace cn.com.farsight.SH.SHWebApplication
{
    /// <summary>
    /// 接受/发送消息帮助类
    /// </summary>
    public class messageHelp
    {
        DataManager dm = new DataManager();
        CommandManager cm = new CommandManager();
        StatusManager sm = new StatusManager();
        TemHumManager thm = new TemHumManager();
        UserManager um = new UserManager();
        //返回消息
        public string ReturnMessage(string postStr)
        {
            string responseContent = "";
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.Load(new System.IO.MemoryStream(System.Text.Encoding.GetEncoding("GB2312").GetBytes(postStr)));
            XmlNode MsgType = xmldoc.SelectSingleNode("/xml/MsgType");
            if (MsgType != null)
            {
                switch (MsgType.InnerText)
                {
                    case "event":
                        responseContent = EventHandle(xmldoc);//事件处理
                        break;
                    case "text":
                        responseContent = TextHandle(xmldoc);//接受文本消息处理
                        break;
                    case "voice":
                        responseContent = VoiceHandle(xmldoc);//接受语音消息处理
                        break;
                    case "image":
                        responseContent = ImageHandle(xmldoc);//接受图片消息处理
                        break;
                    default:
                        break;
                }
            }
            return responseContent;
        }
        //事件
        public string EventHandle(XmlDocument xmldoc)
        {
            string responseContent = "";
            XmlNode Event = xmldoc.SelectSingleNode("/xml/Event");
            XmlNode EventKey = xmldoc.SelectSingleNode("/xml/EventKey");
            XmlNode ToUserName = xmldoc.SelectSingleNode("/xml/ToUserName");
            XmlNode FromUserName = xmldoc.SelectSingleNode("/xml/FromUserName");
            if (Event != null)
            {
                //菜单单击事件
                if (Event.InnerText.Equals("CLICK"))
                {
                    string a = "";
                    int Send_Result;
                    switch (EventKey.InnerText)
                    {
                        case "Light_OneFL"://一楼次卧
                            if ((Send_Result = SendCommand("01", "72", FromUserName.InnerText, 2)) != 2)
                            {
                                if (Send_Result == 0)
                                    a = "正为您关闭一楼主卧灯";
                                else
                                    a = "正为您打开一楼主卧灯";
                            }
                            else
                                a = "操作失败了/::'(，节点好像断开了/:break";
                            break;
                        case "Light_TwoFL"://二楼次卧
                            if ((Send_Result = SendCommand("02", "72", FromUserName.InnerText, 2)) != 2)
                            {
                                if (Send_Result == 0)
                                    a = "正为您关闭二楼次卧灯";
                                else
                                    a = "正为您打开二楼次卧灯";
                            }
                            else
                                a = "操作失败了/::'(，节点好像断开了/:break";
                            break;
                        case "Light_Hall"://大厅
                            if ((Send_Result = SendCommand("03", "72", FromUserName.InnerText, 2)) != 2)
                            {
                                if (Send_Result == 0)
                                    a = "正为您关闭大厅灯";
                                else
                                    a = "正为您打开大厅灯";
                            }
                            else
                                a = "操作失败了/::'(，节点好像断开了/:break";
                            break;
                        case "Light_Kitchen"://厨房
                            if ((Send_Result = SendCommand("05", "72", FromUserName.InnerText, 2)) != 2)
                            {
                                if (Send_Result == 0)
                                    a = "正为您关闭厨房灯";
                                else
                                    a = "正为您打开厨房灯";
                            }
                            else
                                a = "操作失败了/::'(，节点好像断开了/:break";
                            break;
                        case "Light_Study"://书房
                            if ((Send_Result = SendCommand("06", "72", FromUserName.InnerText, 2)) != 2)
                            {
                                if (Send_Result == 0)
                                    a = "正为您关闭书房灯";
                                else
                                    a = "正为您打开书房灯";
                            }
                            else
                                a = "操作失败了/::'(，节点好像断开了/:break";
                            break;
                        case "Light_Stairs"://楼梯
                            if ((Send_Result = SendCommand("04", "72", FromUserName.InnerText, 2)) != 2)
                            {
                                if (Send_Result == 0)
                                    a = "正为您关闭楼梯灯";
                                else
                                    a = "正为您打开楼梯灯";
                            }
                            else
                                a = "操作失败了/::'(，节点好像断开了/:break";
                            break;
                        case "Home_Curtain"://窗帘
                            if ((Send_Result = SendCommand("09", "72", FromUserName.InnerText, 2)) != 2)
                            {
                                if (Send_Result == 0)
                                    a = "正为您关闭窗帘";
                                else
                                    a = "正为您打开窗帘";
                            }
                            else
                                a = "操作失败了/::'(，节点好像断开了/:break";
                            break;
                        case "Garage_Door"://车库门
                            if ((Send_Result = SendCommand("07", "72", FromUserName.InnerText, 2)) != 2)
                            {
                                if (Send_Result == 0)
                                    a = "正为您关闭车库门";
                                else
                                    a = "正为您打开车库门";
                            }
                            else
                                a = "操作失败了/::'(，节点好像断开了/:break";
                            break;
                        case "Home_Warning"://报警
                            if ((Send_Result = SendCommand("0A", "72", FromUserName.InnerText, 2)) != 2)
                            {
                                if (Send_Result == 0)
                                    a = "正为您关闭报警器";
                                else
                                    a = "正为您打开报警器";
                            }
                            else
                                a = "操作失败了/::'(，节点好像断开了/:break";
                            break;
                        case "Home_Camera"://摄像头
                            if ((Send_Result = SendCommand("0C", "72", FromUserName.InnerText, 2)) != 2)
                            {
                                if (Send_Result == 0)
                                    a = "正为您关闭摄像头";
                                else
                                    a = "正为您打开摄像头";
                            }
                            else
                                a = "操作失败了/::'(，节点好像断开了/:break";
                            break;
                        case "Home_Tem_Hum"://温湿度
                            NodeStatus s = new NodeStatus();
                            s.Modle = "0B";
                            s = sm.GetListByPage(s);
                            a = "温度：" + int.Parse(s.Data.Substring(2, 2), NumberStyles.HexNumber) + "℃" + "\n湿度：" + int.Parse(s.Data.Substring(4, 2), NumberStyles.HexNumber) + "%"+"\n时间："+s.Time;
                            break;
                        case "All_note_status"://所有节点信息
                            int count = 0;
                            NodeStatus u = new NodeStatus();
                            if ((count = sm.GetRecordCount(u)) > 0)
                            {
                                List<NodeStatus> list = sm.getModelListAll(u);
                                a = create_node_list(list);
                                list.Clear();
                            }
                            break;
                        case "Home_Photoelectricity"://光电闸
                            NodeStatus s2 = new NodeStatus();
                            s2.Modle = "0B";
                            s2 = sm.GetListByPage(s2);
                            if (s2.Data[0] == '1')
                                a = "光电闸：开" + "\n时间：" + s2.Time;
                            else
                                a = "光电闸：关" + "\n时间：" + s2.Time;
                            break;
                        case "Home_Door"://房门
                            NodeStatus s3 = new NodeStatus();
                            s3.Modle = "0B";
                            s3 = sm.GetListByPage(s3);
                            if (s3.Data[1] == '1')
                                a = "房门：开" + "\n时间：" + s3.Time;
                            else
                                a = "房门：关" + "\n时间：" + s3.Time;
                            break;
                        default:
                            a = "未知错误！";
                            break;

                    }
                        responseContent = string.Format(ReplyType.Message_Text,
                            FromUserName.InnerText,
                            ToUserName.InnerText,
                            DateTime.Now.Ticks,
                            a);
                    
                }
                else if(Event.InnerText.Equals("subscribe"))
                {
                    responseContent = string.Format(ReplyType.Message_Text,
                            FromUserName.InnerText,
                            ToUserName.InnerText,
                            DateTime.Now.Ticks,
                            "您好！感谢您的关注！很高兴为您服务,请输入序号了解相关信息！\n[1]查看联系方式\n[2]查看最新资讯\n[3]官方网站");
                }
            }
            return responseContent;
        }
        //接受文本消息
        public string TextHandle(XmlDocument xmldoc)
        {
            string responseContent = "";
            XmlNode ToUserName = xmldoc.SelectSingleNode("/xml/ToUserName");
            XmlNode FromUserName = xmldoc.SelectSingleNode("/xml/FromUserName");
            XmlNode Content = xmldoc.SelectSingleNode("/xml/Content");
            if (Content != null)
            {
                StringBuilder a = new StringBuilder();
                if (Content.InnerText.IndexOf("歌") != -1)
                {
                    responseContent = string.Format(ReplyType.Message_Music,
                    FromUserName.InnerText,
                    ToUserName.InnerText,
                    DateTime.Now.Ticks,
                    "旅行的意义",
                    "陈绮贞",
                    "http://tsmusic24.tc.qq.com/466727.mp3",
                    "http://tsmusic24.tc.qq.com/466727.mp3"
                    );
                }
                else if (Content.InnerText == "帮助")
                {
                    a.Append("您好！很高兴为您服务。请输入序号了解相关信息！");
                    a.Append("\n[1]查看联系方式");
                    a.Append("\n[2]查看最新资讯");
                    a.Append("\n[3]官方网站");
                }
                else if (Content.InnerText.IndexOf("笑话") != -1)
                {
                    a.Append("有个中年男子骑着摩托车飞驰在马路上，车的后面还坐着一个小男孩，由于路面高低不平车又开得快所以摩托车后面的男孩被颠簸得左晃右晃，这一切都被路边的另外一个男人看到，他心想这个骑摩托车的大人也太不负责了，这要是把小孩摔下来可咋办，于是他便开着他的车追了上去，然后拦下那辆摩托车，并斥责那个开摩托车的男子说：“有你这样开车的哇！这要是把小孩摔下来咋办！”，这时骑摩托车的男子回头惊讶的对小孩说：“儿子，你妈呢？” ");
                    a.Append("\r\n<a href=\"http://m.qiushibaike.com/\">点击进入糗事百科</a>");
                }
                else if ((Content.InnerText.IndexOf("图片") != -1) || (Content.InnerText.IndexOf("美女") != -1))
                {
                    a.Append("IdY69mE7bQJEGBA82EVsy8-3Tp8XkXJR85g4pMxyZakRiVoIyJllNWPd4wae_fg4");
                }
                else if (Content.InnerText == "1")
                {
                    a.Append("地址：北京市海淀区西三旗悦秀路北京明园大学校内 华清远见教育集团");
                    a.Append("\n高校业务洽谈电话：18600463336");
                    a.Append("\n技术支持电话：010-82600386转855/851");
                    a.Append("\n技术支持邮箱：support@farsight.com.cn");
                    a.Append("\n产品咨询QQ：752605080");
                }
                else if (Content.InnerText == "2")
                {
                    responseContent = string.Format(ReplyType.Message_News_Main,
                            FromUserName.InnerText,
                            ToUserName.InnerText,
                            DateTime.Now.Ticks,
                            "1",
                             string.Format(ReplyType.Message_News_Item, "FarSight Watch开源智能手表", "",
                             "http://image.baidu.com/i?tn=download&word=download&ie=utf8&fr=detail&url=http%3A%2F%2Fdev.hqyj.com%2Fproducts%2Fimages%2Fcase40.jpg&thumburl=http%3A%2F%2Fimg2.imgtn.bdimg.com%2Fit%2Fu%3D306995081%2C1400109639%26fm%3D11%26gp%3D0.jpg",
                             "http://dev.hqyj.com/products/case40.htm"));
                    return responseContent;
                }
                else if (Content.InnerText == "3")
                {
                    responseContent = string.Format(ReplyType.Message_News_Main,
                            FromUserName.InnerText,
                            ToUserName.InnerText,
                            DateTime.Now.Ticks,
                            "1",
                             string.Format(ReplyType.Message_News_Item, "华清远见研发中心", "专业始于专注 卓识源于远见",
                             "http://image.baidu.com/i?tn=download&word=download&ie=utf8&fr=detail&url=http%3A%2F%2Ff.seals.qq.com%2Ffilestore%2F10024%2Fc5%2Fb3%2F2e%2F1000%2Fpic%2FAgencyNew%2F201406%2F1402905994_702112156.jpg&thumburl=http%3A%2F%2Fimg3.imgtn.bdimg.com%2Fit%2Fu%3D3188260314%2C3062773615%26fm%3D21%26gp%3D0.jpg",
                             "http://dev.hqyj.com/"));
                    return responseContent;
                }
                else if (Content.InnerText == "4")
                {
                    a.Append("啊哈哈哈哈哈哈哈哈！！！Surprise！！！！");
                }
                else 
                {
                    a.Append(Command_Control(Content.InnerText, FromUserName.InnerText));
                }
                responseContent = string.Format(ReplyType.Message_Text,
                        FromUserName.InnerText,
                        ToUserName.InnerText,
                        DateTime.Now.Ticks,
                        a);
            }
            return responseContent;
        }
        //接收语音消息
        public string VoiceHandle(XmlDocument xmldoc)
        {
            string responseContent = "";
            XmlNode ToUserName = xmldoc.SelectSingleNode("/xml/ToUserName");
            XmlNode FromUserName = xmldoc.SelectSingleNode("/xml/FromUserName");
            XmlNode Content = xmldoc.SelectSingleNode("/xml/Content");
            XmlNode Recognition = xmldoc.SelectSingleNode("/xml/Recognition");
            if (Recognition.InnerText != null)
            {
                if (Recognition.InnerText.IndexOf("歌") != -1)
                {
                    responseContent = string.Format(ReplyType.Message_Music,
                    FromUserName.InnerText,
                    ToUserName.InnerText,
                    DateTime.Now.Ticks,
                    "旅行的意义",
                    "陈绮贞",
                    "http://tsmusic24.tc.qq.com/466727.mp3",
                    "http://tsmusic24.tc.qq.com/466727.mp3"
                    );
                }

                else
                {
                    string a = Command_Control(Recognition.InnerText, FromUserName.InnerText);
                    responseContent = string.Format(ReplyType.Message_Text,
                    FromUserName.InnerText,
                    ToUserName.InnerText,
                    DateTime.Now.Ticks,
                    a);
                }
            }
            return responseContent;
        }
        //接收图片消息
        public string ImageHandle(XmlDocument xmldoc)
        {
            string responseContent = "";
            XmlNode ToUserName = xmldoc.SelectSingleNode("/xml/ToUserName");
            XmlNode FromUserName = xmldoc.SelectSingleNode("/xml/FromUserName");
            XmlNode Content = xmldoc.SelectSingleNode("/xml/Content");
            string a = "";
            a = "IdY69mE7bQJEGBA82EVsy8-3Tp8XkXJR85g4pMxyZakRiVoIyJllNWPd4wae_fg4";
            responseContent = string.Format(ReplyType.Message_Text,
                    FromUserName.InnerText,
                    ToUserName.InnerText,
                    DateTime.Now.Ticks,
                    a);


            return responseContent;
        }
        //写入日志
        public void WriteLog(string text)
        {
            StreamWriter sw = new StreamWriter(HttpContext.Current.Server.MapPath(".") + "\\log.txt", true);
            sw.WriteLine(text);
            sw.Close();//写入
        }
        /// <summary>
        /// 发送命令
        /// On_Off：0关闭，1打开，2改变状态
        /// return：0已发送关闭指令，1已发送打开命令，2节点断开连接，10节点已经是关闭状态，11节点已经是打开状态
        /// </summary>
        private int SendCommand(string modle1, string modle2, string userid,int On_Off)
        {
            NodeStatus s = new NodeStatus();
            s.Modle = modle1;
            s = sm.GetListByPage(s);
            if (s != null && s.Time.CompareTo(DateTime.Now.AddSeconds(-10)) >= 0 && s.Time.CompareTo(DateTime.Now.AddSeconds(10)) <= 0)
            {
                byte[] array = new byte[1];
                NodeCommand n = new NodeCommand();
                n.Identify = "2343";//#C
                n.Type = s.Type;
                n.Modle = modle2;
                n.User = userid;
                n.Addr = s.Addr.Substring(2, 2) + s.Addr.Substring(0, 2); ;
                if (s.Data == "000001")//现在状态为开
                {
                    if (On_Off == 1)
                        return 11;//
                    n.Data = "30";
                    n.Time = DateTime.Now;
                    cm.addModel(n);
                    return 0;
                }
                else
                {
                    if (On_Off == 0)
                        return 10;
                    n.Data = "31";
                    n.Time = DateTime.Now;
                    cm.addModel(n);
                    return 1;
                }
               
            }
            else
            {
                return 2;
            }
        }
        private string Light_Control(string command, string FromUserName)
        {
            string result = "抱歉，现在还无法理解您的指令（" + command + "），不过我会努力的/:,@f";
            int Send_Result;
            if ((command.IndexOf("一楼") != -1) || (command.IndexOf("主卧") != -1))
            {
                if ((command.IndexOf("开") != -1) & (command.IndexOf("关") != -1))
                    return result;
                else if (command.IndexOf("开") != -1)
                {
                    if ((Send_Result = SendCommand("01", "72", FromUserName, 1)) != 2)
                    {
                        if (Send_Result == 1)
                            result = "正为您打开一楼主卧灯";
                        else
                            result = "一楼主卧灯为打开状态";
                    }
                    else
                        result = "操作失败了/::'(，节点好像断开了/:break";
                }
                else if (command.IndexOf("关") != -1)
                {
                    if ((Send_Result = SendCommand("01", "72", FromUserName, 0)) != 2)
                    {
                        if (Send_Result == 0)
                            result = "正为您关闭一楼主卧灯";
                        else
                            result = "一楼主卧灯已经关闭了";
                    }
                    else
                        result = "操作失败了/::'(，节点好像断开了/:break";
                }
            }
            if ((command.IndexOf("二楼") != -1) || (command.IndexOf("次卧") != -1))
            {
                if ((command.IndexOf("开") != -1) & (command.IndexOf("关") != -1))
                    return result;
                else if (command.IndexOf("开") != -1)
                {
                    if ((Send_Result = SendCommand("02", "72", FromUserName, 1)) != 2)
                    {
                        if (Send_Result == 1)
                            result = "正为您打开二楼次卧灯";
                        else
                            result = "二楼次卧灯为打开状态";
                    }
                    else
                        result = "操作失败了/::'(，节点好像断开了/:break";
                }
                else if (command.IndexOf("关") != -1)
                {
                    if ((Send_Result = SendCommand("02", "72", FromUserName, 0)) != 2)
                    {
                        if (Send_Result == 0)
                            result = "正为您关闭二楼次卧灯";
                        else
                            result = "二楼次卧灯已经关闭了";
                    }
                    else
                        result = "操作失败了/::'(，节点好像断开了/:break";
                }
            }
            if (command.IndexOf("大厅") != -1)
            {
                if ((command.IndexOf("开") != -1) & (command.IndexOf("关") != -1))
                    return result;
                else if (command.IndexOf("开") != -1)
                {
                    if ((Send_Result = SendCommand("03", "72", FromUserName, 1)) != 2)
                    {
                        if (Send_Result == 1)
                            result = "正为您打开大厅灯";
                        else
                            result = "大厅灯为打开状态";
                    }
                    else
                        result = "操作失败了/::'(，节点好像断开了/:break";
                }
                else if (command.IndexOf("关") != -1)
                {
                    if ((Send_Result = SendCommand("03", "72", FromUserName, 0)) != 2)
                    {
                        if (Send_Result == 0)
                            result = "正为您关闭大厅灯";
                        else
                            result = "大厅灯已经关闭了";
                    }
                    else
                        result = "操作失败了/::'(，节点好像断开了/:break";
                }
            }
            if (command.IndexOf("楼梯") != -1)
            {
                if ((command.IndexOf("开") != -1) & (command.IndexOf("关") != -1))
                    return result;
                else if (command.IndexOf("开") != -1)
                {
                    if ((Send_Result = SendCommand("04", "72", FromUserName, 1)) != 2)
                    {
                        if (Send_Result == 1)
                            result = "正为您打开楼梯灯";
                        else
                            result = "楼梯灯为打开状态";
                    }
                    else
                        result = "操作失败了/::'(，节点好像断开了/:break";
                }
                else if (command.IndexOf("关") != -1)
                {
                    if ((Send_Result = SendCommand("04", "72", FromUserName, 0)) != 2)
                    {
                        if (Send_Result == 0)
                            result = "正为您关闭楼梯灯";
                        else
                            result = "楼梯灯已经关闭了";
                    }
                    else
                        result = "操作失败了/::'(，节点好像断开了/:break";
                }
            }
            if (command.IndexOf("厨房") != -1)
            {
                if ((command.IndexOf("开") != -1) & (command.IndexOf("关") != -1))
                    return result;
                else if (command.IndexOf("开") != -1)
                {
                    if ((Send_Result = SendCommand("05", "72", FromUserName, 1)) != 2)
                    {
                        if (Send_Result == 1)
                            result = "正为您打开厨房灯";
                        else
                            result = "厨房灯为打开状态";
                    }
                    else
                        result = "操作失败了/::'(，节点好像断开了/:break";
                }
                else if (command.IndexOf("关") != -1)
                {
                    if ((Send_Result = SendCommand("05", "72", FromUserName, 0)) != 2)
                    {
                        if (Send_Result == 0)
                            result = "正为您关闭厨房灯";
                        else
                            result = "厨房灯已经关闭了";
                    }
                    else
                        result = "操作失败了/::'(，节点好像断开了/:break";
                }
            }
            if (command.IndexOf("书房") != -1)
            {
                if ((command.IndexOf("开") != -1) & (command.IndexOf("关") != -1))
                    return result;
                else if (command.IndexOf("开") != -1)
                {
                    if ((Send_Result = SendCommand("06", "72", FromUserName, 1)) != 2)
                    {
                        if (Send_Result == 1)
                            result = "正为您打开书房灯";
                        else
                            result = "书房灯为打开状态";
                    }
                    else
                        result = "操作失败了/::'(，节点好像断开了/:break";
                }
                else if (command.IndexOf("关") != -1)
                {
                    if ((Send_Result = SendCommand("06", "72", FromUserName, 0)) != 2)
                    {
                        if (Send_Result == 0)
                            result = "正为您关闭书房灯";
                        else
                            result = "书房灯已经关闭了";
                    }
                    else
                        result = "操作失败了/::'(，节点好像断开了/:break";
                }
            }
            if (command.IndexOf("所有") != -1)
            {
                StringBuilder a = new StringBuilder();
                if ((command.IndexOf("开") != -1) & (command.IndexOf("关") != -1))
                    return result;
                else if (command.IndexOf("开") != -1)
                {
                    if ((Send_Result = SendCommand("01", "72", FromUserName, 1)) != 2)
                        a.Append("一楼主卧灯已打开！");
                    else
                        a.Append("一楼主卧灯节点未连接/:break");

                    if ((Send_Result = SendCommand("02", "72", FromUserName, 1)) != 2)
                        a.Append("\n二楼次卧灯已打开！");
                    else
                        a.Append("\n二楼次卧灯节点未连接/:break");

                    if ((Send_Result = SendCommand("03", "72", FromUserName, 1)) != 2)
                        a.Append("\n大厅灯已打开！");
                    else
                        a.Append("\n大厅灯节点未连接/:break");

                    if ((Send_Result = SendCommand("04", "72", FromUserName, 1)) != 2)
                        a.Append("\n楼梯灯已打开！");
                    else
                        a.Append("\n楼梯灯节点未连接/:break");

                    if ((Send_Result = SendCommand("05", "72", FromUserName, 1)) != 2)
                        a.Append("\n厨房灯已打开！");
                    else
                        a.Append("\n厨房灯节点未连接/:break");
                    
                    if ((Send_Result = SendCommand("06", "72", FromUserName, 1)) != 2)
                        a.Append("\n书房灯已打开！");
                    else
                        a.Append("\n书房灯节点未连接/:break");
                }
                else if (command.IndexOf("关") != -1)
                {
                    if ((Send_Result = SendCommand("01", "72", FromUserName, 0)) != 2)
                        a.Append("一楼主卧灯已关闭！");
                    else
                        a.Append("一楼主卧灯节点未连接/:break");

                    if ((Send_Result = SendCommand("02", "72", FromUserName, 0)) != 2)
                        a.Append("\n二楼次卧灯已关闭！");
                    else
                        a.Append("\n二楼次卧灯节点未连接/:break");

                    if ((Send_Result = SendCommand("03", "72", FromUserName, 0)) != 2)
                        a.Append("\n大厅灯已关闭！");
                    else
                        a.Append("\n大厅灯节点未连接/:break");

                    if ((Send_Result = SendCommand("04", "72", FromUserName, 0)) != 2)
                        a.Append("\n楼梯灯已关闭！");
                    else
                        a.Append("\n楼梯灯节点未连接/:break");

                    if ((Send_Result = SendCommand("05", "72", FromUserName, 0)) != 2)
                        a.Append("\n厨房灯已关闭！");
                    else
                        a.Append("\n厨房灯节点未连接/:break");

                    if ((Send_Result = SendCommand("06", "72", FromUserName, 0)) != 2)
                        a.Append("\n书房灯已关闭！");
                    else
                        a.Append("\n书房灯节点未连接/:break");
                }
                result = a.ToString();
            }
            return result;
        }
        private string Command_Control(string command, string FromUserName)
        {
            string result = "抱歉，现在还无法理解您的指令（" + command + "），不过我会努力的/:,@f";//"没有听明白这句指令：" + command;
            int Send_Result;
            if (command.IndexOf("灯") != -1)
            {
                result = Light_Control(command, FromUserName);
            }
            else if (command.IndexOf("窗帘") != -1)
            {
                if ((command.IndexOf("开") != -1) & (command.IndexOf("关") != -1))
                    return result;
                else if (command.IndexOf("开") != -1)
                {
                    if ((Send_Result = SendCommand("09", "72", FromUserName, 1)) != 2)
                    {
                        if (Send_Result == 1)
                            result = "正为您打开窗帘";
                        else
                            result = "窗帘为打开状态";
                    }
                    else
                        result = "操作失败了/::'(，节点好像断开了/:break";
                }
                else if (command.IndexOf("关") != -1)
                {
                    if ((Send_Result = SendCommand("09", "72", FromUserName, 0)) != 2)
                    {
                        if (Send_Result == 0)
                            result = "正为您关闭窗帘";
                        else
                            result = "窗帘已经关闭了";
                    }
                    else
                        result = "操作失败了/::'(，节点好像断开了/:break";
                }
            }
            else if (command.IndexOf("报警") != -1)
            {
                if ((command.IndexOf("开") != -1) & (command.IndexOf("关") != -1))
                    return result;
                else if (command.IndexOf("开") != -1)
                {
                    if ((Send_Result = SendCommand("0A", "72", FromUserName, 1)) != 2)
                    {
                        if (Send_Result == 1)
                            result = "正为您打开报警";
                        else
                            result = "报警为打开状态";
                    }
                    else
                        result = "操作失败了/::'(，节点好像断开了/:break";
                }
                else if (command.IndexOf("关") != -1)
                {
                    if ((Send_Result = SendCommand("0A", "72", FromUserName, 0)) != 2)
                    {
                        if (Send_Result == 0)
                            result = "正为您关闭报警";
                        else
                            result = "报警已经关闭了";
                    }
                    else
                        result = "操作失败了/::'(，节点好像断开了/:break";
                }
            }
            else if (command.IndexOf("摄像头") != -1)
            {
                if ((command.IndexOf("开") != -1) & (command.IndexOf("关") != -1))
                    return result;
                else if (command.IndexOf("开") != -1)
                {
                    if ((Send_Result = SendCommand("0C", "72", FromUserName, 1)) != 2)
                    {
                        if (Send_Result == 1)
                            result = "正为您打开摄像头";
                        else
                            result = "摄像头为打开状态";
                    }
                    else
                        result = "操作失败了/::'(，节点好像断开了/:break";
                }
                else if (command.IndexOf("关") != -1)
                {
                    if ((Send_Result = SendCommand("0C", "72", FromUserName, 0)) != 2)
                    {
                        if (Send_Result == 0)
                            result = "正为您关闭摄像头";
                        else
                            result = "摄像头已经关闭了";
                    }
                    else
                        result = "操作失败了/::'(，节点好像断开了/:break";
                }
            }
            else if (command.IndexOf("度") != -1)
            {
                if (command.IndexOf("温湿度") != -1)
                {
                    NodeStatus s = new NodeStatus();
                    s.Modle = "0B";
                    s = sm.GetListByPage(s);
                    result = "温度：" + int.Parse(s.Data.Substring(2, 2), NumberStyles.HexNumber) + "℃" + "\n湿度：" + int.Parse(s.Data.Substring(4, 2), NumberStyles.HexNumber) + "%" + "\n时间：" + s.Time;
                }
                else if (command.IndexOf("温度") != -1)
                {
                    NodeStatus s = new NodeStatus();
                    s.Modle = "0B";
                    s = sm.GetListByPage(s);
                    result = "温度：" + int.Parse(s.Data.Substring(2, 2), NumberStyles.HexNumber) + "℃" + "\n时间：" + s.Time;

                }
                else if (command.IndexOf("湿度") != -1)
                {
                    NodeStatus s = new NodeStatus();
                    s.Modle = "0B";
                    s = sm.GetListByPage(s);
                    result = "湿度：" + int.Parse(s.Data.Substring(4, 2), NumberStyles.HexNumber) + "%" + "\n时间：" + s.Time;
                }
            }
            else if (command.IndexOf("门") != -1)
            {
                if (command.IndexOf("房门") != -1)
                {
                    if ((command.IndexOf("开") != -1) || (command.IndexOf("关") != -1))
                    {
                        result = "房门不可以控制，只可查看状态！";
                    }
                    else
                    {
                        NodeStatus s3 = new NodeStatus();
                        s3.Modle = "0B";
                        s3 = sm.GetListByPage(s3);
                        if (s3.Data[1] == '1')
                            result = "房门：开" + "\n时间：" + s3.Time;
                        else
                            result = "房门：关" + "\n时间：" + s3.Time;
                    }
                }
                else if (command.IndexOf("车库门") != -1)
                {
                    if ((command.IndexOf("开") != -1) & (command.IndexOf("关") != -1))
                        return result;
                    else if (command.IndexOf("开") != -1)
                    {
                        if ((Send_Result = SendCommand("07", "72", FromUserName, 1)) != 2)
                        {
                            if (Send_Result == 1)
                                result = "正为您打开车库门";
                            else
                                result = "车库门为打开状态";
                        }
                        else
                            result = "操作失败了/::'(，节点好像断开了/:break";
                    }
                    else if (command.IndexOf("关") != -1)
                    {
                        if ((Send_Result = SendCommand("07", "72", FromUserName, 0)) != 2)
                        {
                            if (Send_Result == 0)
                                result = "正为您关闭车库门";
                            else
                                result = "车库门已经关闭了";
                        }
                        else
                            result = "操作失败了/::'(，节点好像断开了/:break";
                    }
                }
            }
            else if (command.IndexOf("光电") != -1)
            {
                NodeStatus s2 = new NodeStatus();
                s2.Modle = "0B";
                s2 = sm.GetListByPage(s2);
                if (s2.Data[0] == '1')
                    result = "光电闸：开" + "\n时间：" + s2.Time;
                else
                    result = "光电闸：关" + "\n时间：" + s2.Time;
            }
            else if ((command.IndexOf("所有") != -1) & (command.IndexOf("节点") != -1))
            {
                int count = 0;
                NodeStatus u = new NodeStatus();
                if ((count = sm.GetRecordCount(u)) > 0)
                {
                    List<NodeStatus> list = sm.getModelListAll(u);
                    result = create_node_list(list);
                    list.Clear();
                }
            }
            else if (command.IndexOf("你妹") != -1)
            {
                result = "/:shit/:shit/:shit";
            }
            return result;
        }
        private string create_node_list(List<NodeStatus> list)
        {
            StringBuilder sb = new StringBuilder();
            int i;
            for (i = 0; i < 11 && i < list.Count; i++)
            {
                int b = Convert.ToInt32(list[i].Modle, 16);
                if (b > 0 && b < 12)
                {
                    switch (b)
                    {
                        case 1:
                            sb.Append("\n主卧灯：");
                            if (list[i].Data == "000001")
                                sb.Append("开");
                            else
                                sb.Append("关");
                            break;
                        case 2:
                            sb.Append("\n次卧灯：");
                            if (list[i].Data == "000001")
                                sb.Append("开");
                            else
                                sb.Append("关");
                            break;
                        case 3:
                            sb.Append("\n大厅灯：");
                            if (list[i].Data == "000001")
                                sb.Append( "开");
                            else
                                sb.Append( "关");
                            break;
                        case 4:
                            sb.Append("\n楼梯灯：");
                            if (list[i].Data == "000001")
                                sb.Append( "开");
                            else
                                sb.Append( "关");
                            break;
                        case 5:
                            sb.Append("\n厨房灯：");
                            if (list[i].Data == "000001")
                                sb.Append( "开");
                            else
                                sb.Append( "关");
                            break;
                        case 6:
                            sb.Append("\n书房灯：");
                            if (list[i].Data == "000001")
                                sb.Append( "开");
                            else
                                sb.Append( "关");
                            break;
                        case 7:
                            sb.Append("\n车库门：");
                            if (list[i].Data == "000001")
                                sb.Append( "开");
                            else
                                sb.Append( "关");
                            break;
                        case 8:
                            sb.Append("\n遥控器：    ");
                            break;
                        case 9:
                            sb.Append("\n    窗帘：");
                            if (list[i].Data == "000001")
                                sb.Append( "开");
                            else
                                sb.Append( "关");
                            break;
                        case 10:
                            sb.Append("\n    报警：");
                            if (list[i].Data == "000001")
                                sb.Append( "开");
                            else
                                sb.Append("关");
                            break;
                        case 11:
                            sb.Append("\n检测杠：");
                            if (list[i].Data[0] == '1')
                                sb.Append("开");
                            else
                                sb.Append("关");
                            if (list[i].Time.CompareTo(DateTime.Now.AddSeconds(-30)) > 0)
                            {
                                sb.Append( "   online");
                            }
                            else
                                sb.Append("   offline");

                            sb.Append("\n    门磁：");
                            if (list[i].Data[1] == '1')
                                sb.Append( "开");
                            else
                                sb.Append( "关");
                            if (list[i].Time.CompareTo(DateTime.Now.AddSeconds(-30)) > 0)
                            {
                                sb.Append( "   online");
                            }
                            else
                                sb.Append("   offline");

                            sb.Append("\n    温度：");
                            sb.Append( int.Parse(list[i].Data.Substring(2, 2), NumberStyles.HexNumber) + "℃");
                            if (list[i].Time.CompareTo(DateTime.Now.AddSeconds(-30)) > 0)
                            {
                                sb.Append( "  online");
                            }
                            else
                                sb.Append("   offline");

                            sb.Append("\n    湿度：");
                            sb.Append( int.Parse(list[i].Data.Substring(4, 2), NumberStyles.HexNumber) + "%");
                            break;
                    }
                    if (list[i].Time.CompareTo(DateTime.Now.AddSeconds(-10)) > 0)
                    {
                        sb.Append("   online");
                    }
                    else
                        sb.Append("   offline");
                }
            }
            return sb.ToString();
        }
    }

    //回复类型
    public class ReplyType
    {
        /// <summary>
        /// 普通文本消息
        /// </summary>
        public static string Message_Text
        {
            get
            {
                return @"<xml>
                            <ToUserName><![CDATA[{0}]]></ToUserName>
                            <FromUserName><![CDATA[{1}]]></FromUserName>
                            <CreateTime>{2}</CreateTime>
                            <MsgType><![CDATA[text]]></MsgType>
                            <Content><![CDATA[{3}]]></Content>
                            </xml>";
            }
        }
        /// <summary>
        /// 图片消息
        /// </summary>
        public static string Message_Image
        {
            get
            {
                return @"<xml>
                            <ToUserName><![CDATA[{0}]]></ToUserName>
                            <FromUserName><![CDATA[{1}]]></FromUserName>
                            <CreateTime>{2}</CreateTime>
                            <MsgType><![CDATA[image]]></MsgType>
                            <Image>
                            <MediaId><![CDATA[{3}]]></MediaId>
                            </Image>
                            </xml>";
            }
        }
        public static string Message_Music
        {
            get
            {
                return @"<xml>
                            <ToUserName><![CDATA[{0}]]></ToUserName>
                            <FromUserName><![CDATA[{1}]]></FromUserName>
                            <CreateTime>{2}</CreateTime>
                            <MsgType><![CDATA[music]]></MsgType>
                            <Music>
                            <Title><![CDATA[{3}]]></Title>
                            <Description><![CDATA[{4}]]></Description>
                            <MusicUrl><![CDATA[{5}]]></MusicUrl>
                            <HQMusicUrl><![CDATA[{6}]]></HQMusicUrl>
                            </Music>
                            </xml>";
            }
        }
        /// <summary>
        /// 图文消息主体
        /// </summary>
        public static string Message_News_Main
        {
            get
            {
                return @"<xml>
                            <ToUserName><![CDATA[{0}]]></ToUserName>
                            <FromUserName><![CDATA[{1}]]></FromUserName>
                            <CreateTime>{2}</CreateTime>
                            <MsgType><![CDATA[news]]></MsgType>
                            <ArticleCount>{3}</ArticleCount>
                            <Articles>
                            {4}
                            </Articles>
                            </xml> ";
            }
        }
        /// <summary>
        /// 图文消息项
        /// </summary>
        public static string Message_News_Item
        {
            get
            {
                return @"<item>
                            <Title><![CDATA[{0}]]></Title> 
                            <Description><![CDATA[{1}]]></Description>
                            <PicUrl><![CDATA[{2}]]></PicUrl>
                            <Url><![CDATA[{3}]]></Url>
                            </item>";
            }
        }
       
    }
}
