namespace net.shonx.privatenotes.frontend.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[AllowAnonymous]
public class ErrorController : Controller
{
    [AllowAnonymous]
    public IActionResult Index()
    {
        return View();
    }

    [AllowAnonymous]
    public new IActionResult NotFound()
    {
        return View();
    }

    [AllowAnonymous]
    public new IActionResult BadRequest()
    {
        return View();
    }

    [AllowAnonymous]
    public IActionResult InternalServerError()
    {
        return View();
    }

    public IActionResult SecretNameTaken()
    {
        return View();
    }
}
