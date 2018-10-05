using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;


namespace GlassDetctionSystem.Model.FunctionClass.XML
{
    public class XMLParser
    {
        XmlDocument doc;
        private static XMLParser instance = null;

        public static XMLParser getInstance()
        {
            if (instance == null)
            {
                instance = new XMLParser(System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "detect.xml");
            }
            return instance;
        }

        private XMLParser(string path)
        {
            doc = new XmlDocument();
            doc.Load(path);
        }

        /// <summary>
        /// 获取指定值
        /// </summary>
        /// <param name="xpath">XPath表达式</param>
        /// <returns></returns>
        public string get(string xpath)
        {
            return doc.SelectSingleNode(xpath).InnerText;
        }

        /// <summary>
        /// 修改元素的值
        /// </summary>
        /// <param name="xpath">XPath表达式</param>
        /// <param name="newValue">新值</param>
        public void set(string xpath, string newValue)
        {
            doc.SelectSingleNode(xpath).InnerText = newValue;
            doc.Save(System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "detect.xml");
        }
        /// <summary>
        /// 获取指定节点下所有子节点的id属性值
        /// </summary>
        /// <param name="xpath"></param>
        /// <param name="count"></param>
        /// <param name="AllChildNodes"></param>
        public void getAllNodes(string xpath,out int count,out Dictionary<int,string> AllChildNodes)
        {
            Dictionary<int, string> ChildNodes=new Dictionary<int, string> { };
            XmlNode root = doc.SelectSingleNode(xpath);
            XmlNodeList childlist=null;
            if (root != null)
            {
                childlist = root.ChildNodes;
                count = childlist.Count;
            }
            else
            {
                count = 0;
            }
           
            
            for (int i = 0; i <count; i++)
            {
                ChildNodes.Add(i, childlist[i].Attributes["id"].Value);
            }
            AllChildNodes = ChildNodes;


        }
        /// <summary>
        /// 在指定节点下添加带有id属性的子节点
        /// </summary>
        /// <param name="xpath"></param>
        /// <param name="nodename"></param>
        /// <param name="id"></param>
        public void addNodeWithID(string xpath, string nodename, string id)
        {
             XmlNode node= doc.SelectSingleNode(xpath);

            XmlNode child  = doc.CreateNode(XmlNodeType.Element, nodename, null) ;
            XmlAttribute xa = doc.CreateNode(XmlNodeType.Attribute, "id", null) as XmlAttribute;
            xa.Value = id;

            child.Attributes.Append(xa);
            //XmlNode childend = doc.CreateNode(XmlNodeType.EndElement, nodename, null);
            //child.SetAttribute("id", id);
            node.AppendChild(child);
            //node.AppendChild(childend);
            doc.Save(System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "detect.xml");

        }
        /// <summary>
        /// 在指定节点下添加子节点及内容
        /// </summary>
        /// <param name="xpath"></param>
        /// <param name="nodename"></param>
        /// <param name="text"></param>
        public void addNodeWithText(string xpath, string nodename, string text)
        {
            XmlNode node = doc.SelectSingleNode(xpath);

            XmlElement child = doc.CreateElement(nodename);
            child.InnerText = text;
            node.AppendChild(child);
            doc.Save(System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "detect.xml");

        }
        /// <summary>
        /// 在指定节点下添加子节点
        /// </summary>
        /// <param name="xpath"></param>
        /// <param name="nodename"></param>
        public void addNode(string xpath, string nodename)
        {
            XmlNode node = doc.SelectSingleNode(xpath);
            XmlElement child = doc.CreateElement(nodename);
            node.AppendChild(child);
            doc.Save(System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "detect.xml");
        }
    }
}
