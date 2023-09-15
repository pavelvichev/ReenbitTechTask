using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlobFunction.ConfigureInterfaces
{
    public interface IBlobStorageSigner
    {
        Task<string> CreateSASURi(string accountName, string accountKey, string blobContainer,string name);
    }
}
