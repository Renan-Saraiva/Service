using System;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using NLog;
using NLog.Extensions.Logging;

namespace ServiceIntegration.Global
{
    internal class ServiceSettings
    {
        private IConfigurationRoot RootConfig;
        private IConfigurationBuilder ConfingBuilder;
        public Dictionary<string, BaseBinding> Bindings { get; set; }
        public Dictionary<string, string> ConnectionStrings { get; set; }

        public static ServiceSettings Create() {
            return new ServiceSettings(new ConfigurationBuilder().SetBasePath(AppContext.BaseDirectory).AddJsonFile("ServiceSettings.json"));
        }

        private ServiceSettings(IConfigurationBuilder Builder) {
            ConfingBuilder = Builder;
            RootConfig = ConfingBuilder.Build();            

            var bindings = RootConfig.GetSection("Bindings");
            var connections = RootConfig.GetSection("ConnectionStrings");
            LogManager.Configuration = new NLogLoggingConfiguration(RootConfig.GetSection("NLog"));

            Bindings = new Dictionary<string, BaseBinding>();
            ConnectionStrings = new Dictionary<string, string>();

            /* Processa a ConnectionString */
            var connectionsList = connections.AsEnumerable(true);
            foreach (KeyValuePair<string, string> item in connectionsList)
            {
                int index = 0;
                if (int.TryParse(item.Key, out index))
                    ConnectionStrings.Add(connections[$"{index}:name"], connections[$"{index}:connection"]);
            }

            /* Processa a Binding */
            var bindList = bindings.AsEnumerable(true);
            foreach (KeyValuePair<string, string> item in bindList)
            {
                int index = 0;
                if (int.TryParse(item.Key, out index)) {
                    var bindindChild = new BaseBinding(
                        bindings[$"{index}:name"], 
                        bindings[$"{index}:connectrionStringName"],
                        ConnectionStrings[bindings[$"{index}:connectrionStringName"]],
                        int.Parse(bindings[$"{index}:sleepInterval"]));

                    Bindings.Add(bindindChild.Name, bindindChild);
                }
            }
        }

        public NLog.Logger GetLogger()
        {
            return LogManager.GetLogger("ServiceIntegration");
        }
    }
}
