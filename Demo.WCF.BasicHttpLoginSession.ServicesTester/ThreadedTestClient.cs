using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net;
using System.Diagnostics;

namespace Demo.WCF.BasicHttpLoginSession.ServicesTester
{
    class ThreadedTestClient
    {
        const string AUTHNS = "http://demo.wcfloginsession.net/auth/";
        static void Main(string[] args)
        {
            int numThreads;
            Console.Write("Enter number of threads:");
            numThreads = Convert.ToInt32(Console.ReadLine());
            Stopwatch sw = new Stopwatch();
            sw.Start();

            var threads = new List<Thread>();
            for (int i = 0; i < numThreads; i++)
            {
                string currentUserId = "user" + i.ToString();
                Thread t = new Thread(x =>
                  {
                      LoginSessionService.ServiceClient cli = new LoginSessionService.ServiceClient();
                      string userID = currentUserId;
                      string password = currentUserId;
                      string token = cli.Login(userID, password);
                      Console.WriteLine($"got {token} for {userID}");
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
                      }
                  });
                threads.Add(t);
            }

            foreach (var thread in threads)
                thread.Start();

            foreach (var thread in threads)
                thread.Join();
            sw.Stop();
            Console.WriteLine("Time:" + sw.Elapsed);
        }
    }
}
