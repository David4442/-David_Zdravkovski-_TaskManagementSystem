using Exam.Data;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class TestController : ControllerBase
{
    private readonly TaskManagementContext _context;

    public TestController(TaskManagementContext context)
    {
        _context = context;
    }

    [HttpGet("test-connection")]
    public IActionResult TestConnection()
    {
        var canConnect = _context.Database.CanConnect();
        return Ok(new { Success = canConnect });
    }
}
