using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PhotoHome.Data;
using PhotoHome.Models.Entity;

namespace PhotoHome.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles = "Admin")]
	public class NotificationController : Controller
	{
		private readonly AppDbContext _base;

		public NotificationController(AppDbContext context)
		{
			_base = context;
		}


		public async Task<IActionResult> Index()
		{
			return View(await _base.Images.Include(a => a.User).Where(a => a.Allow == false).ToListAsync());
		}


		public async Task<IActionResult> Allow(Picture model)
		{
			if (!ModelState.IsValid)
			{
				var picture = await _base.Images.FirstOrDefaultAsync(a => a.Id == model.Id);
				picture.Allow = true;

				await _base.SaveChangesAsync();
			}

			return RedirectToAction("Index");
		}


		public async Task<IActionResult> NotAllow(Picture model)
		{
			if (!ModelState.IsValid)
			{
				var picture = await _base.Images.FirstOrDefaultAsync(a => a.Id == model.Id);
				picture.Allow = false;

				_base.Images.Remove(picture);
				await _base.SaveChangesAsync();
			}

			return RedirectToAction("Index");
		}


		public async Task<IActionResult> DetailedInfo(int id)
		{
			var model = await _base.Images.Include(a => a.User).Include(a => a.Category).Include(a => a.ImageTags).FirstOrDefaultAsync(i => i.Id == id);

			ViewBag.Tags = await _base.ImageTags.Include(it => it.Image).Where(it => it.ImageId == id).Select(it => it.Tag).Select(t => t.Name).ToListAsync();

			return View(model);
		}
	}
}
