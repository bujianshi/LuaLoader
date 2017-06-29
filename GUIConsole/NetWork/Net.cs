using System;
using System.IO;
using System.Net;
using System.Text;
using LuaHotLoader.LogSys;
using Newtonsoft.Json.Linq;
using SuperSocket.ClientEngine;
using ErrorEventArgs = SuperSocket.ClientEngine.ErrorEventArgs;

namespace LuaHotLoader.NetWork
{
    public delegate void DelOnPackage(JObject jsonObj);
    public delegate void DelOnConnected();
    public delegate void DelOnError();
    public delegate void DelOnClosed();

    class Net
    {
        private AsyncTcpSession session = null;
        private string ip;
        private int port;
        private MemoryStream ms;
        private const ushort MagicNum = 0xfefe;
        private const int HeadLen = 6;

        public event DelOnPackage EventOnPackage;
        public event DelOnError EventOnError;
        public event DelOnConnected EventOnConnected;
        public event DelOnClosed EventOnClosed;

        public Net(string ip, int port)
        {
            this.ip = ip;
            this.port = port;
            ms = new MemoryStream(1024*1024*64);
        }

        public void Connect()
        {
            if (session != null) session.Close();
            session = new AsyncTcpSession();
            session.Connect(new IPEndPoint(IPAddress.Parse(ip), port));
            session.Connected += OnConnected;
            session.DataReceived += OnRecevied;
            session.Error += OnError;
            session.Closed += OnClosed;
        }

        public void DisConnect()
        {
            session.Close();
            session = null;
        }

        private void OnClosed(object sender, EventArgs e)
        {
            if (EventOnClosed != null)
            {
                EventOnClosed.Invoke();
            }
        }

        private void OnError(object sender, ErrorEventArgs e)
        {
            Log.LogE("connect error " + e.Exception);
            session.Close();
            if (EventOnError != null)
            {
                EventOnError.Invoke();
            }
        }

        private void OnConnected(object sender, EventArgs e)
        {
            if (EventOnConnected != null)
            {
                EventOnConnected.Invoke();
            }
        }

        private void OnRecevied(object sender, DataEventArgs e)
        {
            try
            {
                if (e.Length == 0)
                {
                    return;
                }

                if (ms.Position < HeadLen)
                {
                    int headNeed = HeadLen - (int)ms.Position;
                    if (e.Length < headNeed)
                    {
                        ms.Write(e.Data, e.Offset, e.Length);
                        return;
                    }

                    ms.Write(e.Data, e.Offset, headNeed);
                    e.Offset += headNeed;
                    e.Length -= headNeed;

                    var magic = BitConverter.ToUInt16(ms.GetBuffer(), 0);
                    if (magic != MagicNum)
                    {
                        ms.Seek(0, SeekOrigin.Begin);
                        return;
                    }

                    OnRecevied(sender, e);
                }
                else
                {
                    var bodyLen = BitConverter.ToInt32(ms.GetBuffer(), 2);
                    var bodyNeed = HeadLen + bodyLen - (int)ms.Position;
                    if (e.Length >= bodyNeed)
                    {
                        ms.Write(e.Data, e.Offset, bodyNeed);
                        RoutePackage();
                        ms.Seek(0, SeekOrigin.Begin);
                        e.Offset += bodyNeed;
                        e.Length -= bodyNeed;
                        OnRecevied(sender, e);
                    }
                    else
                    {
                        ms.Write(e.Data, e.Offset, e.Length);
                    }
                }
            }
            catch (Exception exp)
            {
                Log.LogE("recv data exp " + exp);
            }
        }

        private void RoutePackage()
        {
            try
            {
                var jsonStr = Encoding.UTF8.GetString(ms.GetBuffer(), HeadLen, (int) ms.Position - HeadLen);
                var jsonObj = JObject.Parse(jsonStr);
                if (jsonObj != null)
                {
                    if (EventOnPackage != null)
                    {
                        EventOnPackage.Invoke(jsonObj);
                    }
                }
            }
            catch (Exception e)
            {
                Log.LogE("route pkg exp " + e);
            }
        }

        public void Send(JObject jsonObj)
        {
            var jsonStr = jsonObj.ToString();
            var jsonBytes = Encoding.UTF8.GetBytes(jsonStr);
            using (var ms = new MemoryStream())
            {
                ms.Write(BitConverter.GetBytes(MagicNum), 0, 2);
                ms.Write(BitConverter.GetBytes(jsonBytes.Length), 0, 4);
                ms.Write(jsonBytes, 0, jsonBytes.Length);
                session.Send(ms.GetBuffer(), 0, HeadLen+jsonBytes.Length);
            }
        }

    }
}
