using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace BudgetApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RandomController : ControllerBase
{

  [HttpGet(Name = "GetRandomNumber")]
  [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "<Pending>")]
  public IEnumerable<int> Get()
  {
    Random random = new();
    yield return random.Next(12) + 1;
  }


}

