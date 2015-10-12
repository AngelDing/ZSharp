using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;

namespace ZSharp.Framework.MongoDb.Managers
{
    public class ServerContext
    {
        private XDocument XML { get; set; }
        private string ConfigFile
        {
            get { return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data/servers.xml"); }
        }

        public ServerContext()
        {
            XML = XDocument.Load(ConfigFile);
        }

        /// <summary>
        /// 获取服务器列表
        /// </summary>
        /// <returns></returns>
        public List<ServerModel> GetServerNodes()
        {
            var list = new List<ServerModel>();
            XML.Descendants("Server").ToList().ForEach(item =>
            {
                list.Add(new ServerModel
                {
                    Id = ConstHelper.GetRandomId(),
                    IP = item.Attribute("IP").Value,
                    Port = item.Attribute("Port").Value
                });
            });
            return list;
        }

        public void AddServer(string ip, int port)
        {
            var element = new XElement("Server", new XAttribute("IP", ip), new XAttribute("Port", port));
            XML.Root.Add(element);
            XML.Save(ConfigFile);

            CacheHelper.Clear();
        }

        public void DeleteServer(string ip, int port)
        {
            var server = XML.Descendants("Server").ToList()
                .Find(p => p.Attribute("IP").Value == ip && p.Attribute("Port").Value == port.ToString());
            if (server != null)
            {
                server.Remove();
                XML.Save(ConfigFile);
                CacheHelper.Clear();
            }
        }
    }
}
