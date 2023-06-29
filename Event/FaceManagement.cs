using HikAPI.Public.FaceManagement;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HikAPI.Event
{
    using HikAPI.HttpClient;
    using System.IO;

    internal class FaceManagement
    {
        public static bool Delete(Device SelfDevice, string EmployeeNo, string FaceType, int FDID)
        {
            string szUrl = "/ISAPI/Intelligent/FDLib/FDSearch/Delete?format=json&FDID=" + FDID + "&faceLibType=" + FaceType + "";
            string szResponse = string.Empty;
            string szRequest = "{\"FPID\":[{\"value\":\"" + EmployeeNo + "\"}]}";
            string szMethod = "PUT";

            szResponse = HttpClient.ActionISAPI(szUrl, szRequest, szMethod, SelfDevice);
            if (szResponse != string.Empty)
            {
                ResponseStatus rs = JsonConvert.DeserializeObject<ResponseStatus>(szResponse);
                if (rs.statusCode.Equals("1"))
                {
                    Console.WriteLine("Delete Picture Succ!");
                    return true;
                }
                else
                {
                    Console.WriteLine("Delete face data failed! Error code:" + rs.subStatusCode);
                }
            }
            return false;
        }

        public static string Get(Device SelfDevice, string EmployeeNo, string FaceType, int FDID)
        {
            string szUrl = "/ISAPI/AccessControl/UserInfo/Search?format=json";
            string szResponse = string.Empty;
            string szRequest = "{\"UserInfoSearchCond\":{\"searchID\":\"1\",\"searchResultPosition\":0,\"maxResults\":30,\"EmployeeNoList\":[{\"employeeNo\":\"" + EmployeeNo + "\"}]}}";
            string szMethod = "POST";


            szResponse = HttpClient.ActionISAPI(szUrl, szRequest, szMethod, SelfDevice);
            if (szResponse != string.Empty)
            {
                UserInfoSearchRoot us = JsonConvert.DeserializeObject<UserInfoSearchRoot>(szResponse);
                if (0 == us.UserInfoSearch.totalMatches)
                {
                    Console.WriteLine("Employee No isn't found!");
                    return "";
                }
            }

            szUrl = "/ISAPI/Intelligent/FDLib?format=json&FDID=" + FDID + "&faceLibType=" + FaceType;
            szResponse = string.Empty;
            szMethod = "GET";

            szResponse = HttpClient.ActionISAPI(szUrl, szRequest, szMethod, SelfDevice);
            if (szResponse != string.Empty)
            {
                FaceLib fb = JsonConvert.DeserializeObject<FaceLib>(szResponse);
                if (fb == null || fb.statusCode != 1)
                {
                    Console.WriteLine("FaceLib isn't existed!");
                    return "";
                }
            }

            szUrl = "/ISAPI/Intelligent/FDLib/FDSearch?format=json";
            szResponse = string.Empty;
            szRequest = "{\"searchResultPosition\":0,\"maxResults\":5,\"faceLibType\":\"" + FaceType +
                "\",\"FDID\":\"" + FDID +
                "\",\"FPID\":\"" + EmployeeNo + "\"}";
            szMethod = "POST";

            szResponse = HttpClient.ActionISAPI(szUrl, szRequest, szMethod, SelfDevice);
            if (szResponse != string.Empty)
            {
                Root rt = JsonConvert.DeserializeObject<Root>(szResponse);
                if (rt.statusCode == 1)
                {
                    Console.WriteLine("Get picture succ!");
                    if (rt.totalMatches == 1)
                    {
                        string picData = string.Empty;
                        foreach (MatchListItem item in rt.MatchList)
                        {
                            picData = item.modelData;
                            string strPath = string.Format("1.jpg");

                            string url = item.faceURL;
                            string data = HttpClient.ActionISAPI(url, szRequest, "GET", SelfDevice);
                            return WriteFaceData(data); // Salva imagem
                        }
                    }
                    else
                    {
                        Console.WriteLine("Picture isn't found!");
                    }
                }
            }
            return "";
        }

        public static bool Set(Device SelfDevice, string EmployeeNo, string FaceType, int FDID, string FilePath)
        {
            string szUrl = "/ISAPI/AccessControl/UserInfo/Search?format=json";
            string szResponse = string.Empty;
            string szRequest = "{\"UserInfoSearchCond\":{\"searchID\":\"1\",\"searchResultPosition\":0,\"maxResults\":30,\"EmployeeNoList\":[{\"employeeNo\":\"" + EmployeeNo + "\"}]}}";
            string szMethod = "POST";

            //查询是否存在工号
            szResponse = HttpClient.ActionISAPI(szUrl, szRequest, szMethod, SelfDevice);

            if (szResponse != string.Empty)
            {
                UserInfoSearchRoot us = JsonConvert.DeserializeObject<UserInfoSearchRoot>(szResponse);
                if (0 == us.UserInfoSearch.totalMatches)
                {
                    Console.WriteLine("Employee No isn't found!");
                    return false;
                }
            }

            szUrl = "/ISAPI/Intelligent/FDLib?format=json&FDID=" + FDID + "&faceLibType=" + FaceType;
            szResponse = string.Empty;
            szMethod = "GET";

            //查询FaceLib是否存在
            szResponse = HttpClient.ActionISAPI(szUrl, szRequest, szMethod, SelfDevice);
            if (szResponse != string.Empty)
            {
                FaceLib fb = JsonConvert.DeserializeObject<FaceLib>(szResponse);
                if (fb == null || fb.statusCode != 1)
                {
                    Console.WriteLine("FaceLib isn't existed!");
                    return false;
                }
            }
            //查询是否已有图片，若有则删除
            szUrl = "/ISAPI/Intelligent/FDLib/FDSearch?format=json";

            szResponse = string.Empty;
            szRequest = "{\"searchResultPosition\":0,\"maxResults\":30,\"faceLibType\":\"" + FaceType +
                "\",\"FDID\":\"" + FDID +
                "\",\"FPID\":\"" + EmployeeNo + "\"}";
            szMethod = "POST";

            szResponse = HttpClient.ActionISAPI(szUrl, szRequest, szMethod, SelfDevice);
            if (szResponse != string.Empty)
            {
                Root rt = JsonConvert.DeserializeObject<Root>(szResponse);
                if (rt.statusCode == 1)
                {
                    if (rt.totalMatches != 0)
                    {
                        szUrl = "/ISAPI/Intelligent/FDLib/FDSearch/Delete?format=json&FDID=" + FDID + "&faceLibType=" + FaceType + "";
                        szResponse = string.Empty;
                        szRequest = "{\"FPID\":[{\"value\":\"" + EmployeeNo + "\"}]}";
                        szMethod = "PUT";

                        szResponse = HttpClient.ActionISAPI(szUrl, szRequest, szMethod, SelfDevice);
                        if (szResponse != string.Empty)
                        {
                            ResponseStatus rs = JsonConvert.DeserializeObject<ResponseStatus>(szResponse);
                            if (!rs.statusCode.Equals("1"))
                            {
                                return false;
                            }
                        }
                    }
                }
            }


            szUrl = "/ISAPI/Intelligent/FDLib/FaceDataRecord?format=json";

            if (!szUrl.Substring(0, 4).Equals("http"))
            {
                szUrl = "http://" + SelfDevice.struDeviceInfo.strDeviceIP + ":" + SelfDevice.struDeviceInfo.strHttpPort + szUrl;
            }
            HttpClient clHttpClient = new HttpClient();
            szResponse = string.Empty;
            szRequest = "{\"faceLibType\":\"" + FaceType +
                "\",\"FDID\":\"" + FDID +
                "\",\"FPID\":\"" + EmployeeNo + "\"}";
            szResponse = clHttpClient.HttpPostData(SelfDevice.struDeviceInfo.strUsername, SelfDevice.struDeviceInfo.strPassword, szUrl, FilePath, szRequest);
            ResponseStatus res = JsonConvert.DeserializeObject<ResponseStatus>(szResponse);
            if (res != null && res.statusCode.Equals("1"))
            {
                Console.WriteLine("Set Picture Succ!");
                return true;
            }
            Console.WriteLine("Set Picture Failed!");
            return false;
        }

        private static string WriteFaceData(string faceData, string path = "")
        {
            string szPath = null;
            DateTime now = DateTime.Now;

            Console.WriteLine("faceData.Length: " + faceData.Length.ToString());
            byte[] byFaceData = Encoding.Default.GetBytes(faceData);

            string folder = string.Format("{0}\\{1}", Environment.CurrentDirectory, "output");
            if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);

            szPath = string.Format("{0}\\{1}", folder, path == "" ? $"faceData-{now.Ticks}.jpg" : path);
            try
            {
                using (FileStream fs = new FileStream(szPath, FileMode.OpenOrCreate))
                {
                    if (!File.Exists(szPath))
                        Console.WriteLine("FaceData storage file create failed！");

                    BinaryWriter objBinaryWrite = new BinaryWriter(fs);
                    fs.Write(byFaceData, 0, byFaceData.Length);
                    fs.Close();
                }
                Console.WriteLine("FaceData GET SUCCEED", "SUCCESSFUL");
                return szPath;
            }
            catch
            {
                Console.WriteLine("FaceData process failed");
                return "";
            }
        }
    }
}
