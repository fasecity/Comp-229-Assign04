using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;

namespace Comp229__Assign04
{
    public class Global : HttpApplication
    {
        public static IEnumerable<Model.Model> Models;
        public const string ModelsJsonFilePath = "~/Data/Assign04.json";
        private const string ModelsNewJsonFilePath = "~/Data/NewAssign04.json";
        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            prepareModelCollection();
        }
        private void prepareModelCollection()
        {
            using (System.IO.StreamReader streamReader = new StreamReader(System.Web.Hosting.HostingEnvironment.MapPath(ModelsJsonFilePath)))
            {
                var jsonString = streamReader.ReadToEnd();
                Models = JsonConvert.DeserializeObject<List<Model.Model>>(jsonString);

            }
        }
        public static void UpdateNewJsonFile()
        {
            using (StreamWriter StreamWriter = File.CreateText(System.Web.Hosting.HostingEnvironment.MapPath(ModelsNewJsonFilePath)))
            {
                StreamWriter.WriteLine(JsonConvert.SerializeObject(Models));
            }
        }
    }
}