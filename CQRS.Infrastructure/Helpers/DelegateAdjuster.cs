using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CQRS.Infrastructure.Helpers
{
    internal static class aDelegateAdjuster
    {
        public static Action<TBase> CastArgument<TBase, TDerived>(Expression<Action<TDerived>> source) where TDerived : TBase
        {
            if (typeof(TDerived) == typeof(TBase))
            {
                return (Action<TBase>)((Delegate)source.Compile());
            }

            var sourceParameter = Expression.Parameter(typeof(TBase), "source");
            var result = Expression.Lambda<Action<TBase>>(
                Expression.Invoke(
                    source,
                    Expression.Convert(sourceParameter, typeof(TDerived))),
                sourceParameter);
            return result.Compile();
        }
    }
}
