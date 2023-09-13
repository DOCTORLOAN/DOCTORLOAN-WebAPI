using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using DoctorLoan.Application.Models.Commons;
using DoctorLoan.Common.Application.Features.Medias.Admin.Commands;
using DoctorLoan.Common.Application.Features.Menus.Admin.Queries;
using DoctorLoan.Domain.Enums.Authorizations;
using DoctorLoan.Domain.Enums.Commons;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace DoctorLoan.Common.WebUI.Areas.Controllers;

public class MediaController : ApiControllerBase
{

    [HttpPost]
    public async Task<IActionResult> UploadImage([FromForm]UploadMediaCommand command, CancellationToken cancellationToken)
    {
        return Ok(await Mediator.Send(command, cancellationToken));
    }
   

}