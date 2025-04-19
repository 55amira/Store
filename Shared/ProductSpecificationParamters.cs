using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public class ProductSpecificationParamters
    {
        public int? BrandId { get; set; }
        public int? TypeId { get; set; }
        public string? Sort { get; set; }
        public string? Sreach { get; set; }
        private int _Pagesize = 5;
        private int _PageIndex = 1;

        public int PageIndex
        {
            get { return _PageIndex ; }
            set { _PageIndex  = value; }
        }


        public int Pagesize
        {
            get { return _Pagesize ; }
            set { _Pagesize = value; }
        }

    }
}
