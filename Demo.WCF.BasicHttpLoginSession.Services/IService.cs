using System.ServiceModel;

namespace Demo.WCF.BasicHttpLoginSession.Services
{
    [ServiceContract(Namespace = "http://demo.wcfloginsession.net")]
    interface IService
    {
        [OperationContract]
        string Login(string userID, string password);

        [OperationContract]
        void Logout();

        [OperationContract]
        string ActionA();

        [OperationContract]
        string ActionB();
    }
}
