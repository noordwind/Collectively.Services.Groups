using System.Collections.Generic;
using System.Globalization;
using Autofac;
using Collectively.Messages.Commands;
using Collectively.Common.Files;
using Collectively.Common.Mongo;
using Collectively.Common.Nancy;
using Collectively.Common.RabbitMq;
using Collectively.Common.Security;
using Microsoft.Extensions.Configuration;
using Nancy.Bootstrapper;
using Nancy.Bootstrappers.Autofac;
using NLog;
using RawRabbit.Configuration;
using System.Reflection;
using Collectively.Common.Exceptionless;
using Collectively.Common.Services;
using Nancy;
using Nancy.Configuration;
using Newtonsoft.Json;
using Collectively.Common.Extensions;
using System;
using Collectively.Messages.Events;
using Collectively.Messages.Events.Users;
using Collectively.Services.Groups.Repositories;
using Collectively.Services.Groups.Services;
using Collectively.Common.ServiceClients;
using Collectively.Messages.Events.Groups;

namespace Collectively.Services.Groups.Framework
{
    public class Bootstrapper : AutofacNancyBootstrapper
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private static IExceptionHandler _exceptionHandler;
        private readonly IConfiguration _configuration;
        public static ILifetimeScope LifetimeScope { get; private set; }

        public Bootstrapper(IConfiguration configuration)
        {
            _configuration = configuration;
        }

#if DEBUG
        public override void Configure(INancyEnvironment environment)
        {
            base.Configure(environment);
            environment.Tracing(enabled: false, displayErrorTraces: true);
        }
#endif

        protected override void ConfigureApplicationContainer(ILifetimeScope container)
        {
            base.ConfigureApplicationContainer(container);
            container.Update(builder =>
            {
                builder.RegisterType<CustomJsonSerializer>().As<JsonSerializer>().SingleInstance();
                builder.RegisterInstance(_configuration.GetSettings<MongoDbSettings>());
                builder.RegisterModule<MongoDbModule>();
                builder.RegisterType<MongoDbInitializer>().As<IDatabaseInitializer>();
                builder.RegisterType<Encrypter>().As<IEncrypter>().SingleInstance();
                builder.RegisterType<Handler>().As<IHandler>();
                builder.RegisterInstance(_configuration.GetSettings<ExceptionlessSettings>()).SingleInstance();
                builder.RegisterType<ExceptionlessExceptionHandler>().As<IExceptionHandler>().SingleInstance();
                builder.RegisterModule(new FilesModule(_configuration));
                builder.RegisterType<GroupRepository>().As<IGroupRepository>();
                builder.RegisterType<OrganizationRepository>().As<IOrganizationRepository>();
                builder.RegisterType<UserRepository>().As<IUserRepository>();
                builder.RegisterType<GroupService>().As<IGroupService>();
                builder.RegisterType<OrganizationService>().As<IOrganizationService>();
                builder.RegisterType<UserService>().As<IUserService>();
                builder.RegisterModule<ServiceClientModule>();
                builder.RegisterInstance(AutoMapperConfig.InitializeMapper());
                RegisterResourceFactory(builder);

                var assembly = typeof(Startup).GetTypeInfo().Assembly;
                builder.RegisterAssemblyTypes(assembly).AsClosedTypesOf(typeof(IEventHandler<>));
                builder.RegisterAssemblyTypes(assembly).AsClosedTypesOf(typeof(ICommandHandler<>));

                SecurityContainer.Register(builder, _configuration);
                RabbitMqContainer.Register(builder, _configuration.GetSettings<RawRabbitConfiguration>());
            });
            LifetimeScope = container;
        }

        protected override void RequestStartup(ILifetimeScope container, IPipelines pipelines, NancyContext context)
        {
            pipelines.OnError.AddItemToEndOfPipeline((ctx, ex) =>
            {
                _exceptionHandler.Handle(ex, ctx.ToExceptionData(),
                    "Request details", "Collectively", "Service", "Groups");

                return ctx.Response;
            });
        }

        protected override void ApplicationStartup(ILifetimeScope container, IPipelines pipelines)
        {
            var databaseSettings = container.Resolve<MongoDbSettings>();
            var databaseInitializer = container.Resolve<IDatabaseInitializer>();
            databaseInitializer.InitializeAsync();
            pipelines.AfterRequest += (ctx) =>
            {
                ctx.Response.Headers.Add("Access-Control-Allow-Origin", "*");
                ctx.Response.Headers.Add("Access-Control-Allow-Methods", "POST,PUT,GET,OPTIONS,DELETE");
                ctx.Response.Headers.Add("Access-Control-Allow-Headers", "Authorization, Origin, X-Requested-With, Content-Type, Accept");
            };
            pipelines.SetupTokenAuthentication(container);
            _exceptionHandler = container.Resolve<IExceptionHandler>();
            Logger.Info("Collectively.Services.Groups API has started.");
        }

        private void RegisterResourceFactory(ContainerBuilder builder)
        {
            var resources = new Dictionary<Type, string>
            {
                [typeof(GroupCreated)] = "groups/{0}",
                [typeof(OrganizationCreated)] = "organizations/{0}",
            };
            builder.RegisterModule(new ResourceFactory.Module(resources));
        }
    }
}