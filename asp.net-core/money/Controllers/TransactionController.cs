using Microsoft.AspNetCore.Mvc;
using Money.Models;
using Money.Dtos;
using Money.Repositories.Interfaces;

namespace Money.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TransactionsController : ControllerBase
{
    private readonly ITransactionRepository _repo;

    public TransactionsController(ITransactionRepository repo)
    {
        _repo = repo;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _repo.GetAllAsync());
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] TransactionCreateDto dto)
    {
        Transaction transaction = await _repo.Create(dto);
        return Created($"/api/transactions/{transaction.Id}", transaction);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        Transaction? transaction = await _repo.GetByIdAsync(id);
        return transaction is null ? NotFound() : Ok(transaction);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        bool removed = await _repo.Delete(id);
        return removed ? NoContent() : NotFound();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePut([FromRoute] int id, [FromBody] TransactionUpdateDto dto)
    {
        return Ok(await _repo.UpdatePut(id, dto));
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> UpdatePatch([FromRoute] int id, TransactionPatchDto dto)
    {
        return Ok(await _repo.UpdatePatch(id, dto));
    }
}
