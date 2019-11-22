using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Threading;
using ServiceIntegration.Global;

namespace ServiceIntegration
{
    class Service : ServiceBase
    {
        ServiceSettings Setting;
        NLog.Logger Logger;
        Dictionary<string, Thread> ServiceInstances;
        private bool ServiceIsWork;

        public Service(string servicename)
        {
            this.ServiceName = servicename;
        }

        #region OVERRIDES

        protected override void OnStart(string[] args)
        {
            /* Inicializa o JsonConfig */
            Setting = ServiceSettings.Create();

            /* Inicializa o Logger */
            Logger = Setting.GetLogger();

            /* Inicializa o Controle de theads */
            ServiceInstances = new Dictionary<string, Thread>();

            Logger.Info("OnStart ServiceIntegration");

            try
            {
                ServiceIsWork = true;
                foreach (var key in Setting.Bindings.Keys.ToArray())
                {
                    BaseBinding item;
                    Setting.Bindings.TryGetValue(key, out item);
                    
                    lock (ServiceInstances)
                    {
                        Thread work = new Thread(new ParameterizedThreadStart(ServiceThread));
                        work.Start(key);
                        ServiceInstances.Add(key, work);
                    }
                    Thread.Sleep(250);
                }
            }
            catch (Exception ex)
            {
                Logger.Fatal(ex, $"Erro na inicialização da binding");
                ServiceIsWork = false;
            }

            #region DEBUG
            //if (!Environment.UserInteractive)
            //    base.OnStart(args);
            #endregion

            base.OnStart(args);
        }

        protected override void OnStop()
        {
            Logger.Info("OnStop ServiceIntegration");

            ServiceIsWork = false;

            bool IsFinalize = true;

            #region Service Instances Finalize

            while (IsFinalize)
            {
                IsFinalize = false;

                var keys = ServiceInstances.Keys.ToArray();
                foreach (string k1 in keys)
                {
                    if (ServiceInstances[k1].ThreadState == System.Threading.ThreadState.Running)
                        IsFinalize = true;
                }

                if (IsFinalize)
                    Thread.Sleep(1000);
            }

            #endregion

            ServiceInstances.Clear();
            ServiceInstances = null;
            Setting = null;
            Logger = null;

            NLog.LogManager.Shutdown();

            base.OnStop();
        }

        protected override void OnShutdown()
        {   
            if (Logger != null)
                Logger.Warn("OnShutdown Service");

            NLog.LogManager.Shutdown();

            #region DEBUG
            //if (!Environment.UserInteractive)
            // base.OnShutdown();
            #endregion

            base.OnShutdown();
        }

        public void RunAsConsole(string[] args)
        {
            OnStart(args);
            Console.WriteLine("Press any key to exit...");
            Console.ReadLine();
            OnStop();
        }

        #endregion
     
        void ServiceThread(object k)
        {
            while (ServiceIsWork) 
            {

            }
        }
    }
}
