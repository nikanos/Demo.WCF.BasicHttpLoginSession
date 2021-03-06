﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Demo.WCF.BasicHttpLoginSession.ServicesTester.LoginSessionService {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://demo.wcfloginsession.net", ConfigurationName="LoginSessionService.IService")]
    public interface IService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://demo.wcfloginsession.net/IService/Login", ReplyAction="http://demo.wcfloginsession.net/IService/LoginResponse")]
        string Login(string userID, string password);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://demo.wcfloginsession.net/IService/Login", ReplyAction="http://demo.wcfloginsession.net/IService/LoginResponse")]
        System.Threading.Tasks.Task<string> LoginAsync(string userID, string password);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://demo.wcfloginsession.net/IService/Logout", ReplyAction="http://demo.wcfloginsession.net/IService/LogoutResponse")]
        void Logout();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://demo.wcfloginsession.net/IService/Logout", ReplyAction="http://demo.wcfloginsession.net/IService/LogoutResponse")]
        System.Threading.Tasks.Task LogoutAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://demo.wcfloginsession.net/IService/ActionA", ReplyAction="http://demo.wcfloginsession.net/IService/ActionAResponse")]
        string ActionA();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://demo.wcfloginsession.net/IService/ActionA", ReplyAction="http://demo.wcfloginsession.net/IService/ActionAResponse")]
        System.Threading.Tasks.Task<string> ActionAAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://demo.wcfloginsession.net/IService/ActionB", ReplyAction="http://demo.wcfloginsession.net/IService/ActionBResponse")]
        string ActionB();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://demo.wcfloginsession.net/IService/ActionB", ReplyAction="http://demo.wcfloginsession.net/IService/ActionBResponse")]
        System.Threading.Tasks.Task<string> ActionBAsync();
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IServiceChannel : Demo.WCF.BasicHttpLoginSession.ServicesTester.LoginSessionService.IService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class ServiceClient : System.ServiceModel.ClientBase<Demo.WCF.BasicHttpLoginSession.ServicesTester.LoginSessionService.IService>, Demo.WCF.BasicHttpLoginSession.ServicesTester.LoginSessionService.IService {
        
        public ServiceClient() {
        }
        
        public ServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public ServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public string Login(string userID, string password) {
            return base.Channel.Login(userID, password);
        }
        
        public System.Threading.Tasks.Task<string> LoginAsync(string userID, string password) {
            return base.Channel.LoginAsync(userID, password);
        }
        
        public void Logout() {
            base.Channel.Logout();
        }
        
        public System.Threading.Tasks.Task LogoutAsync() {
            return base.Channel.LogoutAsync();
        }
        
        public string ActionA() {
            return base.Channel.ActionA();
        }
        
        public System.Threading.Tasks.Task<string> ActionAAsync() {
            return base.Channel.ActionAAsync();
        }
        
        public string ActionB() {
            return base.Channel.ActionB();
        }
        
        public System.Threading.Tasks.Task<string> ActionBAsync() {
            return base.Channel.ActionBAsync();
        }
    }
}
