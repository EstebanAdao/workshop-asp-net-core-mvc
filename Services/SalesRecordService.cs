using SalesWebMvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace SalesWebMvc.Services
{
    public class SalesRecordService
    {
        private readonly SalesWebMvcContext _context;

        public SalesRecordService(SalesWebMvcContext context)
        {
            _context = context;
        }
        public async Task<List<SalesRecord>> FindByDateAsync(DateTime? minDate, DateTime? maxDate)
        {
            var result = from obj in _context.SalesRecord select obj;
            if (minDate.HasValue)
            {
                result = result.Where(x => x.Date >= minDate.Value);
            }
            if (maxDate.HasValue)
            {
                result = result.Where(x => x.Date <= maxDate.Value);
            }
            return await result
                .Include(x => x.Seller)
                .Include(x => x.Seller.Department)
                .OrderByDescending(x => x.Date)
                .ToListAsync();
        }

        public async Task<IEnumerable<IGrouping<Department, SalesRecord>>> FindByDateGroupingAsync1(DateTime? minDate, DateTime? maxDate)
        {
            var result = from obj in _context.SalesRecord select obj;
            if (minDate.HasValue)
            {
                result = result.Where(x => x.Date >= minDate.Value);
            }
            if (maxDate.HasValue)
            {
                result = result.Where(x => x.Date <= maxDate.Value);
            }
            result = result.Include(x => x.Seller);

            result = result.Include(x => x.Seller.Department);

            result = result.OrderByDescending(x => x.Date);

            return await result
             .GroupBy(x => x.Seller.Department)
             .ToListAsync();
        }

        public async Task<Dictionary<Department, List<SalesRecord>>> FindByDateGroupingAsync2(DateTime? initial, DateTime? final)
        {
            var salesRecords = await FindByDateAsync(initial, final);
            var departments = _context.Department.ToList();
            var grouping = new Dictionary<Department, List<SalesRecord>>();

            departments.ForEach(d =>
            {
                var sales = salesRecords.Where(sr => sr.Seller.Department == d).ToList();
                grouping.Add(d, sales);
            });

            return grouping;
        }
    }
}
