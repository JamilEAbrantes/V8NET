using System;
using System.Data;
using Oracle.ManagedDataAccess.Client;
using V8Net.Shared;

namespace V8Net.Infra.Data.DataContext
{
    public class V8NetDataContext : IDisposable
    {
        public OracleConnection Connection { get; set; }

        public V8NetDataContext()
        {
            Connection = new OracleConnection(Settings.ConnectionString);
            Connection.Open();
        }

        public void Dispose()
        {
            if (Connection.State != ConnectionState.Closed)
                Connection.Close();
        }
    }
}
