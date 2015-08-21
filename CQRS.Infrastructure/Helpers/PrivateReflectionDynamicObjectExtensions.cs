using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRS.Infrastructure.Helpers
{
    internal static class aPrivateReflectionDynamicObjectExtensions
    {
        public static dynamic aAsDynamic(this object o)
        {
            return aPrivateReflectionDynamicObject.WrapObjectIfNeeded(o);
        }
    }
}
