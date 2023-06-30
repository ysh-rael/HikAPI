using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Text.RegularExpressions;


namespace HikAPI.Public.FileManeger
{
    using HikAPI.Public.Utils;
    public class FileManeger
    {
        protected string path = Utils.Dirname();
        protected string contentLasterFile { get; set; }

        public FileManeger() { contentLasterFile = ""; }
        public FileManeger(string _path) { path += _path; contentLasterFile = ""; }

        public string getContentLasteFile() { return contentLasterFile; }
        public string getPath() { return path; }



        public string read(string filename, bool noLog = false, bool thisPath = true)
        {
            try
            {
                string pathOfFile = thisPath ? path + "/" + filename : filename;
                string file = File.ReadAllText(pathOfFile);
                contentLasterFile = file;
                return file;
            }
            catch (Exception e)
            {
                if (!noLog) System.Console.WriteLine("Exception in read file: " + e);
                return "";
            }
        }
        public bool write(string content, string filename, bool thisPath = true)
        {
            try
            {
                string pathOfFile = thisPath ? path + "/" + filename : filename;

                if (read(pathOfFile, true) != "") File.Delete(pathOfFile);

                System.IO.StreamWriter sw = new System.IO.StreamWriter(pathOfFile);
                sw.WriteLine(content);
                sw.Close();

                return true;
            }
            catch (Exception e)
            {
                System.Console.WriteLine("Exception in write file: " + e);

                return false;
            }
        }
        public bool delete(string filename, bool thisPath = true)
        {
            try
            {
                string pathOfFile = thisPath ? path + "/" + filename : filename;
                File.Delete(pathOfFile);
                return true;
            }
            catch (Exception e)
            {
                System.Console.WriteLine("Exception in delete file: " + e);
                return false;
            }
        }
        public string[] findFile(string allFile, string directory = "", string exp = @"^*config.*.BRA$")
        {
            try
            {
                Regex regex = new Regex(allFile != "" ? @"^*" + allFile + "$" : exp);
                if (allFile != "") System.Console.WriteLine(allFile);

                if (directory == "") directory = path;
                string[] files = Directory.GetFiles(path);

                string quant = ",";
                for (int i = 0; i < files.Length; i++) if (regex.IsMatch(files[i])) quant = ",";

                string[] result = quant.Split(',');
                int indice = 0;
                for (int i = 0; i < files.Length; i++) if (regex.IsMatch(files[i]))
                    {
                        if (indice < result.Length)
                            result[indice] = files[i].Replace('\\', '/');
                        indice++;
                    }
                return result;
            }
            catch (Exception e)
            {
                System.Console.WriteLine("Exception in findFile file: " + e);
                string[] res = { "" };
                return res;
            }
        }

    }
    public class BRA : FileManeger
    {
        public string content { get; }
        public string file = "";
        public string attr = "";
        public string errCase = "";
        public string type = "";
        public string idTerminal = "";
        public string time = "";
        public string impressora = "";
        public string mode = "homologacao";
        public string lote = "1";
        public string identificador = "";
        public string protocolo = "";
        public string id = "";
        public string justificativa = "Cancelamento de NFCe por motivo genérico.";

        public bool pathDefault = true;

        public BRA(string Content)
        {
            content = Content;
            string[] line = content.Split('\n');
            foreach (string esse in line)
            {
                string[] keyValue = esse.Split(':');
                if (esse != "\n" || esse != "")
                {
                    switch (keyValue[0])
                    {


                        case "attr": attr = keyValue[1]; break;
                        case "file": file = keyValue[1].Replace(" ", ""); break;
                        case "pathDefault": pathDefault = keyValue[1].Contains("true"); break;
                        case "errCase": errCase = keyValue[1]; break;
                        case "type": type = keyValue[1].Replace(" ", ""); break;
                        case "idTerminal": idTerminal = keyValue[1].Replace(" ", ""); break;
                        case "time": time = keyValue[1].Replace(" ", ""); break;
                        case "impressora": impressora = keyValue[1]; break;
                        case "mode": mode = keyValue[1].Replace(" ", ""); break;
                        case "lote": lote = keyValue[1].Replace(" ", ""); break;
                        case "identificador": identificador = keyValue[1].Replace(" ", ""); break;
                        case "protocolo": protocolo = keyValue[1].Replace(" ", ""); break;
                        case "id": id = keyValue[1].Replace(" ", ""); break;
                        case "justificativa": justificativa = keyValue[1]; break;

                        default:
                            System.Console.WriteLine("Não foi possível reconhecer o seguinte atributo: " + keyValue[0]);
                            break;
                    }
                }
            }


        }
    }
}
