using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
    private readonly UserManager<User> _userManager;

    public CategoryController(ILogger<CategoryController> logger, ICategoryRepository categoryRepository, IConfiguration configuration, UserManager<User> userManager)
    {
        _logger = logger;
        _categoryRepository = categoryRepository;
        _configuration = configuration;
        _userManager = userManager;
    }

    [HttpGet("GetAll/{userName}")]
    [Authorize(Policy = "RequiredUserOrAdminRole")]
    public async Task<ActionResult<IEnumerable<Category>>> GetAllByUser([FromRoute]string userName)
    {
        try
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == userName);
            if (user == null)
            {
                return BadRequest("User not found.");
            }
            
            var categories = await _categoryRepository.GetCategoriesByUser(user.Id);
            return Ok(categories);
        }
        catch (Exception e)
        {
            const string message = "Error getting categories";
            _logger.LogError(e, message);
            return NotFound(message);
        }
    }
    
    [HttpPost("Add")]
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
            var isAdmin = User.IsInRole(adminRole);

            var category = await _categoryRepository.GetByIdAsync(catId);
            if (category == null)
            {
                return NotFound("Category not found.");
            }

            if (category.OwnerId != userId && !isAdmin)
            {
                return Unauthorized("You do not have access.");
            }

            await _categoryRepository.DeleteCategory(catId);
            return NoContent();
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