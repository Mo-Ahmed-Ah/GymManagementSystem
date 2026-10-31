using GymManagementSystem.BLL.Services.AttachmentServices.Interfaces;
using GymManagementSystem.BLL.Services.MembersServices.Interfaces;
using GymManagementSystem.BLL.ViewModels.MemberViewModels;
using GymManagementSystem.MVC.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementSystem.PL.Controllers;

[Authorize]
public class MembersController : Controller
{
    private readonly IAttachmentServices _attachmentServices;
    private readonly IMemberServices _memberServices;
    public MembersController(IMemberServices memberServices, IAttachmentServices attachmentServices)
    {
        _memberServices = memberServices;
        _attachmentServices = attachmentServices;
    }
    [Authorize(Roles = "SuperAdmin , Admin")]
    public async Task<IActionResult> Index()
    {
        var result = await _memberServices.GetMemoriesAsync();
        if (result.Success)
            return View(result.Value);
        return RedirectToAction(nameof(Index), nameof(HomeController));
    }
    [Authorize(Roles = "SuperAdmin , Admin")]
    public IActionResult Create()
    {
        return View();
    }
    [Authorize(Roles = "SuperAdmin , Admin")]
    [HttpPost]
    public async Task<IActionResult> Create(CreateMemberViewModel model)
    {
        if (!ModelState.IsValid) return View(model);
        var result = await _memberServices.AddMemberAsync(model);
        if (result.Success)
        {
            TempData["SuccessMessage"] = "Member created successfully.";
            return RedirectToAction(nameof(Index));
        }
        TempData["ErrorMessage"] = result.error;
        return View(model);
    }
    [Authorize(Roles = "SuperAdmin , Admin , Member , Trainer")]
    public async Task<IActionResult> Details(int? id)
    {
        var result = await _memberServices.GetMemberDetailsAsync(id);
        if (!result.Success)
        {
            TempData["ErrorMessage"] = result.error;
            return RedirectToAction(nameof(Index));
        }
        return View(result.Value);
    }
    [Authorize(Roles = "SuperAdmin , Admin , Member , Trainer")]
    public async Task<IActionResult> GetPicture(int? id)
    {
        var result = await _memberServices.GetMemberDetailsAsync(id);
        if (!result.Success)
            return NotFound();

        var file = _attachmentServices.GetAsync(result.Value!.Photo!, "MemberPhotos");
        if (file == null)
            return NotFound();
        return File(file.Value.stream, file.Value.contentType);
    }
    [Authorize(Roles = "SuperAdmin , Admin , Member , Trainer")]
    public async Task<IActionResult> HealthRecordDetails(int? id)
    {
        var result = await _memberServices.GetHealthRecordDetailsAsync(id);
        if (!result.Success)
        {
            TempData["ErrorMessage"] = result.error;
            return RedirectToAction(nameof(Index));
        }
        return View(result.Value);
    }
    [Authorize(Roles = "SuperAdmin , Admin , Member")]
    public async Task<IActionResult> Edit(int? id)
    {
        var result = await _memberServices.GetMemberToUpdateAsync(id);
        if (!result.Success)
        {
            TempData["ErrorMessage"] = result.error;
            return RedirectToAction(nameof(Index));
        }

        return View(result.Value);
    }
    [Authorize(Roles = "SuperAdmin , Admin , Member")]
    [HttpPost]
    public async Task<IActionResult> Edit(MemberToUpdateViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var result = await _memberServices.UpdateMemberAsync(model);
        if (result.Success)
        {
            TempData["SuccessMessage"] = "Member Updated successfully.";
            return RedirectToAction(nameof(Index));
        }

        TempData["ErrorMessage"] = result.error;
        return View(model);
    }
    [Authorize(Roles = "SuperAdmin , Admin , Member")]
    public async Task<IActionResult> Delete(int? id)
    {
        return View(id);
    }
    [Authorize(Roles = "SuperAdmin , Admin , Member")]
    [HttpPost]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {

        var result = await _memberServices.DeleteMemberAsync(id);
        if (result.Success)
        {
            TempData["SuccessMessage"] = "Member delete successfully.";
            return RedirectToAction(nameof(Index));
        }
        TempData["ErrorMessage"] = result.error;
        return View(id);
    }

}
