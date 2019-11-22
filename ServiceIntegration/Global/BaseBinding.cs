using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceIntegration.Global
{
    public class BaseBinding
    {
        public string Name { get; }
        public string ConnectionStringName { get; }
        public string ConnectionString { get; }
        public int SleepInterval { get; }

        public BaseBinding(string name, string connectionstringname, string connectionstring, int sleepInterval) {
            this.Name = name;
            this.ConnectionStringName = connectionstringname;
            this.ConnectionString = connectionstring;
            this.SleepInterval = sleepInterval;
        }
    }
}
