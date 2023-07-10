using LT.DigitalOffice.ClaimService.Business.Features.Categories.Commands.Edit;
using LT.DigitalOffice.ClaimService.Business.Features.Categories.Queries.GetCategories;
using LT.DigitalOffice.ClaimService.Business.Features.Categories.Queries.GetCategory;
using LT.DigitalOffice.ClaimService.Business.Features.Claims.Commands.Create;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
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
  [HttpPost]
  public async Task<IActionResult> CreateAsync(
    [FromBody] CreateCategoryCommand command,
    CancellationToken ct)
  {
    return Created("/categories", await _mediator.Send(command, ct));
  }

  /// <summary>
  /// Edit category.
  /// </summary>
  /// <param name="categoryId">Id of the claim to edit.</param>
  /// <param name="patch">Properties of the claim to edit.</param>
  /// <returns></returns>
  [HttpPatch("{categoryId}")]
  public async Task<IActionResult> EditAsync(
    [FromRoute] Guid categoryId,
    [FromBody] JsonPatchDocument<EditCategoryRequest> patch,
    CancellationToken ct)
  {
    return Ok(await _mediator.Send(new EditCategoryCommand { CategoryId = categoryId, Patch = patch }, ct));
  }

  /// <summary>
  /// Get categories.
  /// </summary>
  [HttpGet]
  public async Task<IActionResult> GetAsync(
    [FromQuery] GetCategoriesQuery query,
    CancellationToken ct)
  {
    return Ok(await _mediator.Send(query, ct));
  }

  /// <summary>
  /// Get category.
  /// </summary>
  [HttpGet("{categoryId}")]
  public async Task<IActionResult> GetAsync(
    [FromRoute] GetCategoryQuery query,
    CancellationToken ct)
  {
    return Ok(await _mediator.Send(query, ct));
  }
}
