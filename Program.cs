using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HikAPI
{
    using HikAPI.Event;
    using HikAPI.Public.FileManeger;
    using System.Xml;

    internal class Program
    {
        public static bool Continuar = true;

        private static readonly FileManeger PathResponse = new FileManeger("/res");

        static async Task Main(string[] args)
        {
            // Argumento passado na chamada do executável. Vazio pede para buscar todos os config na pasta req.
            try
            {
                string AllRequestFilePathOf = args[0];
                /*foreach (string PathFileRequest in PathRequest.findFile(AllRequestFilePathOf)) // Para cada arquivo config.*.BRA encontrado.
                {
                    if (string.IsNullOrEmpty(PathFileRequest)) continue;
                    BRA RequestFile = new BRA(PathRequest.read(PathFileRequest, false, false));
                    string type = $"{RequestFile.type[0]}{RequestFile.type[1]}{RequestFile.type[2]}";
                    switch (type)
                    {
                        case "100":
                            break;
                        case "200":
                            break;
                        case "300":
                            break;
                        case "400":
                            break;
                        case "500":
                            break;
                        case "600":
                            break;
                        default:
                            break;
                    }
                }*/
            }
            catch { };



            /*
            Device Dispositivo = new Device();

            Dispositivo.struDeviceInfo.strUsername = "admin";
            Dispositivo.struDeviceInfo.strPassword = "root@1234";
            Dispositivo.struDeviceInfo.strDeviceIP = "10.0.0.5";
            Dispositivo.struDeviceInfo.strHttpPort = "80";

            Dispositivo.Login();

            // Chamo classe desejada, e passo o equipamento para o método que irá disparar o evento.

            ControlDoor.Open(Dispositivo);
            ControlDoor.Close(Dispositivo);
            ControlDoor.StayOpen(Dispositivo);
            ControlDoor.StayClose(Dispositivo);
            */

            /*
            CardManagement.Get(Dispositivo, "1234");
            CardManagement.Delete(Dispositivo, "1234");
            CardManagement.Set(Dispositivo, "3002", "1234", "normalCard")
            */

            /*
            FaceManagement.Get(Dispositivo, "1", "blackFD", 1);
            FaceManagement.Delete(Dispositivo, "1", "blackFD", 1);
            string path = "C:\\Users\\yshaeldev\\Desktop\\yshrael\\hikvision\\Hik-ControlDoor\\bin\\Debug\\output\\";
            FaceManagement.Set(Dispositivo, "333", "blackFD", 1, $"{path}face.jpg")
            */

            /*
            Console.WriteLine(UserManagement.Get(Dispositivo, "1"));
            Console.WriteLine(UserManagement.Delete(Dispositivo, "1"));
            UserManagement.Set(Dispositivo, "333", "Meio besta", "normal", "1")
            */

            /* bool Usuario = UserManagement.Set(Dispositivo, "333", "Meio besta", "normal", "1");
             if (Usuario)
             {
                 string path = "C:\\Users\\yshaeldev\\Desktop\\yshrael\\hikvision\\HikAPI\\bin\\Debug\\output\\";
                 bool FaceId = FaceManagement.Set(Dispositivo, "333", "blackFD", 1, $"{path}face.jpg");
                 if (FaceId)
                 {
                     bool Cartao = CardManagement.Set(Dispositivo, "333", "2023", "normalCard");
                     if (Cartao)
                     {
                         Console.WriteLine("Helo, word! Tudo certo, até agora.");
                         await Task.Delay(TimeSpan.FromSeconds(8));
                         Continuar = false;
                     }
                 }
             }*/

            while (Continuar) { }
        }
    }
}
