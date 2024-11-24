using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IDWM_TallerAPI.Src.DTOs;
using IDWM_TallerAPI.Src.Models;

namespace IDWM_TallerAPI.Src.Interfaces.Service
{
    public interface IPurchaseService
    {

        public Task<VoucherDto> AddPurchase(int productId, int userId, MakePurchaseDto makePurchaseDto);    
        public Task<IEnumerable<VoucherDto>> GetPurchasesById(int id);
        public Task<IEnumerable<VoucherDto>> GetAllPurchases();
        public Task<IEnumerable<VoucherDto>> GetPurchasesByQuery(int? id, DateTime? date, int? price);

    }
}