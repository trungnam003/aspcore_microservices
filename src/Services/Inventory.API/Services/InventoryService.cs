using AutoMapper;
using Infrastructure.Common.Models;
using Infrastructure.Extensions;
using Inventory.API.Entities;
using Inventory.API.Extensions;
using Inventory.API.Repositories.Abstraction;
using Inventory.API.Services.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;
using Shared.DTOs.Inventory;
using System.Linq.Expressions;

namespace Inventory.API.Services
{
    public class InventoryService : MongoDbRepository<InventoryEntry>, IInventoryService
    {
        private readonly IMapper _mapper;

        public InventoryService(IMapper mapper, IMongoClient database, DatabaseSettings databaseSettings) : base(database, databaseSettings)
        {
            _mapper = mapper;
        }

        public async Task<IEnumerable<InventoryEntryDto>> GetAllByItemNoAsync(string itemNo)
        {
            var entities = await FindAll()
                .Find(x=> x.ItemNo.Equals(itemNo))
                .ToListAsync();

            var result = _mapper.Map<IEnumerable<InventoryEntryDto>>(entities);
            return result;
        }

        public async Task<PagedList<InventoryEntryDto>> GetAllByItemNoPagingAsync(GetInventoryPagingQuery query)
        {
            var filterSearchTerm = Builders<InventoryEntry>.Filter.Empty;
            var filterItemNo = Builders<InventoryEntry>.Filter.Eq( x => x.ItemNo, query.ItemNo());
            if(!string.IsNullOrEmpty(query.SearchTerm))
            {
                filterSearchTerm = Builders<InventoryEntry>.Filter.Eq(x=>x.DocumentNo, query.SearchTerm);
            }
            var andFilter = filterItemNo & filterSearchTerm;
            var pageList = await Collection.PaginatedListAsync(andFilter, query.PageNumber, query.PageSize);

            var items = _mapper.Map<IEnumerable<InventoryEntryDto>>(pageList);

            var result = new PagedList<InventoryEntryDto>
                (items, pageList.GetMetaData().TotalPages, query.PageNumber, query.PageNumber );

            return result;
        } 

        public async Task<InventoryEntryDto> GetByIdAsync(string id)
        {
            FilterDefinition<InventoryEntry> filter = Builders<InventoryEntry>.Filter.Eq(x => x.Id, id);
            var entity = await FindAll().Find(filter).FirstOrDefaultAsync();
            var result = _mapper.Map<InventoryEntryDto>(entity);
            return result;
        }

        public async Task<InventoryEntryDto> PurchaseItemAsync(string itemNo, PurchaseProductDto dto)
        {
            var itemToAdd = new InventoryEntry(ObjectId.GenerateNewId().ToString())
            {
                ItemNo = itemNo,
                Quantity = dto.Quantity,
                DocumentType = dto.DocumentType,
            };
            await CreateAsync(itemToAdd);
            var result = _mapper.Map<InventoryEntryDto>(itemToAdd);
            return result;
        }
    }
}
