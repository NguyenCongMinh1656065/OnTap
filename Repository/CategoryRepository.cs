﻿using AutoMapper;
using Ôn_tập.Data;
using Ôn_tập.Interfaces;
using Ôn_tập.Models;

namespace Ôn_tập.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private DataContext _context;
        private IMapper _mapper;

        public CategoryRepository(DataContext context, IMapper mapper) 
        {
            _context = context;
            _mapper = mapper;
        }
        public bool CategoryExists(int id)
        {
            return _context.Categories.Any(c => c.Id == id);
        }

        public bool CreateCategory(Category category)
        {
            _context.Add(category);
            return Save();
        }

        public bool DeleteCategory(Category category)
        {
            _context.Remove(category);
            return Save();
        }

        public ICollection<Category> GetCategories()
        {
            return _context.Categories.ToList();
        }

        public Category GetCategory(int id)
        {
            return _context.Categories.Where(c =>  c.Id == id).FirstOrDefault();
        }


        public ICollection<Pokemon> GetPokemonByCategory(int categoryId)
        {
            return _context.PokemonCategories.Where(c => c.CategoryId == categoryId).Select(p => p.Pokemon).ToList();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateCategory(Category category)
        {
            _context.Update(category);
                return Save();
        }
    }
}
