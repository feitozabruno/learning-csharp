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
    public IActionResult GetAll()
    {
        return Ok(_repo.GetAll());
    }

    [HttpPost]
    public IActionResult Create([FromBody] TransactionCreateDto dto)
    {
        Transaction transaction = _repo.Create(dto);
        return Created($"/api/transactions/{transaction.Id}", transaction);
    }

    [HttpGet("{id}")]
    public IActionResult GetById([FromRoute] int id)
    {
        Transaction? transaction = _repo.GetById(id);
        return transaction is null ? NotFound() : Ok(transaction);
    }

    [HttpDelete("{id}")]
    public IActionResult Delete([FromRoute] int id)
    {
        bool removed = _repo.Delete(id);
        return removed ? NoContent() : NotFound();
    }

    [HttpPut("{id}")]
    public IActionResult UpdatePut([FromRoute] int id, [FromBody] TransactionUpdateDto dto)
    {
        return Ok(_repo.UpdatePut(id, dto));
    }

    [HttpPatch("{id}")]
    public IActionResult UpdatePatch([FromRoute] int id, TransactionPatchDto dto)
    {
        return Ok(_repo.UpdatePatch(id, dto));
    }
}
