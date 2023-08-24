using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PhotoHome.Data;
using PhotoHome.Models.Entity;

namespace PhotoHome.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles = "Admin")]
	public class CategoryController : Controller
	{
		private readonly AppDbContext _base;
		public CategoryController(AppDbContext context)
		{
			_base = context;
		}


		public async Task<IActionResult> Index()
		{
			return View(await _base.Categories.ToListAsync());
		}


		public IActionResult AddCategory()
		{
			return View();
		}


		[HttpPost]
		public async Task<IActionResult> AddCatagory(Category catalogue)
		{
			if (!ModelState.IsValid)
			{
				var category = new Category()
				{
					Name = catalogue.Name
				};

				await _base.Categories.AddAsync(category);
				await _base.SaveChangesAsync();

				return RedirectToAction("Index");
			}

			return View();
		}


		[HttpGet]
		public async Task<IActionResult> UpdateCategory(int Id)
		{
			var category = await _base.Categories.FirstOrDefaultAsync(c => c.Id == Id);

			return View(category);
		}


		[HttpPost]
		public async Task<IActionResult> UpdateCategory(Category model)
		{
			if (model.Name != null)
			{
				var category = await _base.Categories.FirstOrDefaultAsync(c => c.Id == model.Id);
				category.Name = model.Name;

				await _base.SaveChangesAsync();
			}

			return RedirectToAction("Index");
		}


		public async Task<IActionResult> DeleteCategory(int Id)
		{
			var category = await _base.Categories.FirstOrDefaultAsync(c => c.Id == Id);

			_base.Categories.Remove(category);
			await _base.SaveChangesAsync();

			return RedirectToAction("Index");
		}
	}
}
