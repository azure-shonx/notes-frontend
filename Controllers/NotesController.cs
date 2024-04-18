namespace net.shonx.privatenotes.frontend.Controllers;

using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web;
using net.shonx.privatenotes.backend;
using net.shonx.privatenotes.frontend.Data;
using net.shonx.privatenotes.frontend.Models;

[AuthorizeForScopes(ScopeKeySection = "Notes:Scopes")]
public class NotesController : Controller
{
    private readonly ILogger<NotesController> _logger;
    private readonly ITokenAcquisition _tokenAcquisition;
    private readonly BackendCommunicator BackendCommunicator;

    public NotesController(ILogger<NotesController> logger, ITokenAcquisition tokenAcquisition)
    {
        _logger = logger;
        _tokenAcquisition = tokenAcquisition;
        BackendCommunicator = new();
    }

    // GET: /Note/
    public async Task<IActionResult> Index()
    {
        List<Note>? Notes = await BackendCommunicator.GetAllNotes(await Token());
        if (Notes is null)
            return RedirectToAction("InternalServerError", "Error");
        return View(Notes);
    }

    // GET: /Note/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: /Note/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind()] CreateableNote Note)
    {
        if (!ValidateNote(Note))
            return RedirectToAction("BadRequest", "Error");
        AKVHResponse response = await BackendCommunicator.CreateNote(await Token(), new(Note));
        return response switch
        {
            AKVHResponse.OK => RedirectToAction(nameof(Index)),
            AKVHResponse.SECRET_NAME_TAKEN => RedirectToAction("SecretNameTaken", "Error"),
            _ => RedirectToAction("InternalServerError", "Error"),
        };
    }

    // GET: /Note/Edit/{SecretName}
    [HttpGet]
    public async Task<IActionResult> Edit(string? SecretName)
    {
        Console.WriteLine($"Looking up note {SecretName}");
        Note? Note = await GetNote(SecretName);
        if (Note is null)
        {
            return RedirectToAction("NotFound", "Error");
        }
        return View(new EditableNote(Note));
    }

    // POST: /Note/Edit/{SecretName}
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(string? SecretName, [Bind()] EditableNote Note)
    {
        if (!ValidateNote(Note))
            return RedirectToAction("BadRequest", "Error");
        if (ModelState.IsValid)
        {
            if (Note.NameRenamed() || Note.ValueChanged())
            {
                AKVHResponse response = await BackendCommunicator.UpdateNote(await Token(), Note.ID, Note.Name, Note.Value);
                return response switch
                {
                    AKVHResponse.OK => RedirectToAction(nameof(Index)),
                    AKVHResponse.SECRET_NAME_TAKEN => RedirectToAction("SecretNameTaken", "Error"),
                    _ => RedirectToAction("InternalServerError", "Error"),
                };
            }
        }
        return RedirectToAction(nameof(Index));
    }

    // GET: /Note/Delete/{SecretName}
    public async Task<IActionResult> Delete(string? SecretName)
    {
        Note? Note = await GetNote(SecretName);
        if (Note is null)
            return RedirectToAction("NotFound", "Error");
        return View(Note);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    // POST: /Note/Delete/{SecretName}
    public async Task<IActionResult> DeleteConfirmed(string? SecretName)
    {
        if (string.IsNullOrEmpty(SecretName))
            return RedirectToAction("NotFound", "Error");
        AKVHResponse response = await BackendCommunicator.DeleteNote(await Token(), SecretName);
        return response switch
        {
            AKVHResponse.OK => RedirectToAction(nameof(Index)),
            _ => RedirectToAction("InternalServerError", "Error"),
        };
    }

    private async Task<string> Token()
    {
        return await _tokenAcquisition.GetAccessTokenForUserAsync(["user.read"]).ConfigureAwait(false);
    }

    private async Task<Note?> GetNote(string? SecretName)
    {
        if (string.IsNullOrEmpty(SecretName))
            return null;
        return await BackendCommunicator.GetNote(await Token(), SecretName);
    }

    private bool ValidateNote(INote? Note)
    {
        if (Note is null)
            return false;
        if (string.IsNullOrEmpty(Note.Name()) || string.IsNullOrEmpty(Note.Value()))
            return false;
        return Note.IsValidName();
    }


}