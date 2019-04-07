using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Demo.WCF.BasicHttpLoginSession.ServicesTester
{
    class SimpleTestClient
    {
        const string AUTHNS = "http://demo.wcfloginsession.net/auth/";

        static void Main(string[] args)
        {
            LoginSessionService.ServiceClient cli = new LoginSessionService.ServiceClient();
            try
            {
                string userID = "test";
                string password = "test";
                string token = cli.Login(userID, password);
                using (var opScope = new OperationContextScope(cli.InnerChannel))
                {
                    var userIDHeader = new MessageHeader<string>(userID);
                    var tokenHeader = new MessageHeader<string>(token);
                    var untypedUserIDHeader = userIDHeader.GetUntypedHeader("UserID", AUTHNS);
                    var untypedTokenHeader = tokenHeader.GetUntypedHeader("Token", AUTHNS);
                    OperationContext.Current.OutgoingMessageHeaders.Add(untypedUserIDHeader);
                    OperationContext.Current.OutgoingMessageHeaders.Add(untypedTokenHeader);
                    Console.WriteLine(cli.ActionA());
                    Console.WriteLine(cli.ActionB());
                    cli.Logout();
                    cli.Close();
                }
            }
            catch (FaultException ex)
            {
                Console.WriteLine(ex.Message);
                cli.Abort();
            }
            catch (CommunicationException ex)
            {
                Console.WriteLine(ex.Message);
                cli.Abort();
            }
            catch (TimeoutException ex)
            {
                Console.WriteLine(ex.Message);
                cli.Abort();
            }
        }
    }
}
