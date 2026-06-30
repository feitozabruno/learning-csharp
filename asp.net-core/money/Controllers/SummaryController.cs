using Microsoft.AspNetCore.Mvc;
using System.Globalization;

[ApiController]
[Route("/api/[controller]")]
public class SummaryController : ControllerBase
{
    private readonly ITransactionRepository _repo;

    public SummaryController(ITransactionRepository repo)
    {
        _repo = repo;
    }

    [HttpGet]
    public IActionResult Summary()
    {
        List<Transaction> transactions = _repo.GetAll();
        decimal incomes = 0;
        decimal outcomes = 0;

        foreach (Transaction transaction in transactions)
        {
            if (transaction.Type == TransactionType.Income) incomes += transaction.Value;
            if (transaction.Type == TransactionType.Outcome) outcomes += transaction.Value;
        }

        decimal balance = incomes - outcomes;

        var response = new
        {
            Entradas = incomes.ToString("C", new CultureInfo("pt-BR")),
            Saídas = outcomes.ToString("C", new CultureInfo("pt-BR")),
            Saldo = balance.ToString("C", new CultureInfo("pt-BR"))
        };

        return Ok(response);
    }

    [HttpGet("incomes")]
    public IActionResult Incomes()
    {
        List<Transaction> transactions = _repo.GetAll();
        decimal incomes = 0;

        foreach (Transaction transaction in transactions)
        {
            if (transaction.Type == TransactionType.Income) incomes += transaction.Value;
        }

        var response = new
        {
            Incomes = transactions.FindAll(transaction => transaction.Type == TransactionType.Income),
            Sum = incomes.ToString("C", new CultureInfo("pt-BR"))
        };

        return Ok(response);
    }

    [HttpGet("outcomes")]
    public IActionResult Outcomes()
    {
        List<Transaction> transactions = _repo.GetAll();
        decimal outcomes = 0;

        foreach (Transaction transaction in transactions)
        {
            if (transaction.Type == TransactionType.Outcome) outcomes += transaction.Value;
        }

        var response = new
        {
            Outcomes = transactions.FindAll(transaction => transaction.Type == TransactionType.Outcome),
            Sum = outcomes.ToString("C", new CultureInfo("pt-BR"))
        };

        return Ok(response);
    }

    [HttpGet("{category}")]
    public IActionResult Category([FromRoute] string category)
    {
        List<Transaction> transactions = _repo.GetAll();

        decimal sumCategory = 0;

        foreach (Transaction transaction in transactions)
        {
            if (transaction.Category.Equals(category, StringComparison.CurrentCultureIgnoreCase)) sumCategory += transaction.Value;
        }

        var response = new
        {
            Category = transactions.FindAll(transaction => transaction.Category.Equals(category, StringComparison.CurrentCultureIgnoreCase)),
            Sum = sumCategory
        };

        return Ok(response);
    }
}