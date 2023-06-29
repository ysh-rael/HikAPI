using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;

namespace HikAPI.DeviceInfo
{
    public partial class DeviceInfo
    {
        public string strDeviceNickName;
        public string strDeviceIP;
        public string strHttpPort;
        public string strUsername;
        public string strPassword;
        public int nChannelNum = 0;
        public List<ChannelsInfo> struChannelsInfoList = new List<ChannelsInfo>();
        public bool bIsLogin = false;
        public bool bIsAlarmStart = false;
        public int nSupportMetadata = -1; //-1:init value;0:not support;1:support
        public int nRtspPort = 0;
        public string strDeviceType;
        public int timeDifference;
    }

    public partial class ChannelsInfo
    {
        public int nChannelType;
        public int nID;
        public string strName;
        public string strIPAddress;
        public string strManagePortNo;
        public bool bIPChannelOnline;// ip channel
        public bool bZeroChannelEnable;// zero channel
        public int nProxyProtocol;
        public bool bMetadata;
    }

    public enum ChannelType
    {
        ZERO = 0,
        SIMULATION,
        IP
    }

    public enum DeviceType
    {
        IPCamera = 0,
        IPDome,
        DVR,
        HybirdNVR,
        NVR,
        DVS,
        IPZoom,
        CVR
    }

    public enum ProxyProtocolType
    {
        PRIVATE = 0,
        RTSP
    }

    public class TreeNodeTag
    {
        public ChannelsInfo struChannelInfo;
        public DeviceInfo struDeviceInfo;
        //public MediaDisplay mediaDisplay;
        //public MediaDisplay mediaPlayback;
    }

    public class Position
    {
        public int x;
        public int y;
        public int witch;
        public int heigth;
    }

}
