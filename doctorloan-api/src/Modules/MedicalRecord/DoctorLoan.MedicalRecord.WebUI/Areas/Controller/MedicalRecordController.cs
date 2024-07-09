using System.Net;
using DoctorLoan.Application.Common.Security;
using DoctorLoan.Application.Models.Commons;
using DoctorLoan.MedicalRecord.Application.Features.Commands;
using DoctorLoan.MedicalRecord.Application.Features.Dtos;
using DoctorLoan.MedicalRecord.Application.Features.Queries;
using DoctorLoan.Domain.Enums.Authorizations;
using Microsoft.AspNetCore.Mvc;

namespace DoctorLoan.MedicalRecord.WebUI.Areas.Controller;
public class MedicalRecordController : ApiControllerBase
{
    #region Filter
    [HttpGet]
    [Route("filter")]
    [ProducesResponseType(typeof(Result<PaginatedList<MedicalRecordDto>>), (int)HttpStatusCode.OK)]
    [Authorize(PermissionModuleEnum.MedicalRecord, PermissionActionEnum.Read)]
    public async Task<IActionResult> FilterMedicalRecord([FromQuery] FilterMedicalRecordQuery query, CancellationToken cancellationToken)
    {
        return Ok(await Mediator.Send(query, cancellationToken));
    }

    [HttpGet]
    [Route("{id:int}")]
    [ProducesResponseType(typeof(Result<MedicalRecordDto>), (int)HttpStatusCode.OK)]
    [Authorize(PermissionModuleEnum.MedicalRecord, PermissionActionEnum.Read)]
    public async Task<IActionResult> GetMedicalRecordById([FromRoute] int id, CancellationToken cancellationToken)
    {
        return Ok(await Mediator.Send(new GetMedicalRecordByIdQuery(id), cancellationToken));
    }

    #endregion

    #region CRUD customer

    [Route("create")]
    [ProducesResponseType(typeof(Result<int>), (int)HttpStatusCode.OK)]
    [HttpPost]
    public async Task<IActionResult> CreateMedicalRecord([FromBody] AddMedicalRecordCommand command, CancellationToken cancellationToken)
    {
        return Ok(await Mediator.Send(command, cancellationToken));
    }

    [Route("update-status")]
    [ProducesResponseType(typeof(Result<int>), (int)HttpStatusCode.OK)]
    [Authorize(PermissionModuleEnum.MedicalRecord, PermissionActionEnum.Update)]
    [HttpPatch]
    public async Task<IActionResult> UpdateStatusMedicalRecord([FromBody] UpdateMedicalRecordCommand command, CancellationToken cancellationToken)
    {
        return Ok(await Mediator.Send(command, cancellationToken));
    }
    #endregion
}