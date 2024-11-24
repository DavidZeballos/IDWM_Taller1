using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IDWM_TallerAPI.Src.Models;

namespace IDWM_TallerAPI.Src.Interfaces.Repository
{
    public interface IPurchaseRepository
    {
        Task AddPurchase(Purchase purchase);

        Task<Purchase?> GetPurchaseById(int id);
        Task<IEnumerable<Purchase>> GetPurchasesById(int id);
        Task<IEnumerable<Purchase>> GetAllPurchases();
        Task<IEnumerable<Purchase>> GetPurchasesByQuery(int? id, DateTime? date, int? price);

    }
}