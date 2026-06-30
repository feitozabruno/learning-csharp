using Microsoft.AspNetCore.Mvc;

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

    // esse método (POST /api/transactions)
    // converte o body para transaction diretamente,
    // talvez não seja o melhor a se fazer,
    // porque em caso de erro,
    // não é possivel retornar uma mensagem de erro personalizada,
    // o ideal seria identificar o erro e direcionar o usuário a corrigir.
    [HttpPost]
    public IActionResult Create([FromBody] Transaction transaction)
    {
        Transaction newTransaction = _repo.Create(transaction);
        return Created($"/api/transactions/{newTransaction.Id}", newTransaction);
    }

    [HttpGet("{id}")]
    public IActionResult GetById([FromRoute] int id)
    {
        Transaction? transaction = _repo.GetById(id);
        return transaction is null ? NotFound() : Ok(transaction);
    }

    // mesmo problema da conversão direta do método POST
    [HttpDelete("{id}")]
    public IActionResult Delete([FromRoute] int id)
    {
        bool removed = _repo.Delete(id);
        return removed ? NoContent() : NotFound();
    }


    [HttpPut("{id}")]
    public IActionResult UpdatePut([FromRoute] int id, [FromBody] Transaction updatedTransaction)
    {
        return Ok(_repo.UpdatePut(id, updatedTransaction));
    }

    [HttpPatch("{id}")]
    public IActionResult UpdatePatch([FromRoute] int id, TransactionPatch patch)
    {
        return Ok(_repo.UpdatePatch(id, patch));
    }
}
