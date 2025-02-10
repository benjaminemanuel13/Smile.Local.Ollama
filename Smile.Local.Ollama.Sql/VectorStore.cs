
using System.Text;
using System.IO;
using System.Net;
using System.Resources;
using System.Security.Permissions;

namespace Smile.Local.Ollama.Sql
{
    public class Smile
    {
        [HostProtection(Resources = HostProtectionResource.UI)]
        [Microsoft.SqlServer.Server.SqlProcedure]
        public static int DoRest(string url, string method, string credential, string payload, out string response)
        {
            // Data to be sent in the POST request
            string postData = payload;

            // Create a request using a URL that can receive a post
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

            request.Headers.Add("api-key", credential);
            // Set the Method property of the request to POST
            request.Method = "POST";

            // Convert the data to a byte array
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);

            // Set the ContentType property of the WebRequest
            request.ContentType = "application/json";

            // Set the ContentLength property of the WebRequest
            request.ContentLength = byteArray.Length;

            // Get the request stream
            using (Stream dataStream = request.GetRequestStream())
            {
                // Write the data to the request stream
                dataStream.Write(byteArray, 0, byteArray.Length);
            }

            // Get the response
            using (WebResponse resp = request.GetResponse())
            {
                // Get the stream containing content returned by the server
                using (Stream responseStream = resp.GetResponseStream())
                {
                    // Open the stream using a StreamReader for easy access
                    using (StreamReader reader = new StreamReader(responseStream))
                    {
                        response = reader.ReadToEnd();   
                    }
                }
            }

            return 0;
        }
    }
}
