using System.Web.Http;
using WebActivatorEx;
using EurisTest.Web;
using Swashbuckle.Application;

[assembly: PreApplicationStartMethod(typeof(SwaggerConfig), "Register")]

namespace EurisTest.Web
{
    public class SwaggerConfig
    {
        public static void Register()
        {
            var thisAssembly = typeof(SwaggerConfig).Assembly;

            GlobalConfiguration.Configuration
                .EnableSwagger(c =>
                    {
                        c.SingleApiVersion("v1", "EurisTest.Web");
                        c.IncludeXmlComments(string.Format(@"{0}\bin\EurisTest.Web.xml", System.AppDomain.CurrentDomain.BaseDirectory));

                    })
                .EnableSwaggerUi(c =>
                    {
                        c.DocumentTitle("Swagger EurisTest.Web Api");
                        c.DocExpansion(DocExpansion.Full);
                    });
        }
    }
}
