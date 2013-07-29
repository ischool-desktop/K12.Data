using System;
using FISCA.Data;
using FISCA.DSAClient;
using FISCA.DSAUtil;

namespace K12.Data.Utility
{
    /// <summary>
    /// K12 DAL所使用的DSA連線，預設為呼叫FISCA的DSA連線，可特別指定採用獨立的DSA連線
    /// </summary>
    public class DSAServices
    {
        private static DSAServices mDSAServices = null;
        private delegate DSResponse InternalCallService(string service, DSRequest req);
        private InternalCallService InstanceCallService = null;
        private string mAccessPoint = "test.kh.edu.tw";
        private string mFullUserName = "admin";
        private string mPassword = "1234";

        //private string mAccessPoint = "ischool.sh@test";
        //private string mFullUserName = "admin";
        //private string mPassword = "1234";

        /// <summary>
        /// 初始化採用FISCA的DSA連線，必須要登入FISCA Application才能用DAL
        /// </summary>
        private DSAServices()
        {
            InstanceCallService = new InternalCallService(FISCA.Authentication.DSAServices.CallService);
            //QueryHelper.SetDefaultDataSource(FISCA.Authentication.DSAServices.DefaultDataSource);
        }

        /// <summary>
        /// 採用Singleton模式，所有的DSA都用共用同樣的連線
        /// </summary>
        private static DSAServices Instance
        {
            get
            {
                if (mDSAServices == null)
                    mDSAServices = new DSAServices();
                return mDSAServices;
            }
        }

        /// <summary>
        /// 直接用DSAUtil來呼叫Service
        /// </summary>
        /// <param name="service">服務名稱</param>
        /// <param name="req">申請文件</param>
        /// <returns></returns>
        private DSResponse CallTestingService(string service, DSRequest req)
        {
            FISCA.DSAClient.Connection vConnection = new FISCA.DSAClient.Connection();

            vConnection.Connect(mAccessPoint,"" , mFullUserName , mPassword);

            if (vConnection.IsConnected)
            {
                try
                {
                    Envelope vRequest = new Envelope();

                    vRequest.Body = new XmlStringHolder(req.GetContent().GetRawXml());
                    Envelope vResponse = vConnection.SendRequest(service,vRequest);

                    DSResponse rsp = new DSResponse();

                    rsp.SetContent(vResponse.Body.XmlString);

                    return rsp;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    vConnection = null;
                }
            }
            else
                throw new Exception("無法與DSA連線");

            return null;
        }

        private void SetInternalMode()
        {
            InstanceCallService = new InternalCallService(CallTestingService);
        }

        private void SetExternalMode()
        {
            InstanceCallService = new InternalCallService(FISCA.Authentication.DSAServices.CallService);
        }

        public static void SetFISCAMode()
        {
            Instance.SetExternalMode();
            QueryHelper.SetDefaultDataSource(FISCA.Authentication.DSAServices.DefaultDataSource);
        }

        public static void SetTestingMode()
        {
            Instance.SetInternalMode();

            FISCA.DSAClient.Connection Conn = new Connection();

            Conn.Connect(Instance.mAccessPoint,"admin",Instance.mFullUserName,Instance.mPassword);

            QueryHelper.SetDefaultDataSource(Conn);
        }

        public static void SetTestingMode(string AccessPoint,string FullUserName,string Password)
        {
            Instance.mAccessPoint = AccessPoint;
            Instance.mFullUserName = FullUserName;
            Instance.mPassword = Password;
            Instance.SetInternalMode();

            FISCA.DSAClient.Connection Conn = new Connection();

            Conn.Connect(Instance.mAccessPoint, "admin", Instance.mFullUserName, Instance.mPassword);

            QueryHelper.SetDefaultDataSource(Conn);
        }

        public static DSResponse CallService(string service, DSRequest req)
        {
            return Instance.InstanceCallService(service, req);
        }
    }
}