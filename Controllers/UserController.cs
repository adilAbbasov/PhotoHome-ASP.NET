using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PhotoHome.Data;
using PhotoHome.Models;
using PhotoHome.Models.Entity;
using PhotoHome.Models.ViewModels;
using System.Diagnostics;
using System.Security.Claims;
using EASendMail;
using SmtpClient = EASendMail.SmtpClient;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;

namespace PhotoHome.Controllers
{
	public class UserController : Controller
	{
		private readonly AppDbContext _base;
		private readonly UserManager<User> userManager;
		private readonly SignInManager<User> signInManager;
		private readonly RoleManager<IdentityRole> rolemanager;

		public UserController(UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<IdentityRole> rolemanager, AppDbContext context)
		{
			_base = context;
			this.userManager = userManager;
			this.signInManager = signInManager;
			this.rolemanager = rolemanager;
		}


		//[Route("signin-google")]

		public async Task Google_Login()
		{
			await HttpContext.ChallengeAsync(GoogleDefaults.AuthenticationScheme, new AuthenticationProperties()
			{
				RedirectUri = Url.Action("GoogleResponse")
			});
		}


		//[Route("google-response")]

		public async Task<IActionResult> GoogleResponse()
		{
			var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

			if (result == null || result.Principal == null || result.Principal.Identities == null || !result.Principal.Identities.Any())
				return BadRequest("Unable to retrieve claims");

			var claims = result.Principal.Identities.First().Claims.Select(claim => new
			{
				claim.Issuer,
				claim.OriginalIssuer,
				claim.Type,
				claim.Value
			});

			return Json(claims);
		}


		//Signup

		public IActionResult SignUp(string? errorMessage)
		{
			ViewBag.ErrorMessage = errorMessage;

			return View();
		}


		[HttpPost]
		public async Task<IActionResult> SignUp(SignupViewModel usersdata)

		{
			if (ModelState.IsValid)
			{
				string[] option = usersdata.Email.Split("@");

				var user = new User
				{
					Email = usersdata.Email,
					FirstName = usersdata.FirstName,
					LastName = usersdata.LastName,
					UserName = option[0],
				};

				if (!await rolemanager.RoleExistsAsync("Client"))
				{
					await rolemanager.CreateAsync(new IdentityRole { Name = "Client" });
				}

				var result = await userManager.CreateAsync(user, usersdata.Password);
				var boss = await userManager.AddToRoleAsync(user, "Client");

				if (result.Succeeded)
				{
					await signInManager.SignInAsync(user, true);
					var count = 0;

					try
					{
						var task = Task.Run(async delegate
						{
							try
							{
								await Task.Delay(1000);

								var oMail = new SmtpMail("TryIt")
								{
									From = "photohome2023@gmail.com",
									To = usersdata.Email,
									Subject = $"Thank you '{usersdata.FirstName}' for filling out our form! " + "\nPhotoHome",
									HtmlBody = "<html><body><img src='https://i.pinimg.com/564x/85/ac/9d/85ac9d284995da771bd36153a7c84107.jpg' /></body></html>"
								};

								var oServer = new SmtpServer("smtp.outlook.com")
								{
									Port = 587,
									User = "photohome2023@gmail.com",
									Password = "Photo_Home_2023",
									ConnectType = SmtpConnectType.ConnectTryTLS
								};

								var oSmtp = new SmtpClient();
								await oSmtp.SendMailAsync(oServer, oMail);
							}
							catch (Exception)
							{
								count++;
								throw;
							}
						});
						task.Wait();
					}
					catch (Exception)
					{
						return RedirectToAction("SignUp", "User", new { errorMessage = "Email is not existing, write an existing email." });
					}

					return RedirectToAction("Index", "Home");
				}
				else
				{
					foreach (var item in result.Errors)
						ModelState.AddModelError(item.Code, item.Description);

					return RedirectToAction("SignUp", "User", new { errorMessage = "Account with this email already exists." });
				}
			}

			return View();
		}


		//Login

		public async Task<IActionResult> LogIn(string? returnUrl, string? errorMessage)
		{
			if (returnUrl == "/admin")
				return RedirectToAction("Index", "AdminLogin", new { returnUrl = returnUrl, errorMessage = errorMessage });

			ViewBag.ReturnUrl = returnUrl;
			ViewBag.ErrorMessage = errorMessage;

			return await Task.FromResult(View());
		}


		[HttpPost]
		public async Task<IActionResult> Login(LoginViewModel viewModel, string? returnUrl)
		{
			if (ModelState.IsValid)
			{
				var user = await userManager.FindByEmailAsync(viewModel.Email);

				if (user != null)
				{
					if (await userManager.CheckPasswordAsync(user, viewModel.Password))
					{
						await signInManager.SignInAsync(user, true);

						if (!string.IsNullOrWhiteSpace(returnUrl))
							return Redirect(returnUrl);

						return RedirectToAction("Index", "Home");
					}
					else
						return RedirectToAction("Login", "User", new { returnUrl = returnUrl, errorMessage = "Incorrect email or password. Please try again." });
				}
				else
					return RedirectToAction("Login", "User", new { returnUrl = returnUrl, errorMessage = "Sorry, we couldn't find an account with that email address." });
			}

			return View();
		}


		public IActionResult LogOut()
		{
			signInManager.SignOutAsync();

			return RedirectToAction("Index", "Home");
		}


		public async Task<IActionResult> Created()
		{
			var userId = GetUserId();
			var user = await _base.Users.FirstOrDefaultAsync(a => a.Id == userId);

			var model = await _base.Images.Where(a => a.Allow == true && a.UserId == userId).ToListAsync();

			ViewBag.Images = model;

			return View(user);
		}


		[HttpGet]
		[AllowAnonymous]
		public IActionResult AccessDenied(string? returnUrl)
		{
			return RedirectToAction("LogIn", "User", returnUrl);
		}


		public async Task<IActionResult> Liked()
		{
			var userId = GetUserId();
			var user = await _base.Users.FirstOrDefaultAsync(a => a.Id == userId);

			var imageLikes = await _base.ImageLikes.Include(a => a.User).Where(a => a.UserId == userId).ToListAsync();
			var imageList = new List<Picture>();

			foreach (var item in imageLikes)
			{
				imageList.Add(await _base.Images.FirstOrDefaultAsync(a => a.Id == item.ImageId));
			}

			ViewBag.Images = imageList;

			return View(user);
		}


		public async Task<IActionResult> Settings()
		{
			var userId = GetUserId();
			var user = await _base.Users.FirstOrDefaultAsync(a => a.Id == userId);

			return View(user);
		}


		[HttpPost]
		public async Task<IActionResult> Save(User userdata)
		{
			var userId = GetUserId();
			var user = await _base.Users.FirstOrDefaultAsync(a => a.Id == userId);

			user.FirstName = userdata.FirstName;
			user.LastName = userdata.LastName;
			user.UserName = userdata.UserName;
			user.Email = userdata.Email;

			await _base.SaveChangesAsync();

			return RedirectToAction("Settings");
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