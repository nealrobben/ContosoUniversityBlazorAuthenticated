using Application.Common.Interfaces;
using Application.File.Commands;
using ContosoUniversityBlazor.WebUI.Controllers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebUI.Shared;

namespace WebUI.Server.Controllers;

public class FileController : ContosoApiController
{
    private readonly IWebHostEnvironment _env;
    private readonly IProfilePictureService _profilePictureService;

    public FileController(IWebHostEnvironment env, IProfilePictureService profilePictureService)
    {
        _env = env;
        _profilePictureService = profilePictureService;
    }

    [HttpPost]
    public async Task<ActionResult<IList<UploadResult>>> UploadFiles([FromForm] IEnumerable<IFormFile> files)
    {
        var resourcePath = new Uri($"{Request.Scheme}://{Request.Host}/");
        var uploadResult = await Mediator.Send(new UploadFileCommand(files.First()));

        return new CreatedResult(resourcePath, uploadResult);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetFile(string id)
    {
        var path = Path.Combine(_env.ContentRootPath, "Img", "ProfilePictures", id);
        var bytes = await _profilePictureService.GetImageFile(path);
        return File(bytes, "image/jpeg", Path.GetFileName(path));
    }
}
