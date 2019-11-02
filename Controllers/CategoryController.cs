using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MiniEshop.DAL;
using MiniEshop.Domain;
using MiniEshop.Domain.DTO;

namespace MiniEshop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController
        : ControllerBase
    {
        private readonly IEshopRepository _repository;

        public CategoryController(IEshopRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<List<CategoryDTO>> GetAsync()
        {
            (List<Category> categories, Category root) categories = await _repository.GetCategoriesAsync();
            var parent = new CategoryDTO()
            {
                Id = categories.root.Id,
                Name = categories.root.Name
            };
            return ReadChildCategories(parent, categories.categories);
        }

        private List<CategoryDTO> ReadChildCategories(CategoryDTO parent, List<Category> categories)
        {
            if (parent == null)
                return new List<CategoryDTO>();

            List<CategoryDTO> childsRes = categories.Where(g => g.ParentId == parent.Id).Select(g => new CategoryDTO()
            {
                Id = g.Id,
                Name = g.Name
            }).ToList();
            foreach(var child in childsRes)
            {
                child.Children = ReadChildCategories(child, categories);
            }
            return childsRes;
        }
    }
}