using System.Net;
using DoctorLoan.Application.Common.Security;
using DoctorLoan.Application.Models.Commons;
using DoctorLoan.Customer.Application.Features.CustomerAddresses;
using DoctorLoan.Customer.Application.Features.CustomerAddresses.Dtos;
using DoctorLoan.Customer.Application.Features.Customers;
using DoctorLoan.Domain.Enums.Authorizations;
using Microsoft.AspNetCore.Mvc;

namespace DoctorLoan.Customer.WebUI.Areas.Controllers;

public class CustomerController : ApiControllerBase
{
    #region filter

    [HttpGet]
    [Route("filter")]
    [ProducesResponseType(typeof(Result<PaginatedList<CustomerDto>>), (int)HttpStatusCode.OK)]
    [Authorize(PermissionModuleEnum.Customer, PermissionActionEnum.Read)]
    public async Task<IActionResult> FilterCustomer([FromQuery] FilterCustomerQuery query, CancellationToken cancellationToken)
    {
        return Ok(await Mediator.Send(query, cancellationToken));
    }

    [HttpGet]
    [Route("filter-address")]
    [Authorize(PermissionModuleEnum.Customer, PermissionActionEnum.Read)]
    [ProducesResponseType(typeof(Result<PaginatedList<CustomerAddressDto>>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> FilterCustomerAddress([FromQuery] FilterCustomerAddressQuery query, CancellationToken cancellationToken)
    {
        return Ok(await Mediator.Send(query, cancellationToken));
    }


    [HttpGet]
    [Route("{id:int}")]
    [Authorize(PermissionModuleEnum.Customer, PermissionActionEnum.Read)]
    [ProducesResponseType(typeof(Result<CustomerDto>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetCustomerById([FromRoute] int id, CancellationToken cancellationToken)
    {
        return Ok(await Mediator.Send(new GetCustomerByIdQuery(id), cancellationToken));
    }

    #endregion

    #region CRUD customer

    [Route("create")]
    [Authorize(PermissionModuleEnum.Customer, PermissionActionEnum.Create)]
    [ProducesResponseType(typeof(Result<int>), (int)HttpStatusCode.OK)]
    [HttpPost]
    public async Task<IActionResult> CreateCustomer([FromBody] AddCustomerCommand command, CancellationToken cancellationToken)
    {
        return Ok(await Mediator.Send(command, cancellationToken));
    }


    [Route("update")]
    [Authorize(PermissionModuleEnum.Customer, PermissionActionEnum.Update)]
    [ProducesResponseType(typeof(Result<int>), (int)HttpStatusCode.OK)]
    [HttpPut]
    public async Task<IActionResult> UpdateCustomer([FromBody] UpdateCustomerCommand command, CancellationToken cancellationToken)
    {
        return Ok(await Mediator.Send(command, cancellationToken));
    }

    [HttpDelete]
    [Route("{id:int}")]
    [ProducesResponseType(typeof(Result<bool>), (int)HttpStatusCode.OK)]
    [Authorize(PermissionModuleEnum.Customer, PermissionActionEnum.Delete)]
    public async Task<IActionResult> DeleteCustomer([FromRoute] int id, CancellationToken cancellationToken)
    {
        return Ok(await Mediator.Send(new DeleteCustomerCommand(id), cancellationToken));
    }


    #endregion

    #region crud customer address
    [Route("{id:int}/address")]
    [ProducesResponseType(typeof(Result<bool>), (int)HttpStatusCode.OK)]
    [Authorize(PermissionModuleEnum.Customer, PermissionActionEnum.Create)]
    [HttpPost]
    public async Task<IActionResult> CreateCustomerAddress([FromRoute] int id, [FromBody] AddCustomerAddressCommand command, CancellationToken cancellationToken)
    {
        command.CustomerId = id;
        return Ok(await Mediator.Send(command, cancellationToken));
    }

    [Route("{id:int}/address")]
    [ProducesResponseType(typeof(Result<bool>), (int)HttpStatusCode.OK)]
    [Authorize(PermissionModuleEnum.Customer, PermissionActionEnum.Update)]
    [HttpPut]
    public async Task<IActionResult> UpdateCustomerAddress([FromRoute] int id, [FromBody] UpdateCustomerAddressCommand command, CancellationToken cancellationToken)
    {
        command.CustomerId = id;
        return Ok(await Mediator.Send(command, cancellationToken));
    }

    [HttpDelete]
    [Route("address/{id}")]
    [ProducesResponseType(typeof(Result<bool>), (int)HttpStatusCode.OK)]
    [Authorize(PermissionModuleEnum.Customer, PermissionActionEnum.Delete)]
    public async Task<IActionResult> DeleteUser([FromRoute] int id, CancellationToken cancellationToken)
    {
        return Ok(await Mediator.Send(new DeleteCustomerAddressCommand(id), cancellationToken));
    }
    #endregion
}