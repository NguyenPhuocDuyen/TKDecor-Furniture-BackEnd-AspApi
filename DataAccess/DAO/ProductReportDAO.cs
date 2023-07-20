﻿using BusinessObject;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAO
{
    internal class ProductReportDAO : DAO<ProductReport>
    { 
        internal static async Task<List<ProductReport>> GetAll()
        {
            try
            {
                using var context = new TkdecorContext();
                var reports = await context.ProductReports
                    .Include(x => x.UserReport)
                    .Include(x => x.ProductReported)
                    .ToListAsync();
                return reports;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        internal static async Task<ProductReport?> FindById(Guid id)
        {
            try
            {
                using var context = new TkdecorContext();
                var report = await context.ProductReports
                    .Include(x => x.UserReport)
                    .Include(x => x.ProductReported)
                    .FirstOrDefaultAsync(x => x.ProductReportId == id);
                return report;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        internal static async Task<ProductReport?> FindByUserIdAndProductId(Guid userId, Guid productId)
        {
            try
            {
                using var context = new TkdecorContext();
                var report = await context.ProductReports
                    .Include(x => x.UserReport)
                    .Include(x => x.ProductReported)
                    .FirstOrDefaultAsync(x => x.UserReportId == userId
                        && x.ProductReportedId == productId);
                return report;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
    }
}
