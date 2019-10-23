using KompaniaPchor.ORM_Models;
using KompaniaPchor.Repositories;
using KompaniaPchor.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KompaniaPchor.Services
{
    public class SoldierService : ISoldierService
    {
        private readonly GenericRepo<Zolnierz> _soldierRepo;

        public SoldierService(GenericRepo<Zolnierz> soldierRepo)
        {
            _soldierRepo = soldierRepo;
        }
        public async Task<Zolnierz> GetSoldierInfoAsync(int? soldierId)
        {
            return await _soldierRepo.Get().Where(s => s.IdOsoby == soldierId).SingleOrDefaultAsync();
        }
    }
}
