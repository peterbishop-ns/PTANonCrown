using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PTANonCrown.Models;
using PTANonCrown.Context;

namespace PTANonCrown.Repository
{
    public interface IStandRepository : IBaseRepository<Stand>
    {
    }

    public class StandRepository : BaseRepository<Stand>, IStandRepository
    {
        private readonly AppDbContext _context;

        public StandRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
    }
}