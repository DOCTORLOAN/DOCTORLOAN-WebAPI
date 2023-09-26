using DoctorLoan.Application.Common.Security;
using DoctorLoan.Domain.Enums.Authorizations;
using DoctorLoan.Products.Application.Features.Products.Admin.Commands;
using DoctorLoan.Products.Application.Features.Products.Admin.Commands.InsertProduct;
using DoctorLoan.Products.Application.Features.Products.Admin.Queries;
using Microsoft.AspNetCore.Mvc;

namespace DoctorLoan_Products.WebUI.Areas.Controllers;
public class ProductController : ApiControllerBase
{
    [HttpGet]
    //[Authorize(PermissionModuleEnum.Product, PermissionActionEnum.Read)]
    public async Task<IActionResult> FilterProducts([FromQuery] FilterProductQuery query)
    {
        return Ok(await Mediator.Send(query));
    }

    [HttpGet]
    public async Task<IActionResult> GetProduct(int id)
    {
        return Ok(await Mediator.Send(new GetProductByIdQuery(id)));
    }

    [HttpGet]
    public async Task<IActionResult> GetOptions()
    {
        return Ok(await Mediator.Send(new GetOptionsQuery()));
    }

    [Authorize(PermissionModuleEnum.Product, PermissionActionEnum.Create)]
    [HttpPost]
    public async Task<IActionResult> InsertProduct([FromForm] InsertProductCommand command)
    {
        return Ok(await Mediator.Send(command));
    }

    [Authorize(PermissionModuleEnum.Product, PermissionActionEnum.Update)]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProduct([FromRoute] int id, [FromForm] UpdateProductCommand command)
    {
        command.Id = id;
        return Ok(await Mediator.Send(command));
    }

    [Authorize(PermissionModuleEnum.Product, PermissionActionEnum.Update)]
    [HttpPut]
    public async Task<IActionResult> UpdateProductStatus([FromBody] UpdateProductStatusCommand command)
    {
        return Ok(await Mediator.Send(command));
    }

    [Authorize(PermissionModuleEnum.Product, PermissionActionEnum.Delete)]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct([FromRoute] int id)
    {

        return Ok(await Mediator.Send(new DeleteProductCommand { Id = id }));
    }
}
