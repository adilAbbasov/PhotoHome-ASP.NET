using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PhotoHome.Data;
using PhotoHome.Helpers;
using PhotoHome.Models;
using PhotoHome.Models.Entity;
using PhotoHome.Models.ViewModels;
using PhotoHome.Repository;
using System.Diagnostics;
using System.Security.Claims;

namespace PhotoHome.Controllers
{
	[Authorize(Roles = "Client")]

	public class HomeController : Controller
	{
		private readonly AppDbContext _base;
		public static Cloudinary cloudinary;
		public const string CLOUD_NAME = "dhtcecrpa";
		public const string API_KEY = "621668164995499";
		public const string API_SECRET = "iLcKxUn6rR_cq9qWiTOV8e9H2VY";
		private ImageRepository imageRepository;
		private readonly UserManager<User> userManager;
		private const int pageSize = 15;
		private static List<string> image_tags;
		public const int ImageMinimumBytes = 512;

		public List<string> Image_Tags
		{
			get { return image_tags; }
			set { image_tags = value; }
		}


		public HomeController(AppDbContext context, UserManager<User> userManager)
		{
			_base = context;
			this.userManager = userManager;
			Image_Tags = new List<string>();
		}


		[HttpGet]
		[AllowAnonymous]
		public async Task<ActionResult> Index()
		{
			imageRepository = new ImageRepository(_base);
			var model = await imageRepository.GetImage(0, null, null);

			var userId = GetUserId();

			if (userId != null)
				ViewBag.User = await _base.Users.FirstOrDefaultAsync(a => a.Id == userId);

			ViewBag.Categories = await _base.Categories.ToListAsync();
			ViewBag.Image_Likes = await _base.ImageLikes.ToListAsync();
			ViewBag.ActiveMenu = "index";

			return View(model);
		}


		[HttpGet]
		[AllowAnonymous]
		public async Task<ActionResult> ImageListAsync(int? pageNumber, string? searchPattern, string? searchType)
		{
			imageRepository = new ImageRepository(_base);
			var model = await imageRepository.GetImage(pageNumber, searchPattern, searchType);

			return PartialView("ImageList", model);
		}


		[AllowAnonymous]
		public async Task<IActionResult> Search(string searchPattern, string searchType)
		{
			imageRepository = new ImageRepository(_base);
			var model = await imageRepository.GetImage(0, searchPattern, searchType);

			var userId = GetUserId();

			if (userId != null)
				ViewBag.User = await _base.Users.FirstOrDefaultAsync(a => a.Id == userId);

			ViewBag.Categories = await _base.Categories.ToListAsync();
			ViewBag.ActiveMenu = "index";

			return View("Index", model);
		}


		[AllowAnonymous]
		public IActionResult About()
		{
			ViewBag.ActiveMenu = "about";

			return View();
		}


		[AllowAnonymous]
		public IActionResult Contact()
		{
			ViewBag.ActiveMenu = "contact";

			return View();
		}


		[AllowAnonymous]
		public IActionResult Services()
		{
			ViewBag.ActiveMenu = "services";

			return View();
		}


		[HttpGet]
		[AllowAnonymous]
		public async Task<IActionResult> ImageInfo(string Id)
		{
			if (Id.Contains("%2F")) Id = Id.Replace("%2F", "/");

			var model = await _base.Images.Include(n => n.User).Include(n => n.Category).FirstOrDefaultAsync(n => n.ImageUrl == Id);
			var id = model.Id;

			var relatedImages = await _base.Images.Include(i => i.Category).Where(i => i.CategoryId == model.CategoryId && i.Id != id).ToListAsync();

			var imageTags = await _base.ImageTags.Where(a => a.ImageId == id).ToListAsync();
			var tags = new List<Tag>();

			foreach (var item in imageTags)
			{
				tags.Add(await _base.Tags.FirstOrDefaultAsync(a => a.Id == item.TagId));
			}

			ViewBag.Tags = (tags);
			ViewBag.RelatedImages = relatedImages;

			return View(model);
		}


		[HttpPost]
		[AllowAnonymous]
		public async Task<IActionResult> DownloadImage(string link)
		{
			var picture = await _base.Images.FirstOrDefaultAsync(a => a.ImageUrl == link);
			picture.DownloadCount++;

			await _base.SaveChangesAsync();

			return RedirectToAction("Index");
		}


		[HttpPost]
		[AllowAnonymous]
		public async Task<IActionResult> LikeImage(string link)
		{
			var userId = GetUserId();

			if (userId == null)
				return RedirectToAction("LogIn", "User");

			var user = await _base.Users.FirstOrDefaultAsync(a => a.Id == userId);
			var picture = await _base.Images.FirstOrDefaultAsync(a => a.ImageUrl == link);

			var imageLike = new ImageLike { ImageId = picture.Id, UserId = user.Id };

			if (!_base.ImageLikes.Contains(imageLike))
			{
				await _base.ImageLikes.AddAsync(imageLike);
				picture.LikeCount++;
			}
			else
			{
				_base.ImageLikes.Remove(imageLike);
				picture.LikeCount--;
			}

			await _base.SaveChangesAsync();

			return RedirectToAction("Index");
		}


		[HttpPost]
		[AllowAnonymous]
		public List<string> GetTag(string searchTerm)
		{
			var model = _base.Tags.Where(a => a.Name.ToLower().StartsWith(searchTerm)).Select(a => a.Name).ToList();

			return model;
		}


		[HttpPost]
		[AllowAnonymous]
		public async Task<int> AddTag(List<string> list)
		{
			await Task.Run(async () => {
				await Task.Delay(1000);

				Image_Tags = new List<string>();

				foreach (var item in list)
				{
					Image_Tags.Add(item);
				}
			});

			return 200;
		}


		[HttpPost]
		[AllowAnonymous]
		public async Task<IActionResult> DeleteImage(string link)
		{
			var userId = GetUserId();

			if (userId == null)
				return RedirectToAction("LogIn", "User");

			var picture = await _base.Images.FirstOrDefaultAsync(a => a.ImageUrl == link);

			_base.Images.Remove(picture);
			await _base.SaveChangesAsync();

			return RedirectToAction("Created");
		}


		public IActionResult Create()
		{
			ViewBag.Categories = new SelectList(_base.Categories, "Id", "Name");
			ViewBag.ActiveMenu = "create";

			return View();
		}


		public static string fileName { get; set; }

		[HttpPost]
		[AllowAnonymous]
		public async Task<int> GetFileName(IFormFile file)
		{
			fileName = await UploadFileHelper.UploadFile(file);

			return 200;
		}


		public async Task<IActionResult> AddImage(AddImageViewModel viewModel)
		{
			var account = new Account(CLOUD_NAME, API_KEY, API_SECRET);

			cloudinary = new Cloudinary(account);

			var imagePath = fileName;

			await UploadImage(imagePath, viewModel);

			return RedirectToAction("Index");
		}


		public async Task<int> UploadImage(string imagePath, AddImageViewModel viewModel)
		{
			try
			{
				var uploadParams = new ImageUploadParams()
				{
					File = new FileDescription(imagePath)
				};

				var uploadResult = cloudinary.Upload(uploadParams);

				var userId = GetUserId();

				var picture = new Picture()
				{
					Title = viewModel.Title,
					Description = viewModel.Description,
					CategoryId = viewModel.categoryId,
					LikeCount = viewModel.LikeCount,
					DownloadCount = viewModel.DownloadCount,
					UserId = userId,
					ImageUrl = uploadResult?.SecureUri.ToString(),
					Allow = false
				};

				await _base.Images.AddAsync(picture);
				await _base.SaveChangesAsync();

				foreach (var item in Image_Tags)
				{
					if (await _base.Tags.FirstOrDefaultAsync(a => a.Name == item) == null)
					{
						await _base.Tags.AddAsync(new Tag { Name = item });
					}

					await _base.SaveChangesAsync();

					var tag = await _base.Tags.FirstOrDefaultAsync(a => a.Name == item);
					var image = await _base.Images.FirstOrDefaultAsync(a => a.ImageUrl == picture.ImageUrl);

					await _base.ImageTags.AddAsync(new ImageTag { ImageId = image.Id, TagId = tag.Id });
				}

				await _base.SaveChangesAsync();
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}

			return 200;
		}


		public string GetUserId()
		{
			var claim = (ClaimsIdentity)User.Identity;
			var claims = claim.FindFirst(ClaimTypes.NameIdentifier);

			if (claims == null)
				return null;

			return claims.Value;
		}


		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}