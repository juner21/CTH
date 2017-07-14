using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.Composition;
using System.Configuration;
using System.ComponentModel.Composition.Hosting;

namespace InterfaceContract
{
    
    /// <summary>
    /// IOC容器
    /// </summary>
    public static class ContainerDocker
    {

        public static CompositionContainer Container { get;private set; }
        static ContainerDocker() {
            var iocPath = ConfigurationManager.AppSettings["IocPath"].ToString();
            AggregateCatalog catalog = new AggregateCatalog();
            catalog.Catalogs.Add(new DirectoryCatalog(iocPath));
            Container = new CompositionContainer(catalog);
        }

    }


    public class Contract
    {
        [Import]
        public IUserData UserData { get; set; }
    }
}
