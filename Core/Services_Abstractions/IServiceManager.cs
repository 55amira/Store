using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services_Abstractions
{
    public interface IServiceManager
    {
         IProductService productService { get;}
         IBasketService BasketService { get; }
         ICacheService CacheService { get; }
         IAuthService AuthService { get; }

    }
}
