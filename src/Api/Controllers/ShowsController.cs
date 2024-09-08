using Api.Models;
using Domain.Models;
using Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ShowsController(IShowService showService) : Controller
    {
        private readonly IShowService _showService = showService;

        private const string _serverErrorMessage = "Something went wrong in the server, please retry. if this keeps appearing contact the administrator";

        [HttpGet]
        public Task<List<Show>> Index() => _showService.GetAsync();

        [HttpGet("{name}")]
        public async Task<ActionResult<Show>> Details(string? name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return NotFound();

            try
            {
                var show = await _showService.GetByNameAsync(name);
                if (show is null)
                    return NotFound();

                return show;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, _serverErrorMessage);
            }
        }

        // POST: Shows/Create
        [HttpPost("create")]
        public async Task<IActionResult> Create(CreateUpdateShow show)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _showService.CreateAsync((Show)show);
                    return Created();
                }
                catch (Exception)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, _serverErrorMessage);
                }
            }

            return BadRequest(ModelState);
        }

        // POST: Shows/Edit/5
        [HttpPost("update/{id}")]
        public async Task<IActionResult> Update(int id, CreateUpdateShow show)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _showService.UpdateAsync(id, (Show)show);
                    return Ok();
                }
                catch (Exception)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, _serverErrorMessage);
                }
            }

            return BadRequest(ModelState);
        }

        // POST: Shows/Delete/5
        [HttpPost("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _showService.DeleteAsync(id);
                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, _serverErrorMessage);
            }
        }
    }
}
