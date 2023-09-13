using DoctorLoan.News.Application.Features.NewsCategories.Admin.Commands;
using DoctorLoan.News.Application.Features.NewsCategories.Admin.Commands.SaveCategory;
using DoctorLoan.News.Application.Features.NewsCategories.Admin.Queries;
using Microsoft.AspNetCore.Mvc;

namespace DoctorLoan.News.WebUI.Areas.Controllers;
public class NewsCategoryController : ApiControllerBase
{
    [HttpGet]
    public async Task<IActionResult> FilterCategories([FromQuery] GetNewsCategoriesQuery query)
    {
        return Ok(await Mediator.Send(query));
    }
    [HttpGet]
    public async Task<IActionResult> GetNewsCategoryId(int id)
    {
        return Ok(await Mediator.Send(new GetNewsCategoryByIdQuery(id)));
    }
    [HttpPost]
    public async Task<IActionResult> SaveCategory([FromBody]SaveNewsCategoryCommand command)
    {
        return Ok(await Mediator.Send(command));
    }
    [HttpPut]
    public async Task<IActionResult> UpdateCategoryStatus([FromBody] UpdateCategoryNewsStatusCommand command)
    {
        return Ok(await Mediator.Send(command));
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCategory([FromRoute] int id)
    {
        return Ok(await Mediator.Send(new DeleteCategoryNewsCommand { Id=id}));
    }
}
