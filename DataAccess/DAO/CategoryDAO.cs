﻿using BusinessObject;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.DAO
{
    internal class CategoryDAO
    {
        internal static async Task<List<Category>> GetAll()
        {
            try
            {
                using var context = new TkdecorContext();
                var listCategories = await context.Categories
                    .OrderByDescending(x => x.UpdatedAt)
                    .ToListAsync();
                return listCategories;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        internal static async Task<Category?> FindById(long categoryId)
        {
            try
            {
                using var context = new TkdecorContext();
                var category = await context.Categories
                    .FirstOrDefaultAsync(x => x.CategoryId == categoryId);
                return category;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        internal static async Task<Category?> FindByName(string name)
        {
            try
            {
                using var context = new TkdecorContext();
                var category = await context.Categories
                    .FirstOrDefaultAsync(x => x.Name.Equals(name));
                return category;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        internal static async Task Add(Category category)
        {
            try
            {
                using var context = new TkdecorContext();
                await context.AddAsync(category);
                await context.SaveChangesAsync();
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        internal static async Task Update(Category category)
        {
            try
            {
                using var context = new TkdecorContext();
                context.Update(category);
                await context.SaveChangesAsync();
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        internal static async Task<bool> CheckProductExistsByCateId(long categoryId)
        {
            try
            {
                using var context = new TkdecorContext();
                var list = await context.Products
                    .Where(x => x.CategoryId == categoryId)
                    .ToListAsync();
                if (list.Count > 0) return true;

                return false;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
    }
}
