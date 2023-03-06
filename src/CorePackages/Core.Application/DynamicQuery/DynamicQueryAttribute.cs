using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;

namespace Core.Application.DynamicQuery
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class DynamicQueryAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ActionArguments.Where(x => x.Value as IDynamicQuery is not null).FirstOrDefault().Value is not IDynamicQuery dynamicQuery)
                return;

            var dynamicSort = context.HttpContext.Request.Query.SingleOrDefault(x => x.Key == "Dynamic.Sort" && !x.Value.Contains(null));
            if (dynamicSort.Value.Count > 0)
            {
                dynamicQuery.Dynamic ??= new();
                if (dynamicSort.Value.ToString().StartsWith("[") && dynamicSort.Value.ToString().EndsWith("]"))
                    dynamicQuery.Dynamic.Sort = JsonConvert.DeserializeObject<IEnumerable<Sort>>(dynamicSort.Value!);
                else
                    dynamicQuery.Dynamic.Sort = new List<Sort>(1) { JsonConvert.DeserializeObject<Sort>(dynamicSort.Value!)! }.AsEnumerable();
            }

            var dynamicFilter = context.HttpContext.Request.Query.SingleOrDefault(x => x.Key == "Dynamic.Filter.Filters" && !x.Value.Contains(null));
            if (dynamicFilter.Value.Count > 0)
            {
                ArgumentNullException.ThrowIfNull(dynamicQuery.Dynamic?.Filter);
                if (dynamicFilter.Value.ToString().StartsWith("[") && dynamicFilter.Value.ToString().EndsWith("]"))
                    dynamicQuery.Dynamic.Filter.Filters = JsonConvert.DeserializeObject<IEnumerable<Filter>>(dynamicFilter.Value!);
                else
                    dynamicQuery.Dynamic.Filter.Filters = new List<Filter>(1) { JsonConvert.DeserializeObject<Filter>(dynamicFilter.Value!)! }.AsEnumerable();
            }
        }
    }
}
