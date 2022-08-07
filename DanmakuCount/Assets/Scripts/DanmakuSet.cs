using System.Collections.Generic;
using System.Xml;
using UnityEngine;

namespace CitrusDammakuCount
{
    public class DanmakuSet
    {
        public HashSet<Danmaku> danmakus = new HashSet<Danmaku>();

        public DanmakuSet(params XmlDocument[] xmlDocuments)
        {
            foreach (var xmlDocument in xmlDocuments)
            {
                XmlNodeList childNodes = xmlDocument.ChildNodes;
                for (int i = 0; i < childNodes.Count; i++)
                {
                    XmlNode xmlNode = childNodes.Item(i);
                    if(xmlNode.Name.Equals("i"))
                    {
                        for (int j = 0; j < xmlNode.ChildNodes.Count; j++)
                        {
                            XmlNode xmlNodeI = xmlNode.ChildNodes.Item(j);
                            if (xmlNodeI.Name.Equals("d"))
                            {
                                Danmaku danmaku = new Danmaku(xmlNodeI.InnerText, xmlNodeI.Attributes["p"].Value);
                                //if (danmakus.Contains(danmaku))
                                //    Debug.Log(danmaku.content);
                                danmakus.Add(danmaku);
                            }
                        }
                    }
                }
            }
        }
    }
}