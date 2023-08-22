using DigitalOffice.Kernel.OpenApi.OperationFilters;
using DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.ClaimService.Business.Features.Categories.Commands.Edit;
using LT.DigitalOffice.ClaimService.Business.Features.Categories.Commands.Update;
using LT.DigitalOffice.ClaimService.Business.Features.Categories.Queries.GetCategories;
using LT.DigitalOffice.ClaimService.Business.Features.Categories.Queries.GetCategory;
using LT.DigitalOffice.ClaimService.Business.Features.Claims.Commands.Create;
using LT.DigitalOffice.Kernel.Exceptions.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;

namespace LT.DigitalOffice.ClaimService.Controllers;

[Produces("application/json")]
[Consumes("application/json")]
[Route("[controller]")]
[ApiController]
public class CategoriesController : ControllerBase
{
  private readonly IMediator _mediator;

  public CategoriesController(IMediator mediator)
  {
    _mediator = mediator;
  }

  /// <summary>
  /// Create category.
  /// </summary>
  /// <param name="command">Category to create.</param>
  [HttpPost]
  [SwaggerOperationFilter(typeof(TokenOperationFilter))]
  [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
  [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status401Unauthorized)]
  [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
  public async Task<IActionResult> CreateAsync(
    [FromBody][Required] CreateCategoryCommand command,
    CancellationToken ct)
  {
    return Created("/categories", await _mediator.Send(command, ct));
  }

  /// <summary>
  /// Edit category
  /// </summary>
  /// <param name="categoryId">Id of category to edit</param>
  /// <param name="patch">Properties of category to edit</param>
  [HttpPatch("{categoryId}")]
  [SwaggerOperationFilter(typeof(TokenOperationFilter))]
  [ProducesResponseType(typeof(Unit), StatusCodes.Status200OK)]
  [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status401Unauthorized)]
  [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
  public async Task<IActionResult> EditAsync(
    [FromRoute][Required] Guid categoryId,
    [FromBody][Required] JsonPatchDocument<EditCategoryRequest> patch,
    CancellationToken ct)
  {
    return Ok(await _mediator.Send(new EditCategoryCommand
    {
      CategoryId = categoryId,
      Patch = patch
    }, ct));
  }

  /// <summary>
  /// Get categories
  /// </summary>
  [HttpGet]
  [SwaggerOperationFilter(typeof(TokenOperationFilter))]
  [ProducesResponseType(typeof(FindResult<CategoryInfo>), StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status401Unauthorized)]
  public async Task<IActionResult> GetAsync(
    [FromQuery] GetCategoriesQuery query,
    CancellationToken ct)
  {
    return Ok(await _mediator.Send(query, ct));
  }

  /// <summary>
  /// Get category
  /// </summary>
  [HttpGet("{categoryId}")]
  [SwaggerOperationFilter(typeof(TokenOperationFilter))]
  [ProducesResponseType(typeof(CategoryResponse), StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status401Unauthorized)]
  public async Task<IActionResult> GetAsync(
    [FromRoute] GetCategoryQuery query,
    CancellationToken ct)
  {
    return Ok(await _mediator.Send(query, ct));
  }

  /// <summary>
  /// Update category
  /// </summary>
  /// <param name="categoryId">Id of category to update</param>
  /// <param name="request">Properties of category to update</param>
  [HttpPut("{categoryId}")]
  [SwaggerOperationFilter(typeof(TokenOperationFilter))]
  [ProducesResponseType(typeof(Unit), StatusCodes.Status200OK)]
  [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status401Unauthorized)]
  [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
  [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
  public async Task<IActionResult> UpdateAsync(
    [FromRoute][Required] Guid categoryId,
    [FromBody][Required] UpdateCategoryRequest request,
    CancellationToken ct)
  {
    return Ok(await _mediator.Send(new UpdateCategoryCommand
    {
      CategoryId = categoryId,
      Request = request
    }, ct));
  }
}
