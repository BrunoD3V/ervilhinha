using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Ervilhinha.Data;
using Ervilhinha.Models;
using Ervilhinha.Services;
using System.Security.Claims;

namespace Ervilhinha.Controllers
{
    [Authorize]
    public class BabyTimelineController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly SmartAlertService _alertService;

        public BabyTimelineController(ApplicationDbContext context, SmartAlertService alertService)
        {
            _context = context;
            _alertService = alertService;
        }

        // GET: BabyTimeline
        public async Task<IActionResult> Index(BabyPhase? phase, bool? completed)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var events = _context.BabyTimelines.Where(t => t.UserId == userId).AsQueryable();

            if (phase.HasValue)
            {
                events = events.Where(t => t.Phase == phase.Value);
            }

            if (completed.HasValue)
            {
                events = events.Where(t => t.IsCompleted == completed.Value);
            }

            var eventList = await events.OrderBy(t => t.EventDate ?? DateTime.MaxValue).ThenBy(t => t.Priority).ToListAsync();

            ViewBag.UpcomingCount = eventList.Count(e => !e.IsCompleted && e.EventDate >= DateTime.Today);
            ViewBag.OverdueCount = eventList.Count(e => !e.IsCompleted && e.EventDate < DateTime.Today);

            return View(eventList);
        }

        // GET: BabyTimeline/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: BabyTimeline/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BabyTimeline timeline)
        {
            if (ModelState.IsValid)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                timeline.UserId = userId!;
                timeline.CreatedDate = DateTime.UtcNow;

                _context.Add(timeline);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Evento adicionado à timeline!";
                return RedirectToAction(nameof(Index));
            }

            return View(timeline);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAjax([FromForm] BabyTimeline timeline)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    timeline.UserId = userId!;
                    timeline.CreatedDate = DateTime.UtcNow;

                    _context.Add(timeline);
                    await _context.SaveChangesAsync();

                    return Json(new { success = true });
                }

                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                return Json(new { success = false, message = string.Join(", ", errors) });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreatePartial([FromForm] BabyTimeline timeline)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    timeline.UserId = userId!;
                    timeline.CreatedDate = DateTime.UtcNow;

                    _context.Add(timeline);
                    await _context.SaveChangesAsync();

                    return PartialView("_TimelineEventCard", timeline);
                }

                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET: BabyTimeline/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var timeline = await _context.BabyTimelines
                .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);

            if (timeline == null) return NotFound();

            return View(timeline);
        }

        // POST: BabyTimeline/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, BabyTimeline timeline)
        {
            if (id != timeline.Id) return NotFound();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (ModelState.IsValid)
            {
                try
                {
                    var existing = await _context.BabyTimelines
                        .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);

                    if (existing == null) return NotFound();

                    existing.Title = timeline.Title;
                    existing.Description = timeline.Description;
                    existing.Type = timeline.Type;
                    existing.Phase = timeline.Phase;
                    existing.EventDate = timeline.EventDate;
                    existing.PregnancyWeek = timeline.PregnancyWeek;
                    existing.BabyMonthAge = timeline.BabyMonthAge;
                    existing.EstimatedCost = timeline.EstimatedCost;
                    existing.Priority = timeline.Priority;
                    existing.IsCompleted = timeline.IsCompleted;
                    existing.CompletedDate = timeline.CompletedDate;
                    existing.ActualCost = timeline.ActualCost;
                    existing.Recommendation = timeline.Recommendation;
                    existing.HasReminder = timeline.HasReminder;
                    existing.ReminderDaysBefore = timeline.ReminderDaysBefore;
                    existing.ModifiedDate = DateTime.UtcNow;

                    _context.Update(existing);
                    await _context.SaveChangesAsync();

                    TempData["Success"] = "Evento atualizado!";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    return NotFound();
                }
            }

            return View(timeline);
        }

        // POST: BabyTimeline/Complete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Complete(int id, decimal? actualCost)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var timeline = await _context.BabyTimelines
                .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);

            if (timeline == null) return NotFound();

            timeline.IsCompleted = true;
            timeline.CompletedDate = DateTime.Today;
            timeline.ActualCost = actualCost;
            timeline.ModifiedDate = DateTime.UtcNow;

            _context.Update(timeline);
            await _context.SaveChangesAsync();

            TempData["Success"] = $"✅ {timeline.Title} marcado como concluído!";
            return RedirectToAction(nameof(Index));
        }

        // GET: BabyTimeline/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var timeline = await _context.BabyTimelines
                .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);

            if (timeline == null) return NotFound();

            return View(timeline);
        }

        // POST: BabyTimeline/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var timeline = await _context.BabyTimelines
                .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);

            if (timeline != null)
            {
                _context.BabyTimelines.Remove(timeline);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Evento removido!";
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: BabyTimeline/Calendar
        public async Task<IActionResult> Calendar()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var events = await _context.BabyTimelines
                .Where(t => t.UserId == userId && !t.IsCompleted)
                .Where(t => t.EventDate.HasValue)
                .OrderBy(t => t.EventDate)
                .ToListAsync();

            return View(events);
        }

        // POST: BabyTimeline/GenerateAlerts
        [HttpPost]
        public async Task<IActionResult> GenerateAlerts()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _alertService.GenerateTimelineAlerts(userId!);

            return Json(new { success = true, message = "Alertas gerados com sucesso!" });
        }
    }
}
