using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HikAPI.Public.Utils
{
    internal static class Utils
    {
        public class User
        {
            public string EnployeeNo { get; set; }
            public string Name { get; set; }
            public string UserType { get; set; }
            public string PlanTamplete { get; set; }
            public FaceId FaceId { get; set; }
            public Card Card { get; set; }
        }

        public class FaceId
        {
            public string Id { get; set; }
            public string PathFile { get; set; }
        }

        public class Card
        {
            public string Number { get; set; }
            public string Type { get; set; }
        }

        public class RootObject
        {
            public string typy { get; set; }
            public User User { get; set; }
        }

        public static List<RootObject> Request(string content)
        {
            return JsonConvert.DeserializeObject<List<RootObject>>(content);
        }

        public static string Dirname() { return Directory.GetCurrentDirectory(); }
        public static string Response(int code, string msg, string nameFile = "", string protocolo = "", string idNF = "")
        {
            return $" {'{'} \"xStat\": {code}, \"xMotivo\": \"{msg}\", \"nameFile\": \"{nameFile}\", \"protocolo\": \"{protocolo}\", \"idNF\": \"{idNF}\"  {'}'} ";
        }
    }
}
