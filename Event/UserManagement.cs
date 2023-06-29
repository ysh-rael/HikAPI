using HikAPI.Public.FaceManagement;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace HikAPI.Event
{
    using HikAPI.HttpClient;

    internal class UserManagement
    {
        public class FingerPrintDeleteProcess
        {
            public string Status { get; set; }
        }
        public class FPDel
        {
            public FingerPrintDeleteProcess FingerPrintDeleteProcess { get; set; }
        }

        public static bool Get(Device SelfDevice, string EmployeeNo)
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
                }
                else
                {
                    Console.WriteLine("Get employee data succ!");
                    Console.WriteLine(szResponse);
                    /*                    foreach (UserInfoItem userInfo in us.UserInfoSearch.UserInfo)
                                        {
                                            textBoxName.Text = userInfo.name;
                                            foreach (RightPlanItem item in userInfo.RightPlan)
                                            {
                                                textBoxRightPlan.Text = item.planTemplateNo.ToString();
                                                break;
                                            }
                                            break;
                                        }
                    */
                    return true;
                }
            }
            return false;
        }

        public static bool Set(Device SelfDevice, string EmployeeNo, string Name, string UserType, string PlanTemplete)
        {
            string szUrl = "/ISAPI/AccessControl/UserInfo/SetUp?format=json";
            string szRequest = "{\"UserInfo\":{\"employeeNo\":\"" + EmployeeNo +
                "\",\"name\":\"" + Name +
                "\",\"userType\":\"normal\",\"Valid\":{\"enable\":true,\"beginTime\":\"2017-08-01T17:30:08\",\"endTime\":\"2020-08-01T17:30:08\"},\"doorRight\": \"1\",\"RightPlan\":[{\"doorNo\":1,\"planTemplateNo\":\"" + PlanTemplete + "\"}]}}";
            string szMethod = "PUT";

            string szResponse = HttpClient.ActionISAPI(szUrl, szRequest, szMethod, SelfDevice);
            if (szResponse != string.Empty)
            {
                Console.WriteLine(szResponse);
                ResponseStatus rs = JsonConvert.DeserializeObject<ResponseStatus>(szResponse);
                if ("1" == rs.statusCode)
                {
                    Console.WriteLine("Set UserInfo Succ!");
                    return true;
                }
                else
                {
                    Console.WriteLine(rs.errorMsg);
                }
            }
            return false;
        }

        public static bool Delete(Device SelfDevice, string EmployeeNo)
        {
            #region Deleta os card
            string szUrl = "/ISAPI/AccessControl/CardInfo/Delete?format=json";
            string szResponse = string.Empty;
            string szRequest = "{\"CardInfoDelCond\":{\"EmployeeNoList\":[{\"employeeNo\":\"" + EmployeeNo + "\"}]}}";
            string szMethod = "PUT";

            szResponse = HttpClient.ActionISAPI(szUrl, szRequest, szMethod, SelfDevice);
            if (szResponse != string.Empty)
            {
                ResponseStatus rs = JsonConvert.DeserializeObject<ResponseStatus>(szResponse);
                if (rs.statusString.Equals("OK"))
                {
                    Console.WriteLine("Delete Card Succ!");
                }
                else
                {
                    Console.WriteLine(rs.statusString);
                    return false;
                }
            }

            #endregion

            #region Deleta biometria
            szUrl = "/ISAPI/AccessControl/FingerPrint/Delete?format=json";
            szResponse = string.Empty;
            szRequest = "{\"FingerPrintDelete\":{\"mode\":\"byEmployeeNo\",\"EmployeeNoDetail\":{\"employeeNo\":\"" + EmployeeNo +
            "\"}}}";
            szMethod = "PUT";
            szResponse = HttpClient.ActionISAPI(szUrl, szRequest, szMethod, SelfDevice);
            if (szResponse != string.Empty)
            {
                ResponseStatus rs = JsonConvert.DeserializeObject<ResponseStatus>(szResponse);
                if (rs.errorCode == "1")
                {
                    while (true)
                    {
                        szUrl = "/ISAPI/AccessControl/FingerPrint/DeleteProcess?format=json";
                        szResponse = string.Empty;
                        byte[] byResponsee = { 0 };
                        szMethod = "GET";
                        string szContentType = string.Empty;

                        szResponse = HttpClient.ActionISAPI(szUrl, szRequest, szMethod, SelfDevice);
                        if (szResponse != string.Empty)
                        {
                            FPDel fd = JsonConvert.DeserializeObject<FPDel>(szResponse);
                            if (fd.FingerPrintDeleteProcess.Status.Equals("success") || fd.FingerPrintDeleteProcess.Status.Equals("failed"))
                            {
                                Console.WriteLine("Delete Finger Data " + fd.FingerPrintDeleteProcess.Status);
                                break;
                            }
                        }
                    }
                }
            }
            #endregion

            #region Deleta FaceId
            szUrl = "/ISAPI/Intelligent/FDLib/FDSearch/Delete?format=json&FDID=1&faceLibType=blackFD";//默认人脸库为(1,blackFD)
            szResponse = string.Empty;
            szRequest = "{\"FPID\":[{\"value\":\"" + EmployeeNo + "\"}]}";
            szMethod = "PUT";

            szResponse = HttpClient.ActionISAPI(szUrl, szRequest, szMethod, SelfDevice);
            if (szResponse != string.Empty)
            {
                ResponseStatus rs = JsonConvert.DeserializeObject<ResponseStatus>(szResponse);
                if (rs.statusCode == "1")
                {
                    Console.WriteLine("Delete Picture Succ!");
                }
                else
                {
                    Console.WriteLine(rs.statusString);
                }
            }

            #endregion

            #region Deleta Employee

            szUrl = "/ISAPI/AccessControl/UserInfo/Delete?format=json";
            szResponse = string.Empty;
            szRequest = "{\"UserInfoDelCond\":{\"EmployeeNoList\":[{\"employeeNo\":\"" + EmployeeNo + "\"}]}}";
            szMethod = "PUT";

            szResponse = HttpClient.ActionISAPI(szUrl, szRequest, szMethod, SelfDevice);
            if (szResponse != string.Empty)
            {
                ResponseStatus rs = JsonConvert.DeserializeObject<ResponseStatus>(szResponse);
                if (rs.statusString.Equals("OK"))
                {
                    Console.WriteLine("Delete Employee No Succ!");
                    return true;
                }
                else
                {
                    Console.WriteLine(rs.statusString);
                }
            }

            #endregion
            return false;
        }

    }
}
