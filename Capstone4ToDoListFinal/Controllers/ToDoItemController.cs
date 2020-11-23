using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Capstone4ToDoListFinal.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using PagedList;

namespace Capstone4ToDoListFinal.Controllers
{
    [Authorize]
    public class ToDoItemController : Controller
    {
        private ToDoListContext db = new ToDoListContext();
        private readonly ToDoListContext _context;

        public ToDoItemController(ToDoListContext context)
        {
            _context = context;
        }

        // GET: ToDoItem
        public IActionResult GetUserId()
        {
            string id = User.FindFirst(ClaimTypes.NameIdentifier).Value.ToString();
            return ViewBag[id];
           
        }
        public async Task<IActionResult> Index()
        {
            var toDoListContext = _context.TodoItem.Include(t => t.IdNetUsersNavigation);
            return View(await toDoListContext.ToListAsync());
            //ViewBag.CurrentSort = sortOrder;
            //ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            //ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";

            //if(searchString!=null)
            //{
            //    page = 1;
            //}
            //else
            //{
            //    searchString = currentFilter;
            //}
            //ViewBag.CurrentFilter = searchString;
            //var toDo = from t in db.TodoItem
            //           select t;
            //    if(!String.IsNullOrEmpty(searchString))
            //{
            //    toDo = toDo.Where(t => t.Description.Contains(searchString));
            //}
            //switch(sortOrder)
            //{
            //    case "description_desc":
            //        toDo = toDo.OrderByDescending(t => t.Description);
            //        break;
            //    case "Date":
            //        toDo = toDo.OrderBy(t => t.DueDate);
            //        break;
            //    case "Complete":
            //        toDo = toDo.OrderBy(t => t.Complete);
            //        break;
            //    default:
            //        toDo = toDo.OrderBy(t => t.Id);
            //        break;

            //}
            //int pageSize = 3;
            //int pageNumber = (page ?? 1);

            //return View(db.TodoItem.ToPagedList(pageNumber, pageSize));
        }

       

        // GET: ToDoItem/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var toDoItem = await _context.TodoItem
                .Include(t => t.IdNetUsersNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (toDoItem == null)
            {
                return NotFound();
            }

            return View(toDoItem);
        }

        // GET: ToDoItem/Create
        public IActionResult Create()
        {
            ViewData["IdNetUsers"] = new SelectList(_context.TodoItem, "Id", "Description");
            return View();
        }

        // POST: ToDoItem/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Description,DueDate,Complete,IdNetUsers")] ToDoItem toDoItem)
        {
            if (ModelState.IsValid)
            {
                _context.Add(toDoItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdNetUsers"] = new SelectList(_context.TodoItem, "Id", "Description", toDoItem.IdNetUsers);
            return View(toDoItem);
        }

        // GET: ToDoItem/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var toDoItem = await _context.TodoItem.FindAsync(id);
            if (toDoItem == null)
            {
                return NotFound();
            }
            ViewData["IdNetUsers"] = new SelectList(_context.TodoItem, "Id", "Description", toDoItem.IdNetUsers);
            return View(toDoItem);
        }

        // POST: ToDoItem/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Description,DueDate,Complete,IdNetUsers")] ToDoItem toDoItem)
        {
            if (id != toDoItem.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(toDoItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ToDoItemExists(toDoItem.Id))
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
            ViewData["IdNetUsers"] = new SelectList(_context.TodoItem, "Id", "Description", toDoItem.IdNetUsers);
            return View(toDoItem);
        }

        // GET: ToDoItem/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var toDoItem = await _context.TodoItem
                .Include(t => t.IdNetUsersNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (toDoItem == null)
            {
                return NotFound();
            }

            return View(toDoItem);
        }

        // POST: ToDoItem/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var toDoItem = await _context.TodoItem.FindAsync(id);
            _context.TodoItem.Remove(toDoItem);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ToDoItemExists(int id)
        {
            return _context.TodoItem.Any(e => e.Id == id);
        }
    }
}
