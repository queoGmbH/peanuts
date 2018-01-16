using System;
using System.Diagnostics;
using System.IO;

using Com.QueoFlow.Peanuts.Net.Core.Domain.Users;

using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Conventions.Helpers;
using FluentNHibernate.Conventions.Inspections;

using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;

namespace DbSchemaCreator {
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// If this console-application only writes to the control but does not write to the FILENAME-file, than NHibernate-Version of this projects has been updated. 
    /// It has to be 4.0.0.4000 or earlier!!! 
    /// The SchemaExport does not work with later NHibernate-Versions!!!
    /// </remarks>
    class Program {
        private const string FILENAME = "..\\..\\..\\Peanuts.Net.Core\\Database\\db_ddl.sql";

        private static void BuildSchema(Configuration obj)
        {

            TextWriter textWriter = new StringWriter();
            
                new SchemaExport(obj).Execute(Console.WriteLine, false, false, textWriter);
                using (var file = new FileStream(FILENAME, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                using (var sw = new StreamWriter(file))
                {
                    Debug.Write(textWriter.ToString());
                    sw.Write(textWriter.ToString());
                } 
                
            
        }

        private static void Main(string[] args) {
            Fluently.Configure()
                    .Database(MsSqlConfiguration.MsSql2012)
                    .Mappings(m => m.FluentMappings.AddFromAssemblyOf<User>()
                            .Conventions.Add(Table.Is(x => "tbl" + x.EntityType.Name))
                            .Conventions.Add(DefaultAccess.CamelCaseField(CamelCasePrefix.Underscore))
                            .Conventions.Add(ForeignKey.EndsWith("_Id")))
                    .ExposeConfiguration(BuildSchema).BuildConfiguration();

            Console.WriteLine("enter for exit");
            Console.ReadLine();
        }
    }
}