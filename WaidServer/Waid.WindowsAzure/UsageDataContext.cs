using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;

namespace Waid.WindowsAzure
{
    public class UsageDataContext : TableServiceContext
    {
        public const string UsageTable = "Usage";
   
        private static bool initialized;
        private static readonly object initializationLock = new object();

        //private readonly Dictionary<string, Type> resolverTypes;

        public UsageDataContext()
            : this(CloudStorageAccount.FromConfigurationSetting("DataConnectionString"))
        {
        }

        public UsageDataContext(CloudStorageAccount account)
            : base(account.TableEndpoint.ToString(), account.Credentials)
        {
            if (!initialized)
            {
                lock (initializationLock)
                {
                    if (!initialized)
                    {
                        CreateTables();
                        initialized = true;
                    }
                }
            }

            // we are setting up a dictionary of types to resolve in order
            // to workaround a performance bug during serialization
            //resolverTypes = new Dictionary<string, Type>();
            //resolverTypes.Add(UsageTable, typeof (UsageRow));

            //ResolveType = (name) =>
            //                  {
            //                      string[] parts = name.Split('.');
            //                      if (parts.Length == 2)
            //                      {
            //                          return resolverTypes[parts[1]];
            //                      }

            //                      return null;
            //                  };
        }


        public IQueryable<UsageRow> Usage
        {
            get { return CreateQuery<UsageRow>(UsageTable); }
        }

        public void CreateTables(Type serviceContextType, string baseAddress, StorageCredentials credentials)
        {
            TableStorageExtensionMethods.CreateTablesFromModel(serviceContextType, baseAddress, credentials);
        }

        public void CreateTables()
        {
            TableStorageExtensionMethods.CreateTablesFromModel(typeof (UsageDataContext), BaseUri.AbsoluteUri,
                                                               StorageCredentials);
        }
    }
}