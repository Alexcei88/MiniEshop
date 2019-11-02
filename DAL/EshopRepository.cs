using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MiniEshop.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniEshop.DAL
{
    /// <summary>
    /// Репозиторий для доступа к данным о товарах
    /// </summary>
    public class EshopRepository
        : IEshopRepository
    {
        private readonly EshopDbContext _eshopDbContext;

        public EshopRepository(EshopDbContext eshopDbContext)
        {
            _eshopDbContext = eshopDbContext;
        }

        public async Task<(List<Category> categories, Category root)> GetCategoriesAsync()
        {
            // так как у нас используется в сущности HierarchyId, который нельзя маппить, и нам нужно маппить не все свойства(а это ограничение https://docs.microsoft.com/en-us/ef/core/querying/raw-sql)
            // то используем для получения данных обычную команду, можно притащить даппер, который с этим справиться,
            // но для одного метода тащит разумеется его не буду
            using (var connection = _eshopDbContext.Database.GetDbConnection())
            {
                connection.Open();
                List<Category> categories = new List<Category>();
                using (var command = new SqlCommand("Select Id, Name, ParentId from Categories", connection as SqlConnection))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            categories.Add(new Category()
                            {
                                Id = reader.GetGuid(0),
                                Name = reader.GetString(1),
                                ParentId = reader.GetGuid(2)
                            });
                        }
                    }
                }
                var root = categories.First(g => g.Id == Guid.Empty);
                categories.Remove(root);
                return (categories, root);
            }
        }

        public Task<Good[]> GetGoodsAsync(Guid category, int skip, int size)
        {
            return _eshopDbContext.Goods
                .Where(g => g.CategoryId == category)
                .OrderBy(g => g.Id)
                .Skip(skip)
                .Take(size)
                .ToArrayAsync();
        }

        public Task<int> CreateGoodAsync(Good good)
        {
            _eshopDbContext.Goods.Add(good);
            return _eshopDbContext.SaveChangesAsync();
        }

        public Task<int> UpdateGoodAsync(Good good)
        {
            _eshopDbContext.Update(good);
            return _eshopDbContext.SaveChangesAsync();
        }

        public async Task<Good[]> DeleteGoodAsync(List<Guid> ids)
        {
            var goods = await _eshopDbContext.Goods.Where(x => ids.Contains(x.Id)).ToArrayAsync();
            _eshopDbContext.Goods.RemoveRange(goods);
            await _eshopDbContext.SaveChangesAsync();
            return goods;
        }

        public Task<int> GetGoodCountAsync(Guid category)
        {
            return _eshopDbContext.Goods.Where(g => g.CategoryId == category).CountAsync();
        }
    }
}
