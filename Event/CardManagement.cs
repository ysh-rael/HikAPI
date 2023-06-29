using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace HikAPI.Event
{
    using HikAPI.HttpClient;
    using HikAPI.Public.CardManagement;
    using System.Dynamic;

    internal static class CardManagement
    {
        public static bool Get(Device SelfDevice, string CardNo)
        {
            string szUrl = "/ISAPI/AccessControl/CardInfo/Search?format=json";
            string szResponse = string.Empty;
            string szRequest = "{\"CardInfoSearchCond\":{\"searchID\":\"0\",\"searchResultPosition\":0,\"maxResults\":30,\"CardNoList\":[{\"cardNo\":\"" + CardNo + "\"}]}}";
            string szMethod = "POST";

            szResponse = HttpClient.ActionISAPI(szUrl, szRequest, szMethod, SelfDevice);
            if (szResponse != string.Empty)
            {
                CardInfoSearchRoot cs = JsonConvert.DeserializeObject<CardInfoSearchRoot>(szResponse);
                if (cs.CardInfoSearch.responseStatusStrg.Equals("OK"))
                {
                    Console.WriteLine("Get Card Succ!");
                    foreach (CardInfo info in cs.CardInfoSearch.CardInfo)
                    {
                        Console.WriteLine($"employeeNo: {info.employeeNo}");
                        Console.WriteLine($"cardType: {info.cardType}");
                        break;
                    }
                    return true;
                }
                else if (cs.CardInfoSearch.responseStatusStrg.Equals("NO MATCH"))
                {
                    Console.WriteLine("CardNo not exist!");
                }
                else
                {
                    Console.WriteLine(cs.CardInfoSearch.responseStatusStrg);
                }
            }
            return false;
        }

        public static bool Delete(Device SelfDevice, string CardNo)
        {
            string szUrl = "/ISAPI/AccessControl/CardInfo/Delete?format=json";
            string szResponse = string.Empty;
            string szRequest = "{\"CardInfoDelCond\":{\"CardNoList\":[{\"cardNo\":\"" + CardNo + "\"}]}}";
            string szMethod = "PUT";

            szResponse = HttpClient.ActionISAPI(szUrl, szRequest, szMethod, SelfDevice);

            if (szResponse != string.Empty)
            {
                ResponseStatus rs = JsonConvert.DeserializeObject<ResponseStatus>(szResponse);
                if (rs.statusString.Equals("OK"))
                {
                    Console.WriteLine("Delete Card Succ!");
                    return true;
                }
                else
                {
                    Console.WriteLine(rs.statusString);
                }
            }
            return false;
        }

        public static bool Set(Device SelfDevice, string EmployeeNo, string CardNo, string CardType)
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
                    szUrl = "/ISAPI/AccessControl/UserInfo/SetUp?format=json";

                    szResponse = string.Empty;
                    szRequest = "{\"UserInfo\":{\"employeeNo\":\"" + EmployeeNo +
                    "\",\"userType\":\"normal\",\"Valid\":{\"enable\":true,\"beginTime\":\"2017-08-01T17:30:08\",\"endTime\":\"2020-08-01T17:30:08\",\"timeType\":\"local\"}}}";
                    szMethod = "PUT";

                    szResponse = HttpClient.ActionISAPI(szUrl, szRequest, szMethod, SelfDevice);

                    if (szResponse != string.Empty)
                    {
                        ResponseStatus rs = JsonConvert.DeserializeObject<ResponseStatus>(szResponse);
                        if (1 != rs.statusCode)
                        {
                            Console.WriteLine(rs.errorMsg);
                            return false;
                        }
                    }
                }

                szUrl = "/ISAPI/AccessControl/CardInfo/SetUp?format=json";
                szResponse = string.Empty;
                szRequest = "{\"CardInfo\":{\"employeeNo\":\"" + EmployeeNo +
                    "\",\"cardNo\":\"" + CardNo +
                    "\",\"cardType\":\"" + CardType + "\"}}";
                szMethod = "PUT";

                szResponse = HttpClient.ActionISAPI(szUrl, szRequest, szMethod, SelfDevice);
                if (szResponse != string.Empty)
                {
                    ResponseStatus rs = JsonConvert.DeserializeObject<ResponseStatus>(szResponse);
                    if (1 == rs.statusCode)
                    {
                        Console.WriteLine("Add Card Succ!");
                        return true;
                    }
                    else
                    {
                        Console.WriteLine(rs.errorMsg);
                    }
                }
            }
            return false;
        }

    }
}
