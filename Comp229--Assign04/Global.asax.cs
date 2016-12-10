using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
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
        public const string ModelsNewJsonFilePath = "~/Data/NewAssign04.json";
        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            prepareModelCollection();
        }
        /// <summary>
        /// This method reads the given json file and deserializes it using Json.net newtonsoft dll
        /// </summary>
        private void prepareModelCollection()
        {
            using (System.IO.StreamReader streamReader = new StreamReader(System.Web.Hosting.HostingEnvironment.MapPath(ModelsJsonFilePath)))
            {
                var jsonString = streamReader.ReadToEnd();
                Models = JsonConvert.DeserializeObject<List<Model.Model>>(jsonString);

            }
        }
        /// <summary>
        /// This method serializes and saves the new json data to a new file
        /// </summary>
        public static void UpdateNewJsonFile()
        {
            using (StreamWriter StreamWriter = File.CreateText(System.Web.Hosting.HostingEnvironment.MapPath(ModelsNewJsonFilePath)))
            {
                StreamWriter.WriteLine(JsonConvert.SerializeObject(Models));
            }
        }

        //Send message method
        public static void EmailFile(string clientEmailAddress, string clientName, string attachmentFileName)
        {
            SmtpClient smtpClient = new SmtpClient("smtp-mail.outlook.com", 587);
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;

            MailMessage message = new MailMessage();
            try
            {
                MailAddress fromAddress = new MailAddress("cc-comp229f2016@outlook.com", "Comp229-Assign04");
                MailAddress toAddress = new MailAddress(clientEmailAddress, clientName);
                message.From = fromAddress;
                message.To.Add(toAddress);
                message.Subject = "Comp229-Assign04 email";
                message.Body = "Hope this works";

                smtpClient.Host = "smtp-mail.outlook.com";
                smtpClient.EnableSsl = true;

                // SET UseDefaultCredentials to false BEFORE setting Credentials - we all have 'ugh' moments
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new System.Net.NetworkCredential("cc-comp229f2016@outlook.com", "comp229pwd");

                System.Net.Mime.ContentType contentType = new System.Net.Mime.ContentType();
                contentType.MediaType = System.Net.Mime.MediaTypeNames.Application.Octet;
                contentType.Name = attachmentFileName;
                message.Attachments.Add(new Attachment(System.Web.Hosting.HostingEnvironment.MapPath(ModelsNewJsonFilePath), contentType));

                smtpClient.Send(message);
              
            }
            catch (Exception e)
            {


            }

        }   }
}