using CefSharp;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GetNewsTools.Business
{
   

    class SchemaHandler : IResourceHandler
    {
        private string mimeType;
        private MemoryStream stream;

        void IResourceHandler.Cancel()
        {
            throw new NotImplementedException();
        }

        bool IResourceHandler.CanGetCookie(Cookie cookie)
        {
            return true;
        }

        bool IResourceHandler.CanSetCookie(Cookie cookie)
        {
            throw new NotImplementedException();
        }

        void IResourceHandler.GetResponseHeaders(IResponse response, out long responseLength, out string redirectUrl)
        {
            responseLength = stream == null ? 0 : stream.Length;
            redirectUrl = null;

            response.StatusCode = 200;
            response.StatusText = "OK";
            response.MimeType = mimeType;
        }

        bool IResourceHandler.ProcessRequest(IRequest request, ICallback callback)
        {
            // The 'host' portion is entirely ignored by this scheme handler.
            var uri = new Uri(request.Url);
            var fileName = uri.AbsolutePath;           
            Assembly ass = Assembly.GetExecutingAssembly();
            string resourcePath = ass.GetName().Name + "." + fileName.Replace("/", ".");
            
            if (!string.IsNullOrEmpty(request.Url))
            {
                Task.Run(() =>
                {
                    using (callback)
                    {
                        var bytes = Encoding.UTF8.GetBytes(request.GetCharSet());
                        stream = new MemoryStream(bytes);

                        var fileExtension = Path.GetExtension(fileName);
                        mimeType = ResourceHandler.GetMimeType(fileExtension);

                        callback.Continue();
                    }
                });

                return true;
            }
            else
            {
                callback.Dispose();
            }

            return false;
        }

        bool IResourceHandler.ReadResponse(Stream dataOut, out int bytesRead, ICallback callback)
        {
            //Dispose the callback as it's an unmanaged resource, we don't need it in this case
            callback.Dispose();

            if (stream == null)
            {
                bytesRead = 0;
                return false;
            }

            //Data out represents an underlying buffer (typically 32kb in size).
            var buffer = new byte[dataOut.Length];
            bytesRead = stream.Read(buffer, 0, buffer.Length);

            dataOut.Write(buffer, 0, buffer.Length);

            return bytesRead > 0;
        }
    }
}
