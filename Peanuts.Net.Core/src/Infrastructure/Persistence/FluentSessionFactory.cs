using System;
using System.Reflection;

using FluentNHibernate.Cfg;
using FluentNHibernate.Conventions.Helpers;
using FluentNHibernate.Conventions.Inspections;

using NHibernate.Cfg;

using Spring.Data.NHibernate;

namespace Com.QueoFlow.Peanuts.Net.Core.Infrastructure.Persistence {
    public class FluentSessionFactory : LocalSessionFactoryObject {
        private static FluentSessionFactory _factoryObject;

        public FluentSessionFactory() {
            _factoryObject = this;
        }

        [Obsolete("Nur zur Verwendung in Testklassen, damit das Schema in der InMemory DB generiert werden kann.")]
        public static FluentSessionFactory FactoryObject {
            get { return _factoryObject; }
        }

        /// <summary>
        ///     Auflistung der Assemblies, in denen Mapping Fluent/HBM Mapping Dateien vorkommen.
        /// </summary>
        public string[] FluentMappingAssemblies { get; set; }

        protected override void PostProcessConfiguration(Configuration config) {
            base.PostProcessConfiguration(config);

            Fluently.Configure(config).Mappings(m => {
                foreach (string assemblyName in FluentMappingAssemblies) {
                    /* HBM Mappings der Konfiguration hinzufügen. */
                    m.HbmMappings.AddFromAssembly(Assembly.Load(assemblyName));

                    /* Fluent Mappings der Konfiguration hinzufügen. */
                    m.FluentMappings.AddFromAssembly(Assembly.Load(assemblyName))
                            .Conventions.Add(Table.Is(x => "tbl" + x.EntityType.Name))
                            .Conventions.Add(DefaultAccess.CamelCaseField(CamelCasePrefix.Underscore));
                }
            }).BuildConfiguration();
            // noch vor BuilConfiguration() steht: .ExposeConfiguration(cfg => new SchemaExport(cfg).Create(true, true)) (wenn es benötigt wird.
        }
    }
}