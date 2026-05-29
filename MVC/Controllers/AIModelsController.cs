using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVC.Data;
using MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVC.Controllers;


public class AIModelsController : Controller
{
    private readonly MVCContext _context;

    public AIModelsController(MVCContext context)
    {
        _context = context;
    }

    // GET: AIModels
    public async Task<IActionResult> Index()
    {
        return View(await _context.AIModel.ToListAsync());
    }

    // GET: AIModels/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var aIModel = await _context.AIModel
            .FirstOrDefaultAsync(m => m.Id == id);
        if (aIModel == null)
        {
            return NotFound();
        }

        return View(aIModel);
    }

    // GET: AIModels/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: AIModels/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(AIModel aIModel)
    {
        if (ModelState.IsValid)
        {
            _context.Add(aIModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(aIModel);
    }

    // GET: AIModels/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var aIModel = await _context.AIModel.FindAsync(id);
        if (aIModel == null)
        {
            return NotFound();
        }
        return View(aIModel);
    }

    // POST: AIModels/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,ModelName,EmailRegisted,Description,CreatedDate,ExpiryDate,TotalQuantity,RemainingQuantity")] AIModel aIModel)
    {
        if (id != aIModel.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(aIModel);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AIModelExists(aIModel.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        return View(aIModel);
    }

    // GET: AIModels/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var aIModel = await _context.AIModel
            .FirstOrDefaultAsync(m => m.Id == id);
        if (aIModel == null)
        {
            return NotFound();
        }

        return View(aIModel);
    }

    // POST: AIModels/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var aIModel = await _context.AIModel.FindAsync(id);
        if (aIModel != null)
        {
            _context.AIModel.Remove(aIModel);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool AIModelExists(int id)
    {
        return _context.AIModel.Any(e => e.Id == id);
    }
}
