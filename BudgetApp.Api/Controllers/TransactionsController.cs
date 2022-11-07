using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BudgetApp;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BudgetApp.Api.Controllers;

[Route("api/[controller]")]
public class TransactionsController : Controller
{
  private readonly GenID ID = new();
  // GET: api/Transactions
  [HttpGet]
  public static IEnumerable<string> Get()
  {
    return new string[] { "value1", "value2" };
  }

  // GET api/Transactions/:id
  [HttpGet("{id}")]
  public IActionResult Get(int id)
  {
    return Ok();
  }

  // POST api/Transactions
  [HttpPost]
  public IActionResult Post([FromBody] Transaction model)
  {
    if (model == null)
    {
      return BadRequest();
    }

    if (!ModelState.IsValid)
    {
      return UnprocessableEntity();
    }

    Transaction txn = new()
    {
      TransactionID = ID.Next(),
      AmountCents = model.AmountCents,
      Category = model.Category,
      Day = model.Day,
      UserId = model.UserId,
      Note = model.Note
    };

    return Ok(txn);
  }

  // PUT api/Transactions/:id
  [HttpPut("{id}")]
  public IActionResult Put(int id, [FromBody] Transaction txn)
  {
    //var curTxn = 
    return Ok(txn);
  }

  // DELETE api/Transactions/:id
  [HttpDelete("{id}")]
  public IActionResult Delete(int id)
  {
    return NoContent();
  }
}

