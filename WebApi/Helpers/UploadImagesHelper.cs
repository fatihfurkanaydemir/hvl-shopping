using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Application.Exceptions;
using Domain.Entities;
using Application.Features.SharedViewModels;
using Application.Wrappers;

namespace WebApi.Helpers;

public class UploadImagesHelper
{
  public static async Task<Response<IList<ImageViewModel>>> UploadImages(HttpRequest request)
  {
    
    var formCollection = await request.ReadFormAsync();

    var folderName = Path.Combine("Resources", "Images");
    var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
    if(!Directory.Exists(pathToSave))
      Directory.CreateDirectory(pathToSave);

    var imagesToAdd = new List<ImageViewModel>();

    foreach (var file in formCollection.Files)
    {
      if (file.Length > 0)
      {
        var fileExtension = file.ContentType.Split("/")[1];

        var fileName = Guid.NewGuid().ToString() + "." + fileExtension;
        var fullPath = Path.Combine(pathToSave, fileName);
        var url = Path.Combine(folderName, fileName);
        using (var stream = new FileStream(fullPath, FileMode.Create))
        {
          file.CopyTo(stream);
        }

        imagesToAdd.Add(new ImageViewModel { Url = url });
      }
      else
      {
        throw new ApiException("A problem occured with one of files");
      }
    }

    return new Response<IList<ImageViewModel>>
    {
      Data = imagesToAdd,
      Succeeded = true
    };
  }
}
