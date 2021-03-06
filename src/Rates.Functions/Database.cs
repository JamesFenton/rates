﻿using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Rates.Functions
{
    public class Database
    {
        public const string RatesTable = "rates";
        public const string RatesRmTable = "ratesrm";

        private readonly CloudTableClient _client;

        public CloudTable Rates { get; }

        public CloudTable RatesRm { get; }

        public Database(string connectionString)
        {
            var storageAccount = CloudStorageAccount.Parse(connectionString);
            _client = storageAccount.CreateCloudTableClient();

            Rates = _client.GetTableReference(RatesTable);
            RatesRm = _client.GetTableReference(RatesRmTable);
        }
    }
}
