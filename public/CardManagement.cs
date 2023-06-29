using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HikAPI.Public.CardManagement
{
    public class ResponseStatus
    {



        public string requestURL { get; set; }



        public int statusCode { get; set; }



        public string statusString { get; set; }



        public string subStatusCode { get; set; }



        public int errorCode { get; set; }



        public string errorMsg { get; set; }
    }

    public class CardInfo
    {
        public string employeeNo { get; set; }
        public string cardNo { get; set; }
        public string cardType { get; set; }
        public string leaderCard { get; set; }
    }

    public class CardInfoSearch
    {
        public string searchID { get; set; }
        public string responseStatusStrg { get; set; }
        public string numOfMatches { get; set; }
        public string totalMatches { get; set; }
        public List<CardInfo> CardInfo { get; set; }
    }

    public class CardInfoSearchRoot
    {
        public CardInfoSearch CardInfoSearch { get; set; }
    }
    public class Valid
    {



        public string enable { get; set; }



        public string beginTime { get; set; }



        public string endTime { get; set; }



        public string timeType { get; set; }
    }
    public class RightPlanItem
    {



        public int doorNo { get; set; }



        public string planTemplateNo { get; set; }
    }
    public class UserInfoItem
    {



        public string employeeNo { get; set; }



        public string name { get; set; }



        public string userType { get; set; }



        public string closeDelayEnabled { get; set; }



        public Valid Valid { get; set; }



        public string belongGroup { get; set; }



        public string password { get; set; }



        public string doorRight { get; set; }



        public List<RightPlanItem> RightPlan { get; set; }



        public int maxOpenDoorTime { get; set; }



        public int openDoorTime { get; set; }



        public string userVerifyMode { get; set; }
    }

    public class UserInfoSearch
    {



        public string searchID { get; set; }



        public string responseStatusStrg { get; set; }



        public int numOfMatches { get; set; }



        public int totalMatches { get; set; }



        public List<UserInfoItem> UserInfo { get; set; }
    }

    public class UserInfoSearchRoot
    {



        public UserInfoSearch UserInfoSearch { get; set; }
    }

}
