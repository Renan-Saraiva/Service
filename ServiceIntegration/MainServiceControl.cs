using System;
using System.ServiceProcess;

namespace ServiceIntegration
{
    class MainServiceControl
    {
        static void Main(string[] args)
        {           
            Service serviceToRun;

            #region DEBUG
            //if (Environment.UserInteractive && true)
            //{
            //    serviceToRun = new Service("ServiceIntegration");
            //    serviceToRun.RunAsConsole(args);
            //}
            //else
            //{
            //    ServiceController[] scServices = ServiceController.GetServices();
            //    string srvname = "ServiceIntegration";
            //    foreach (ServiceController scTemp in scServices)
            //    {
            //        if (scTemp.ServiceName.Contains("ServiceIntegration", StringComparison.InvariantCultureIgnoreCase))
            //            srvname = scTemp.ServiceName;
            //    }

            //    serviceToRun = new Service(srvname);
            //    ServiceBase.Run(serviceToRun);
            //}
            #endregion

            #region DEPLOY
            ServiceController[] scServices = ServiceController.GetServices();
            string srvname = "ServiceIntegration";
            foreach (ServiceController scTemp in scServices)
            {
                if (scTemp.ServiceName.Contains("ServiceIntegration", StringComparison.InvariantCultureIgnoreCase))
                    srvname = scTemp.ServiceName;
            }

            serviceToRun = new Service(srvname);
            ServiceBase.Run(serviceToRun);
            #endregion
        }
    }
}
