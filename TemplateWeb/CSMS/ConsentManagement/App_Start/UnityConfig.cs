using System.Web.Configuration;
using System.Web.Mvc;
using ConsentManagement.Services;
using Unity;
using Unity.Injection;
using Unity.Mvc5;

namespace ConsentManament
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
            var container = new UnityContainer();
            string pConstr = WebConfigurationManager.ConnectionStrings["DBSession"].ToString();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();
            


            //container.RegisterType<IViewShareholderProfileServices, ViewShareholderProfileServices>(new InjectionConstructor(new Services.DBUtility(pConstr)));
            container.RegisterType<IMenuPermissionServices, MenuPermissionServices>(new InjectionConstructor(new ConsentManagement.Services.DBUtility(pConstr)));
            //container.RegisterType<IAuthenticationService, AuthenticationService>(new InjectionConstructor(new ConsentManagement.Services.DBUtility(pConstr)));
            container.RegisterType<IUsersServices, UsersServices>(new InjectionConstructor(new ConsentManagement.Services.DBUtility(pConstr)));
            container.RegisterType<IActivityServices, ActivityServices>(new InjectionConstructor(new ConsentManagement.Services.DBUtility(pConstr)));
            //container.RegisterType<IConsentManageServices, ConsentManageServices>(new InjectionConstructor(new ConsentManagement.Services.DBUtility(pConstr)));
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}