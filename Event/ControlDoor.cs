using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace HikAPI.Event
{
    using HikAPI.HttpClient;
    internal static class ControlDoor
    {
        public static bool Open(Device SelfDevice)
        {
            string szUrl = "/ISAPI/AccessControl/RemoteControl/door/65535";
            string szResponse = string.Empty;
            string szRequest = "<RemoteControlDoor version=\"2.0\" xmlns=\"http://www.isapi.org/ver20/XMLSchema\"><cmd>open</cmd></RemoteControlDoor>";
            string szMethod = "PUT";

            szResponse = HttpClient.ActionISAPI(szUrl, szRequest, szMethod, SelfDevice);
            if (szResponse != string.Empty)
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(szResponse);
                XmlNode rootNode = xmlDoc.DocumentElement;
                for (int i = 0; i < rootNode.ChildNodes.Count; i++)
                {
                    if (rootNode.ChildNodes[i].Name.Equals("statusString"))
                    {
                        Console.WriteLine("OPEN DOOR " + rootNode.ChildNodes[i].InnerText);
                        return true;
                    }
                }
            }
            return false;
        }

        public static bool Close(Device SelfDevice)
        {
            string szUrl = "/ISAPI/AccessControl/RemoteControl/door/65535";
            string szResponse = string.Empty;
            string szRequest = "<RemoteControlDoor version=\"2.0\" xmlns=\"http://www.isapi.org/ver20/XMLSchema\"><cmd>close</cmd></RemoteControlDoor>";
            string szMethod = "PUT";

            szResponse = HttpClient.ActionISAPI(szUrl, szRequest, szMethod, SelfDevice);
            if (szResponse != string.Empty)
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(szResponse);
                XmlNode rootNode = xmlDoc.DocumentElement;
                for (int i = 0; i < rootNode.ChildNodes.Count; i++)
                {
                    if (rootNode.ChildNodes[i].Name.Equals("statusString"))
                    {
                        Console.WriteLine("CLOSE DOOR " + rootNode.ChildNodes[i].InnerText);
                        return true;
                    }
                }
            }
            return false;
        }

        public static bool StayOpen(Device SelfDevice)
        {
            string szUrl = "/ISAPI/AccessControl/RemoteControl/door/65535";//全部门
            string szResponse = string.Empty;
            string szRequest = "<RemoteControlDoor version=\"2.0\" xmlns=\"http://www.isapi.org/ver20/XMLSchema\"><cmd>alwaysOpen</cmd></RemoteControlDoor>";
            string szMethod = "PUT";

            szResponse = HttpClient.ActionISAPI(szUrl, szRequest, szMethod, SelfDevice);
            if (szResponse != string.Empty)
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(szResponse);
                Console.WriteLine(szResponse);
                XmlNode rootNode = xmlDoc.DocumentElement;
                for (int i = 0; i < rootNode.ChildNodes.Count; i++)
                {
                    if (rootNode.ChildNodes[i].Name.Equals("statusString"))
                    {
                        Console.WriteLine("STAYOPEN DOOR " + rootNode.ChildNodes[i].InnerText);
                        return true;
                    }
                }
            }
            return false;
        }

        public static bool StayClose(Device SelfDevice)
        {
            string szUrl = "/ISAPI/AccessControl/RemoteControl/door/65535";//全部门            
            string szResponse = string.Empty;
            string szRequest = "<RemoteControlDoor version=\"2.0\" xmlns=\"http://www.isapi.org/ver20/XMLSchema\"><cmd>alwaysClose</cmd></RemoteControlDoor>";
            string szMethod = "PUT";

            szResponse = HttpClient.ActionISAPI(szUrl, szRequest, szMethod, SelfDevice);
            if (szResponse != string.Empty)
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(szResponse);
                XmlNode rootNode = xmlDoc.DocumentElement;
                for (int i = 0; i < rootNode.ChildNodes.Count; i++)
                {
                    if (rootNode.ChildNodes[i].Name.Equals("statusString"))
                    {
                        Console.WriteLine("STAYCLOSE DOOR " + rootNode.ChildNodes[i].InnerText);
                        return true;
                    }
                }
            }
            return false;
        }

    }
}
