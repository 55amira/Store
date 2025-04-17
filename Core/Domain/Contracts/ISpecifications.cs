﻿using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contracts
{
    public interface ISpecifications <TEntity,TKey>  where TEntity : BaseEntity<TKey>
    {
         Expression<Func<TEntity,bool>>? Criteria { get; set; }
         List<Expression<Func<TEntity, object>>> IncludeExperssions { get; set; }

    }
}
