using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using Microsoft.Practices.Prism;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.Logging;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Prism.Regions.Behaviors;
using Microsoft.Practices.ServiceLocation;
using StructureMap;
using StructureMap.Exceptions;
using StructureMap.ServiceLocatorAdapter;

namespace ChinookMediaManager.Infrastructure
{
    public abstract class StructureMapBootstrapper : Microsoft.Practices.Prism.Bootstrapper
    {
        private bool _useDefaultConfiguration = true;

        private readonly ILoggerFacade _loggerFacade = new TraceLogger();

        public ILoggerFacade LoggerFacade { get { return _loggerFacade; } }

        public IContainer Container { get; private set; }

        public override void Run(bool runWithDefaultConfiguration)
        {
            _useDefaultConfiguration = runWithDefaultConfiguration;

            var logger = LoggerFacade;
            if (logger == null) throw new InvalidOperationException("NullLoggerException");

            logger.Log("Creating StructureMap container", Category.Debug, Priority.Low);
            
            Container = CreateContainer();
            if (Container == null) throw new InvalidOperationException("NullContainerException");

            logger.Log("Configuring container", Category.Debug, Priority.Low);
            ConfigureContainer();

            logger.Log("Configuring region adapters", Category.Debug, Priority.Low);
            ConfigureRegionAdapterMappings();
            ConfigureDefaultRegionBehaviors();
            RegisterFrameworkExceptionTypes();

            logger.Log("Creating shell", Category.Debug, Priority.Low);
            var shell = CreateShell();
            if (shell != null)
            {
                RegionManager.SetRegionManager(shell, Container.GetInstance<IRegionManager>());
                RegionManager.UpdateRegions();
            }

            logger.Log("Initializing modules", Category.Debug, Priority.Low);
            InitializeModules();

            logger.Log("Bootstrapper sequence completed", Category.Debug, Priority.Low);
        }

        /// <summary>
        /// Configures the <see cref="IRegionBehaviorFactory"/>. This will be the list of default
        /// behaviors that will be added to a region. 
        /// </summary>
        protected override IRegionBehaviorFactory ConfigureDefaultRegionBehaviors()
        {
            var defaultRegionBehaviorTypesDictionary = Container.TryGetInstance<IRegionBehaviorFactory>();

            if (defaultRegionBehaviorTypesDictionary != null)
            {
                defaultRegionBehaviorTypesDictionary.AddIfMissing(AutoPopulateRegionBehavior.BehaviorKey,
                                                                  typeof(AutoPopulateRegionBehavior));

                defaultRegionBehaviorTypesDictionary.AddIfMissing(BindRegionContextToDependencyObjectBehavior.BehaviorKey,
                                                                  typeof(BindRegionContextToDependencyObjectBehavior));

                defaultRegionBehaviorTypesDictionary.AddIfMissing(RegionActiveAwareBehavior.BehaviorKey,
                                                                  typeof(RegionActiveAwareBehavior));

                defaultRegionBehaviorTypesDictionary.AddIfMissing(SyncRegionContextWithHostBehavior.BehaviorKey,
                                                                  typeof(SyncRegionContextWithHostBehavior));

                defaultRegionBehaviorTypesDictionary.AddIfMissing(RegionManagerRegistrationBehavior.BehaviorKey,
                                                                  typeof(RegionManagerRegistrationBehavior));

            }
            return defaultRegionBehaviorTypesDictionary;

        }

        /// <summary>
        /// Registers in the <see cref="IContainer"/> the <see cref="Type"/> of the Exceptions
        /// that are not considered root exceptions by the <see cref="ExceptionExtensions"/>.
        /// </summary>
        protected override void RegisterFrameworkExceptionTypes()
        {
            ExceptionExtensions.RegisterFrameworkExceptionType(
                typeof(ActivationException));

            ExceptionExtensions.RegisterFrameworkExceptionType(
                typeof(MissingPluginFamilyException));
        }

        /// <summary>
        /// Configures the <see cref="IContainer"/>. May be overwritten in a derived class to add specific
        /// type mappings required by the application.
        /// </summary>
        protected virtual void ConfigureContainer()
        {
            Container.Configure(reg =>
                {
                    reg.For<IContainer>().Use(() => Container);
                    reg.For<ILoggerFacade>().Use(LoggerFacade);
                    reg.ForSingletonOf<IModuleCatalog>().Use(GetModuleCatalog);

                    if (!_useDefaultConfiguration) return;

                    reg.RegisterSingletonType<IServiceLocator, StructureMapServiceLocator>();
                    reg.RegisterSingletonType<IModuleInitializer, ModuleInitializer>();
                    reg.RegisterSingletonType<IModuleManager, ModuleManager>();
                    reg.RegisterSingletonType<RegionAdapterMappings, RegionAdapterMappings>();
                    reg.RegisterSingletonType<IRegionManager, RegionManager>();
                    reg.RegisterSingletonType<IEventAggregator, EventAggregator>();
                    reg.RegisterSingletonType<IRegionViewRegistry, RegionViewRegistry>();
                    reg.RegisterSingletonType<IRegionBehaviorFactory, RegionBehaviorFactory>();
                    reg.RegisterSingletonType<IRegionNavigationContentLoader, RegionNavigationContentLoader>();

                    ServiceLocator.SetLocatorProvider(() => Container.GetInstance<IServiceLocator>());
                });
        }
        protected override void  ConfigureServiceLocator()
        {
            ServiceLocator.SetLocatorProvider(() => Container.GetInstance<IServiceLocator>());
        }

        /// <summary>
        /// Configures the default region adapter mappings to use in the application, in order
        /// to adapt UI controls defined in XAML to use a region and register it automatically.
        /// May be overwritten in a derived class to add specific mappings required by the application.
        /// </summary>
        /// <returns>The <see cref="RegionAdapterMappings"/> instance containing all the mappings.</returns>
        protected override RegionAdapterMappings ConfigureRegionAdapterMappings()
        {
            var regionAdapterMappings = Container.GetInstance<RegionAdapterMappings>();

            if (regionAdapterMappings != null)
            {
                regionAdapterMappings.RegisterMapping(typeof(Selector), Container.GetInstance<SelectorRegionAdapter>());
                regionAdapterMappings.RegisterMapping(typeof(ItemsControl), Container.GetInstance<ItemsControlRegionAdapter>());
                regionAdapterMappings.RegisterMapping(typeof(ContentControl), Container.GetInstance<ContentControlRegionAdapter>());
            }

            return regionAdapterMappings;
        }

        /// <summary>
        /// Initializes the modules. May be overwritten in a derived class to use custom
        /// module loading and avoid using an <seealso cref="IModuleManager"/> and
        /// <seealso cref="IModuleManager"/>.
        /// </summary>
        protected override void InitializeModules()
        {
            IModuleManager manager;

            try
            {
                manager = Container.GetInstance<IModuleManager>();
            }
            catch (StructureMapException ex)
            {
                if (ex.Message.Contains("IModuleCatalog"))
                    throw new InvalidOperationException("Module not found.");

                throw;
            }

            manager.Run();
        }

        /// <summary>
        /// Creates the <see cref="IContainer"/> that will be used as the default container.
        /// </summary>
        /// <returns>A new instance of <see cref="IContainer"/>.</returns>
        protected virtual IContainer CreateContainer()
        {
            return new Container(x=> { });
        }

        /// <summary>
        /// Returns the module enumerator that will be used to initialize the modules.
        /// </summary>
        /// <remarks>
        /// When using the default initialization behavior, this method must be overwritten by a derived class.
        /// </remarks>
        /// <returns>An instance of <see cref="IModuleCatalog"/> that will be used to initialize the modules.</returns>
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        protected virtual IModuleCatalog GetModuleCatalog()
        {
            return new ModuleCatalog();
        }
   }
    
    public static class StructureMapPrismExtensions
    {
         public static void RegisterSingletonType<TFrom, TTo>(this ConfigurationExpression reg)
            where TFrom : class
            where TTo : class, TFrom
         {
                        reg.ForSingletonOf<TFrom>().Use<TTo>();
         }

        public static void RegisterTypeIfMissing<TFrom, TTo>(this ConfigurationExpression reg)
            where TFrom : class
            where TTo : class, TFrom
        {
                        reg.For<TFrom>().Use<TTo>();
        }

    }
}