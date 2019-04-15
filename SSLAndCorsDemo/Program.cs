using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Rewrite.Internal.ApacheModRewrite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace SSLAndCorsDemo
{
    public class Program
    {
        /// <summary>
        /// Command to create pfx file run below command on powershell=>
        ///  New-SelfSignedCertificate -DnsName "localhost" -CertStoreLocation "cert:\LocalMachine\My"
        ///  $CertPassword = ConvertTo-SecureString -String “test#123” -Force –AsPlainText
        /// Export-PfxCertificate -Cert cert:\LocalMachine\My\8194B39E313C2CAE0F595842EC37815BC44B9D57 -FilePath C:\test.pfx -Password $CertPassword
        /// Export-Certificate -Cert Cert:\LocalMachine\My\8194B39E313C2CAE0F595842EC37815BC44B9D57 -FilePath C:\tstcert.cer
        /// certlm.msc=> to see the certificate
        /// 
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
          CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseKestrel(cfg =>
                {
                    cfg.Listen(IPAddress.Loopback, 5000);
                    cfg.Listen(IPAddress.Loopback,5001, listenOptions => { listenOptions.UseHttps("test.pfx", "test#123"); });
                })
                .UseStartup<Startup>()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration();
    }
}
