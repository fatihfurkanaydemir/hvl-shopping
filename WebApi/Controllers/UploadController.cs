using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Common.EventBus.Interfaces;
using WebApi.Helpers;
using Application.Wrappers;

namespace WebApi.Controllers.v1
{
  public class UploadController : BaseApiController
  {
    private readonly IEventBus _eventBus;
    public UploadController(IEventBus eventBus)
    {
      _eventBus = eventBus;
    }

    // POST api/<controller>/UploadImage
    [Authorize]
    [HttpPost("UploadImage")]
    public async Task<IActionResult> UploadImage(IFormFile[] files)
    {
      return Ok(await UploadImagesHelper.UploadImages(Request));
    }
  }
}
