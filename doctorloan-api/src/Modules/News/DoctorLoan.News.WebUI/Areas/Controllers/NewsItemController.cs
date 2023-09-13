using DoctorLoan.Application.Common.Security;
using DoctorLoan.Domain.Enums.Authorizations;
using DoctorLoan.News.Application.Features.News.Admin.Commands;
using DoctorLoan.News.Application.Features.News.Admin.Commands.DeleteNewsItem;
using DoctorLoan.News.Application.Features.News.Admin.Queries;
using DoctorLoan.News.Application.Features.News.Admin.Queries.GetTags;
using Microsoft.AspNetCore.Mvc;

namespace DoctorLoan.News.WebUI.Areas.Controllers;
public class NewsItemController : ApiControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetTags()
    {
        return Ok(await Mediator.Send(new GetTagsQuery()));
    }

    [HttpGet]
    [Authorize(PermissionModuleEnum.News, PermissionActionEnum.Read)]
    public async Task<IActionResult> FilterNews([FromQuery] FilterNewsItemsQuery query)
    {
        return Ok(await Mediator.Send(query));
    }

    [HttpGet]
    [Authorize(PermissionModuleEnum.News, PermissionActionEnum.Read)]
    public async Task<IActionResult> GetNewById(int id)
    {
        return Ok(await Mediator.Send(new GetNewsItemByIdQuery(id)));
    }

    [HttpPost]
    [Authorize(PermissionModuleEnum.News, PermissionActionEnum.Create)]
    public async Task<IActionResult> InsertNews([FromForm] InsertNewsItemCommand command)
    {
        return Ok(await Mediator.Send(command));
    }

    [Authorize(PermissionModuleEnum.News, PermissionActionEnum.Update)]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateNews([FromRoute] int id, [FromForm] UpdateNewsItemCommand command)
    {
        command.Id = id;
        return Ok(await Mediator.Send(command));
    }

    [Authorize(PermissionModuleEnum.News, PermissionActionEnum.Update)]
    [HttpPut]
    public async Task<IActionResult> UpdatNewsStatus([FromBody] UpdateNewsItemStatusCommand command)
    {
        return Ok(await Mediator.Send(command));
    }

    [Authorize(PermissionModuleEnum.News, PermissionActionEnum.Delete)]
    [HttpDelete]
    public async Task<IActionResult> DeleteNews([FromBody] DeleteNewsItemCommand command)
    {
        return Ok(await Mediator.Send(command));
    }
}
