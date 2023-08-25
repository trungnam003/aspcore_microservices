using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#nullable disable
namespace Shared.Configurations
{
    public class DatabaseSettings
    {
        public string DBProvider { get; set; }
        public string ConnectionString { get; set; }
    }
}
