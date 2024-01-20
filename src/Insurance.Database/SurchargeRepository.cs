using Insurance.Contracts.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using AppModel = Insurance.Contracts.Plugins.Database;

namespace Insurance.Database
{
    internal class SurchargeRepository : ISurchargeRepository
    {
        private readonly InsuranceDbContext context;

        public SurchargeRepository(InsuranceDbContext context)
        {
            this.context = context;
        }


        #region Public Methods

        public async Task SaveSurchargeAsync(List<AppModel.Surcharge> surcharges)
        {
            // removing the old ones
            var ProductTypeIds = surcharges.Select(u => u.ProductTypeId).ToList();
            var entities = context.Surcharges.Where(u => ProductTypeIds.Contains(u.ProductTypeId));
            context.Surcharges.RemoveRange(entities);

            // adding new ones
            var newSurcharges = surcharges.Select(u => new Entities.Surcharge()
            {
                ProductTypeId = u.ProductTypeId,
                PercentRate = u.PercentRate,
            });
            await context.Surcharges.AddRangeAsync(newSurcharges);

            // commit
            await context.SaveChangesAsync();
        }

        public async Task<List<AppModel.Surcharge>> GetSurchargesAsync(List<int> productTypeIds)
        {
            return await context.Surcharges
                .Where(u => productTypeIds.Contains(u.ProductTypeId))
                .Select(u => new AppModel.Surcharge()
                {
                    ProductTypeId = u.ProductTypeId,
                    PercentRate = u.PercentRate,
                })
                .ToListAsync();
        }

        #endregion
    }
}
