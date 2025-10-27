using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Specifications;

namespace Talabat.Repository
{
    internal static class SpecificationsEvaluator<TEntity> where TEntity : BaseEntity
    {
        public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> inputQuery, ISpecification<TEntity> spec)
        {
            var query = inputQuery;//_dbContext.set<Product>()

            if (spec.Criteria is not null)
                query = query.Where(spec.Criteria);
            //query = _dbContext.set<Product>().Where(P => P.Id == id)

            //Includes
            //1. p => p.Brand
            //2. p => p.Category

            if(spec.OrderBy is not null)
               query = query.OrderBy(spec.OrderBy);

            else if(spec.OrderByDesc is not null)
                query=query.OrderByDescending(spec.OrderByDesc);

            if(spec.IsPaginationEnable)
                query = query.Skip(spec.Skip).Take(spec.Take);


            query = spec.Includes.Aggregate(query,(currentQuery,includeExpression)
                => currentQuery.Include(includeExpression));

            // at first iteration will return 
            //_dbContext.set<Product>().Where(P => P.Id == id).Include(P => P.Brand)
            //at second iteration will return
            //_dbContext.set<Product>().Where(P => P.Id == id).Include(P => P.Brand).Include(P => P.Category)

            return query;
        }

    }
}
