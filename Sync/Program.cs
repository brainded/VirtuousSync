using Autofac;
using Autofac.Core;
using DAL;
using DAL.Repository;
using RestSharp;
using Services;
using System.Configuration;

namespace Sync
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var container = BuildContainer();

            container.Resolve<Application>().Sync().GetAwaiter().GetResult();
        }
        static IContainer BuildContainer()
        {
            var persistentStore = ConfigurationManager.AppSettings["PersistentStore"];
            var apiKey = ConfigurationManager.AppSettings["ApiKey"];

            var builder = new ContainerBuilder();
            builder.RegisterType<Services.Configuration>()
                   .As<IConfiguration>()
                   .WithParameter("apiKey", apiKey)
                   .SingleInstance();

            builder.RegisterType<VirtuousService>()
                   .As<IVirtuousService>();
            

            builder.RegisterType<DonorContext>();

            builder.RegisterGeneric(typeof(Repository<>)).As(typeof(IRepository<>));
            builder.RegisterGeneric(typeof(CsvRepository<>)).As(typeof(IRepository<>));
            builder.RegisterGeneric(typeof(Repository<>))
                .WithParameter(TypedParameter.From(typeof(Contact)));
            builder.RegisterGeneric(typeof(CsvRepository<>))
                .WithParameter(TypedParameter.From(typeof(Contact)));

            builder.RegisterType<Application>().WithParameter(
                            new ResolvedParameter(
                              (pi, ctx) => pi.ParameterType.Name == typeof(IRepository<>).Name,
                              (pi, ctx) =>
                              {
                                  if (persistentStore == "DB")
                                  {
                                      return ctx.Resolve(typeof(Repository<>).MakeGenericType(pi.ParameterType.GenericTypeArguments[0]));
                                  }
                                  else
                                  {
                                      return ctx.Resolve(typeof(CsvRepository<>).MakeGenericType(pi.ParameterType.GenericTypeArguments[0]));
                                  }
                              }));
            return builder.Build();
        }
    }
}
