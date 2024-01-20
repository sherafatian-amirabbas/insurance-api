using Insurance.Contracts.Application.Exceptions;
using Insurance.Contracts.Application.Interfaces;
using Insurance.Contracts.Plugins.Database;
using System.Net;

namespace Insurance.Application
{
    public class SurchargeService : ISurchargeService
    {
        private readonly ISurchargeRepository surchargeRepository;


        public SurchargeService(ISurchargeRepository surchargeRepository)
        {
            this.surchargeRepository = surchargeRepository;
        }


        #region Properties

        public async Task SaveSurchargeAsync(List<Surcharge> surcharges)
        {
            var invalidSurcharges = surcharges
                .Where(u => u.PercentRate < 0 || u.PercentRate > 100)
                .Select(u => u.ProductTypeId)
                .ToList();

            if (invalidSurcharges.Any())
            {
                var ids = string.Join(", ", invalidSurcharges);
                throw new BusinessException($"The following productTypeId(s) have invalid rate: {ids}",
                    HttpStatusCode.BadRequest);
            }

            await surchargeRepository.SaveSurchargeAsync(surcharges);
        }

        public async Task<List<Surcharge>> GetSurchargesAsync(List<int> productTypeIds)
        {
            return await surchargeRepository.GetSurchargesAsync(productTypeIds);
        }

        #endregion
    }
}
