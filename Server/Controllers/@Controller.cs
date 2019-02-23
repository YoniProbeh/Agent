using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Server.Resources.Application.Context;
using Server.Resources.Application.DTOs;
using Server.Resources.Shared.DTOs;
using Server.Resources.Shared.Exceptions;
using Server.Resources.Shared.Helpers;
using Server.Resources.Shared.Models;
using Server.Resources.Shared.Services;

namespace Server.Controllers
{
    public interface IController<TRequest> where TRequest : BaseDTO, new()
    {
        Task<IActionResult> CreateModel([FromBody] TRequest dto);
        Task<IActionResult> DeleteModel([FromRoute] string id);
        Task<IActionResult> ReadModels([FromQuery] FilterParams query);
        Task<IActionResult> ReadModel([FromRoute] string id);
        Task<IActionResult> UpdateModel([FromBody] TRequest dto);
    }

    [ApiController]
    // [Authorize]
    [Route("api/[controller]")]
    public class BaseController<TDomain, TRequest, TResponse, TRepository> : ControllerBase
    where TDomain : BaseModel<TDomain>, new() where TRequest : BaseDTO, new() where TResponse : BaseResponseDTO<TResponse>, new() where TRepository : IModelRepository<TDomain>
    {
        internal readonly TRepository modelRepo;

        public BaseController(TRepository modelRepo)
        {
            this.modelRepo = modelRepo;
        }

        [HttpPost] public virtual async Task<IActionResult> CreateModel([FromBody] TRequest dto)
        {
            return await this.Process(
                function: async() =>
                {
                    TResponse result = await this.modelRepo.CreateModel<TRequest, TResponse>(dto);
                    return result;
                });
        }

        [HttpDelete("{id}")] public virtual async Task<IActionResult> DeleteModel([FromRoute] string id)
        {
            return await this.Process(
                function: async() =>
                {
                    TResponse result = await this.modelRepo.DeleteModel<TResponse>(id);
                    return result;
                });
        }

        [HttpGet] public virtual async Task<IActionResult> ReadEntries([FromQuery] FilterParams filters)
        {
            return await this.Process(
                function: async() =>
                {
                    PagedList<TResponse> result = await this.modelRepo.ReadModels<TResponse>(filters);
                    this.Response.AddPagination(result);
                    return result;
                });
        }

        [HttpGet("{id}")] public virtual async Task<IActionResult> ReadModel([FromRoute] string id)
        {
            return await this.Process(
                function: async() =>
                {
                    TResponse result = await this.modelRepo.ReadModel<TResponse>(id);
                    return result;
                });
        }

        [HttpPut] public virtual async Task<IActionResult> UpdateModel([FromBody] TRequest dto)
        {
            return await this.Process(
                function: async() =>
                {
                    TResponse result = await this.modelRepo.UpdateModel<TRequest, TResponse>(dto);
                    return result;
                });
        }

        // [HttpPost("Image")] public virtual async Task<IActionResult> UploadImage(IFormFile file)
        // {
        //     #region Validations
        //     var MAX_LENGTH = 1024;
        //     var ACCEPTED_FILE_TYPES = new [] { ".jpg", ".bmp", ".jpeg", ".ico", ".png" };
        //     if (file.Length == 0) { return BadRequest("Empty file"); }
        //     if (file.Length > MAX_LENGTH) { return BadRequest($"The file is greater than {MAX_LENGTH / 1000}MB"); }
        //     if (!ACCEPTED_FILE_TYPES.Any(x => x == Path.GetExtension(file.FileName).ToLower())) { return BadRequest($"Invalid image format"); }
        //     #endregion // Validations

        //     string uploadPath = Path.Combine(this.host.WebRootPath, "images");
        //     if (!Directory.Exists(uploadPath))
        //     {
        //         Directory.CreateDirectory(uploadPath);
        //     }
        //     Image image = new Image();
        //     {
        //         image.FileName = image.ID.ToString() + Path.GetExtension(file.FileName).ToLower();
        //     };
        //     using(FileStream stream = new FileStream(Path.Combine(uploadPath, image.FileName), FileMode.Create))
        //     {
        //         await file.CopyToAsync(stream);
        //     }
        //     EntityEntry<Image> result = await this.context.Images.AddAsync(image);
        //     await this.context.SaveChangesAsync();

        //     return Ok(result.Entity);
        // }
        
        internal async Task<IActionResult> Process<T>([Optional] Func<Task<T>> function)
        {
            try
            {

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                T result = await function();
                return Ok(result);
            }
            catch (BadRequestException ex) { return StatusCode(400, ex.Message); }
            catch (NoContentException ex) { return StatusCode(204, ex.Message); }
            catch (ProcessFailedException ex) { return StatusCode(500, ex.Message); }
            catch (Exception ex) { return StatusCode(500, ex.Message); }
        }

        internal int UserID()
        {
            return int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
        }

    }
}