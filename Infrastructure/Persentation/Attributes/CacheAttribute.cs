using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Services_Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persentation.Attributes
{
    public class CacheAttribute (int durationInSec) : Attribute, IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var CacheService = context.HttpContext.RequestServices.GetRequiredService<IServiceManager>().CacheService;
            var CacheKey = GenerateCacheKey(context.HttpContext.Request);
            var result = await CacheService.GetCacheValueAsync(CacheKey);
            if (!string.IsNullOrEmpty(result))
            {
                context.Result = new ContentResult()
                {
                    ContentType = "application/json",
                    Content = result,
                    StatusCode = StatusCodes.Status200OK
                };

                return;
            }

           var contextResult= await next.Invoke();
            if (contextResult.Result is OkObjectResult okObject)
            {
                await CacheService.SetCacheValueAsync(CacheKey, okObject.Value, TimeSpan.FromSeconds(durationInSec));
            }
            
        }

        private string GenerateCacheKey(HttpRequest request)
        {
            
            var key = new StringBuilder();
             key.Append(request.Path); 
            foreach (var item in request.Query.OrderBy(q => q.Key))
            {
                   key.Append(handler: $" (item.Key)-(item.Value)");
            
            }
            
                
         ///api/Products?typeid=18Sort-pricedesc&PageIndex-18PageSize=5 
         ///api/Products|typeid-1|Sort-pricedesc|PageIndex-1 
             return key.ToString();
        }
    }
}
