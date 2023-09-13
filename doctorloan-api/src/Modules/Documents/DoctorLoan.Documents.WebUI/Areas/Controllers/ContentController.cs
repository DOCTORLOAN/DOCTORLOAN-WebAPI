using System.Net;
using DoctorLoan.Application.Models.Commons;
using DoctorLoan.Contents.Application.Features.Contents.Admin;
using DoctorLoan.Contents.Application.Features.Contents.Admin.Commands;
using DoctorLoan.Contents.Application.Features.Contents.Admin.Dtos;
using DoctorLoan.Contents.Application.Features.Contents.Admin.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DoctorLoan.Contents.WebUI.Areas.Controllers;
public class ContentController : ApiControllerBase
{
    #region Contents 
    //[AllowAnonymous]
    [HttpGet]
    [Route("filter")]
    [ProducesResponseType(typeof(Result<PaginatedList<ContentInfomationDto>>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetDocumentByCondition([FromQuery] FilterContentQuery request, CancellationToken cancellationToken)
    {
        return Ok(await Mediator.Send(request, cancellationToken));
    }

    [AllowAnonymous]
    [Route("{id}")]
    [ProducesResponseType(typeof(Result<ContentInfomationDto>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetDocumentDetailById([FromRoute] int id, CancellationToken cancellationToken)
    {
        return Ok(await Mediator.Send(new ContentDetailQuery(id), cancellationToken));
    }

    [AllowAnonymous]
    [HttpPost]
    [Route("create")]
    [ProducesResponseType(typeof(Result<bool>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> CreateDocument([FromBody] CreateContentCommand command, CancellationToken cancellationToken)
    {
        return Ok(await Mediator.Send(command, cancellationToken));
    }

    [AllowAnonymous]
    [HttpPut]
    [Route("{id}")]
    [ProducesResponseType(typeof(Result<bool>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> UpdateDocument(int id, [FromBody] UpdateContentCommand command, CancellationToken cancellationToken)
    {
        command.Id = id;
        return Ok(await Mediator.Send(command, cancellationToken));
    }
    #endregion
}
