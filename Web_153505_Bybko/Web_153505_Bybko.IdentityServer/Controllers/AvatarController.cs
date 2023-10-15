using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Web_153505_Bybko.IdentityServer.Data.Migrations;
using Web_153505_Bybko.IdentityServer.Models;

namespace Web_153505_Bybko.IdentityServer.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class AvatarController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _environment;
        private readonly string _defaultAvatar;

        public AvatarController(UserManager<ApplicationUser> userManager,
                                IWebHostEnvironment environment) 
        {
            _environment = environment;
            _userManager = userManager;
            _defaultAvatar = "default.png";
        }

        [HttpGet]
        public IActionResult GetAvatar()
        {
            var id = _userManager.GetUserId(User);
            if (id is null)
                return BadRequest("User not found");

            string searchPattern = $@"{id}.*";
            var files = Directory.GetFiles(Path.Combine(_environment.WebRootPath, "Images"), searchPattern);

            string imagePath;
            if (files.Any())
            {
                imagePath = files[0];
            }
            else
            {
                imagePath = Path.Combine(_environment.WebRootPath, "Images", _defaultAvatar);
            }

            FileStream fs = new(imagePath, FileMode.Open);
            string ext = Path.GetExtension(imagePath);

            var extProvider = new FileExtensionContentTypeProvider();

            return File(fs, extProvider.Mappings[ext]);
        }
    }
}
