using DoctorLoan.Products.Application.Features.Categories.Admin.Commands;
using DoctorLoan.Products.Application.Features.Categories.Admin.Queries;
using Microsoft.AspNetCore.Mvc;

namespace DoctorLoan_Products.WebUI.Areas.Controllers;
public class CategoryController : ApiControllerBase
{
    [HttpGet]
    public async Task<IActionResult> FilterCategories([FromQuery] FilterCategoryQuery query)
    {
        return Ok(await Mediator.Send(query));
    }
    [HttpGet]
    public async Task<IActionResult> GetCategory(int id)
    {
        return Ok(await Mediator.Send(new GetCategoryByIdQuery(id)));
    }
    [HttpPost]
    public async Task<IActionResult> SaveCategory([FromBody] SaveCategoryCommand command)
    {
        return Ok(await Mediator.Send(command));
    }
    [HttpPut]
    public async Task<IActionResult> UpdateCategoryStatus([FromBody] UpdateCategoryStatusCommand command)
    {
        return Ok(await Mediator.Send(command));
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCategory([FromRoute] int id)
    {
        return Ok(await Mediator.Send(new DeleteCategoryCommand { Id = id }));
    }
}
