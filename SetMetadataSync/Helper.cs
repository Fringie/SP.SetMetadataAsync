using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CredentialManagement;
using Microsoft.SharePoint.Client;

namespace SetMetadataSync
{
    static class Helper
    {

        private static readonly Credential Cm = new Credential {Target = App.Default.WebUrl};

        /// <summary>
        /// Get the current context web url is stored in the Credential Manager
        /// See settings file for name
        /// </summary>
        public static ClientContext GetWebCtx()
        {
            ClientContext ctx = null;
            if (Cm.Load())
            {
                ctx = new ClientContext(Cm.Target);
                ctx.Credentials = new SharePointOnlineCredentials(Cm.Username, Cm.SecurePassword);
            }
            else
            {
                Console.WriteLine("Can't get context");
            }
            return ctx;
        }
        

        /// <summary>
        /// As executequery isn't thread safe multiple instances of clientcontext this may be required
        /// </summary>
        /// <param name="ctx">The ctx from the parent scope of the async task</param>
        /// <returns>a new clientcontext</returns>
        public static ClientContext CreateDuplicateNewClient(ClientContext ctx)
        {
            ClientContext context = new ClientContext(ctx.Url);
            context.Credentials = ctx.Credentials;
            return context;
        }
    }
}
