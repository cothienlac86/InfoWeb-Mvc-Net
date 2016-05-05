using CefSharp;
using System;

namespace GetNewsTools.Business
{
    public class SchemaHandlerFactory : ISchemeHandlerFactory
    {
        private static string m_SchemaName = string.Empty;
        public SchemaHandlerFactory(string schemaName)
        {
            m_SchemaName = schemaName;
        }
        IResourceHandler ISchemeHandlerFactory.Create(IBrowser browser, IFrame frame, string schemeName, IRequest request)
        {
            return new SchemaHandler();
        }

        public static string SchemaName()
        {
            return m_SchemaName;
        }
    }
}
