using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETHTPS.Data.Core.Models.LiveData
{
    public sealed class TransactionMetadata
    {
        public string? Hash { get; set; }
        public DateTime? Date { get; set; }
    }
}
