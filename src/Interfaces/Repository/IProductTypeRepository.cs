using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IDWM_TallerAPI.Src.Models;


namespace IDWM_TallerAPI.Src.Interfaces.Repository
{
    public interface IProductTypeRepository
    {
        Task<ProductType?> GetProductTypeById(int id);
        Task<ProductType?> GetProductTypeByName(string name);
        Task<IEnumerable<ProductType>> GetAllProductTypes();
    }
}