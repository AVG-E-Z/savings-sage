using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using savings_sage.Model;
using savings_sage.Service.Repositories;

namespace savings_sage.Controller;

[ApiController]
[Route("api/[controller]")]
public class CategoryController : ControllerBase
{
    private readonly ILogger<CategoryController> _logger;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IConfiguration _configuration;

    public CategoryController(ILogger<CategoryController> logger, ICategoryRepository categoryRepository, IConfiguration configuration)
    {
        _logger = logger;
        _categoryRepository = categoryRepository;
        _configuration = configuration;
    }

    [HttpGet("all")]
    [Authorize(Policy = "RequiredUserOrAdminRole")]
    public async Task<ActionResult<IEnumerable<Category>>> GetAllByUser([FromQuery]string userId)
    {
        try
        {
            var categories = await _categoryRepository.GetCategoriesByUser(userId);
            return Ok(categories);
        }
        catch (Exception e)
        {
            const string message = "Error getting categories";
            _logger.LogError(e, message);
            return NotFound(message);
        }
    }
    
    [HttpPost("new")]
    [Authorize(Policy = "RequiredUserOrAdminRole")]
    public async Task<ActionResult<Category>> CreateNewCategory(
        [FromQuery]string userId,
        [FromQuery]string categoryName,
        [FromQuery]int colorId
        )
    {
        try
        {
            var newCategory = await _categoryRepository.NewCategoryAsync(userId, categoryName, colorId);
            return Ok(newCategory);
        }
        catch (Exception e)
        {
            const string message = "Error creating category";
            _logger.LogError(e, message);
            return StatusCode(StatusCodes.Status500InternalServerError, message);
        }
    }
    
    [HttpDelete("Delete/{catId:int}")]
    [Authorize(Policy = "RequiredUserOrAdminRole")]
    public async Task<ActionResult> DeleteCategory(
        [FromRoute]int catId,
        [FromQuery]string userId)
    {
        try
        {
            var adminRole = _configuration["Roles:Admin"];
            var category = await _categoryRepository.GetByIdAsync(catId);
            if (category.OwnerId != userId || !User.IsInRole(adminRole))
            {
                return Unauthorized("You do not have access.");
            }
            var result = _categoryRepository.DeleteCategory(catId);
            return Ok(result);
        }
        catch (Exception e)
        {
            const string message = "Error deleting category";
            _logger.LogError(e, message);
            return StatusCode(StatusCodes.Status500InternalServerError, message);
        }
    }
    
    [HttpPut("Update/{catId:int}")]
    [Authorize(Policy = "RequiredUserOrAdminRole")]
    public async Task<ActionResult> UpdateCategory(
        [FromRoute]int catId,
        [FromQuery]string userId,
        [FromQuery]string categoryName,
        [FromQuery]int colorId)
    {
        try
        {
            var adminRole = _configuration["Roles:Admin"];
            var category = await _categoryRepository.GetByIdAsync(catId);
            if (category.OwnerId != userId || !User.IsInRole(adminRole))
            {
                return Unauthorized("You do not have access.");
            }
            
            var result = _categoryRepository.UpdateCategory(catId, categoryName, colorId);
            return Ok(result);
        }
        catch (Exception e)
        {
            const string message = "Error updating category";
            _logger.LogError(e, message);
            return StatusCode(StatusCodes.Status500InternalServerError, message);
        }
    }
}