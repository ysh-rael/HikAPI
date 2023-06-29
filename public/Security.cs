using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HikAPI.Security
{
    using HikAPI.DeviceInfo;
    using HikAPI.HttpClient;
    internal class Security
    {
        public static bool Login(DeviceInfo struDeviceInfo)
        {
            string strUsername = struDeviceInfo.strUsername;
            string strPassword = struDeviceInfo.strPassword;
            string strDeviceIp = struDeviceInfo.strDeviceIP;
            string strHttpMethod = "GET";
            string strUrl = "http://" + strDeviceIp + ":" + struDeviceInfo.strHttpPort + "/ISAPI/Security/userCheck";
            string strResponse = string.Empty;

            HttpClient client = new HttpClient();
            int iRet = client.HttpRequest(strUsername, strPassword, strUrl, strHttpMethod, ref strResponse);

            if (iRet == (int)HttpClient.HttpStatus.Http200)
            {
                if (strResponse != string.Empty)
                {
                    Console.WriteLine(struDeviceInfo.strDeviceNickName + " Login success: " + strDeviceIp);
                    return true;
                }
            }
            else if (iRet == (int)HttpClient.HttpStatus.HttpOther)
            {
                string statusCode = string.Empty;
                string statusString = string.Empty;
                client.ParserUserCheck(strResponse, ref statusCode, ref statusString);
                Console.WriteLine(struDeviceInfo.strDeviceNickName + " Login failed!  Error Code:" + statusCode + "  Describe:" + statusString);
            }
            else if (iRet == (int)HttpClient.HttpStatus.HttpTimeOut)
            {
                Console.WriteLine(struDeviceInfo.strDeviceNickName + " Login failed!  Describe: " + strResponse);
            }
            return false;
        }
    }
}
