using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRS.Infrastructure.Storage
{
    public class AggregateItemDescriptor
    {
        [Key]
        public Guid AggregateId { get; set; }

        //public string AggregateType { get; set; }

    }
}
