using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HikAPI.Event
{
using HikAPI.DeviceInfo;
using HikAPI.Security;
    internal class Device // Novo dispositivo
    {

        public DeviceInfo struDeviceInfo = new DeviceInfo();
        public void Login()
        {
            // Chama função que de fato efetuará o login
            if (Security.Login(struDeviceInfo))
            {
                // user check success
                struDeviceInfo.bIsLogin = true;
                // continua ....
            } else
            {
                // Retorno falha de login
            }
        }
    }
}
