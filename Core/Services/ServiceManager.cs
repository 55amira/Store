using AutoMapper;
using Domain.Contracts;
using Services_Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class ServiceManager (IUnitOfWork unitOfWork, IMapper mapper) : IServiceManager
    {
        public IProductService productService =>  new ProductService( unitOfWork,  mapper);
    }
}
