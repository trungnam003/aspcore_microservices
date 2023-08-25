using Infrastructure.Common.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Extensions
{
    public static class MongoCollectionExtension
    {
        public static Task<PagedList<TDestination>> PaginatedListAsync<TDestination>
            (
                this IMongoCollection<TDestination> collection,
                FilterDefinition<TDestination> filter,
                int pageNumber, int pageSize
            ) where TDestination : class
        {
            var pagedList = PagedList<TDestination>
                                .ToPagedList(collection, filter, pageNumber, pageSize);
            return pagedList;
        }
        
    }
}
