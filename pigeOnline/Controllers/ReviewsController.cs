#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using pigeOnline.Data;
using pigeOnline.Models;

namespace pigeOnline.Controllers
{
    public class ReviewsController : Controller
    {
        private readonly pigeOnlineContext _context;

        public ReviewsController(pigeOnlineContext context)
        {
            _context = context;
        }

        /*
        * GET : The function return the reviews and average. 
        */
        public async Task<IActionResult> Index()
        {
            List<Review> reviews = await _context.Review.ToListAsync();

            ViewBag.Average = System.Math.Round(reviews.Average(item => item.RateNumber),2);
            return View(reviews);
        }

        /*
        * GET : Reviews/Details/{id} : The function return details of review by id. 
        */
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var review = await _context.Review
                .FirstOrDefaultAsync(m => m.Id == id);
            if (review == null)
            {
                return NotFound();
            }

            return View(review);
        }

        /*
        * GET : Reviews/Create : The function return create page. 
        */
        public IActionResult Create()
        {
            return View();
        }

        /*
        * POST : Reviews/Create : The function create new review. 
        */
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,RateNumber,Text")] Review review)
        {
            review.TimeAndDate = DateTime.Now.ToString();
            if (ModelState.IsValid)

            {
                _context.Add(review);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(review);
        }

        /*
        * GET : Reviews/Edit/{id} : The function return edit page for review by id. 
        */
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var review = await _context.Review.FindAsync(id);
            if (review == null)
            {
                return NotFound();
            }
            return View(review);
        }

        /*
        * POST : Reviews/Edit/{id} : The function edit review by id. 
        */
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,RateNumber,Text,TimeAndDate")] Review review)
        {
            review.TimeAndDate = DateTime.Now.ToString();
            if (id != review.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(review);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReviewExists(review.Id))
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
            return View(review);
        }

        /*
        * GET : Reviews/Delete/{id} : The function return delete page for review by id. 
        */
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var review = await _context.Review
                .FirstOrDefaultAsync(m => m.Id == id);
            if (review == null)
            {
                return NotFound();
            }

            return View(review);
        }

        /*
        * POST : Reviews/Delete/{id} : The function delete review by id. 
        */
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var review = await _context.Review.FindAsync(id);
            _context.Review.Remove(review);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        /*
        * GET : Reviews/Search/{searchVal} : The function return list of search result. 
        */
        [Route("Reviews/Search/{searchVal}")]
        public async Task<List<Review>> Search(string searchVal)
        {
            Console.WriteLine(searchVal);
            List<Review> reviews = await _context.Review.ToListAsync();
            List<Review> filterList = new List<Review> {};
            foreach (Review review in reviews)
            {
                if(review.Name.Contains(searchVal) || review.Text.Contains(searchVal))
                {
                    filterList.Add(review);
                } 
            }
            return filterList;
        }

        private bool ReviewExists(int id)
        {
            return _context.Review.Any(e => e.Id == id);
        }
    }
}
