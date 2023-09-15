using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlobFunction.Configure
{
    public class AppSettings
    {
        public string ConnectionStringEmail { get; }
        public string StorageName { get; } 
        public string AccountKey { get;}
        public string ContainerName { get;}

        public AppSettings(string ConnectionStringEmail,string StorageName, string AccountKey, string ContainerName)
        {
            this.ConnectionStringEmail = ConnectionStringEmail;
            this.StorageName = StorageName;
            this.AccountKey = AccountKey;
            this.ContainerName = ContainerName;
        }
    }
}
