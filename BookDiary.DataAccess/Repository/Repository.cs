﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using BookDiary.Models;
using Microsoft.EntityFrameworkCore;

namespace BookDiary.DataAccess.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        internal DbSet<T> dbSet;
        public Repository(ApplicationDbContext context)
        {
             this._context = context;
             this.dbSet = _context.Set<T>();

        }
        public async Task Add(T entity)
        {
            await dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
         
            var entity = dbSet.Find(id);
            if (entity == null)
            {
                throw new ArgumentException("Id is null!");
            }
            dbSet.Remove(entity);
            await this._context.SaveChangesAsync();
        }

        public async Task DeleteMapping<TMapping>(Expression<Func<TMapping, bool>> predicate) where TMapping : class
        {
            var entity = _context.Set<TMapping>().FirstOrDefault(predicate);
            if (entity != null)
            {
                _context.Set<TMapping>().Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteUser(string id)
        {
            var entity = dbSet.Find(id);
            if (entity == null)
            {
                throw new ArgumentException("Id is null!");
            }
            dbSet.Remove(entity);
            await this._context.SaveChangesAsync();
        }

        public async Task<List<T>> Find(Expression<Func<T, bool>> filter)
        {
             return await dbSet.Where(filter).ToListAsync();
        }

        public async Task<T> Get(int id)
        {
            var entity = dbSet.Find(id);
            if (entity == null)
            {
                throw new ArgumentException("id is null");
            }
            return entity;
        }

        public async Task<T> Get(Expression<Func<T, bool>> filter)
        {
            return await dbSet.FirstOrDefaultAsync(filter);
        }

        public  IQueryable<T> GetAll()
        {
            return  dbSet.AsQueryable();
        }
        public async Task<T> GetById(int id)
        {
            var entity =  await dbSet.FindAsync(id);
            if (entity == null)
            {
                throw new ArgumentException("id is null");
            }
            return entity;
        }

        public async Task<T> GetByIdUser(string id)
        {
            return await dbSet.FindAsync(id);
        }

        public async Task Update(T entity)
        {
            dbSet.Update(entity);
            await _context.SaveChangesAsync();

        }
        
    }
}
