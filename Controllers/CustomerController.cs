using Microsoft.AspNetCore.Mvc;
using LibraryManagement.Models;
using Microsoft.EntityFrameworkCore;
using LibraryManagement.Data;
using Microsoft.AspNetCore.Authorization;

namespace LibraryManagement.Controllers
{
    [Authorize]
    public class CustomerController : Controller
    {
        private readonly AppDbContext _context;

        // Constructor to inject the database context
        public CustomerController(AppDbContext dbcontext)
        {

            _context = dbcontext;
        }
        public IActionResult Details()
        {
            var customers = _context.Customers.ToList();
            return View(customers);
        }
        public IActionResult Edit(int id)
        {
            var customer = _context.Customers.FirstOrDefault(e => e.CustomerId == id);
            return View(customer);
        }

        [HttpPost]
        public IActionResult Edit(Customer customer)
        {
            if (ModelState.IsValid)
            {
                var customerToUpdate = _context.Customers.FirstOrDefault(e => e.CustomerId == customer.CustomerId);
                if (customerToUpdate != null)
                {
                    customerToUpdate.Name = customer.Name;
                    _context.SaveChanges();
                }
            }
            return RedirectToAction(nameof(Details));
        }

        public IActionResult Delete(int id)
        {
            if (id != null)
            {
                var customerToUpdate = _context.Customers.FirstOrDefault(e => e.CustomerId == id);
                if (customerToUpdate != null)
                {
                    _context.Customers.Remove(customerToUpdate);
                    _context.SaveChanges();
                }
            }
            return RedirectToAction(nameof(Details));
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(string name)
        {
            if (name == null) return View();
            var maxId = 0;
            if (_context.Customers.Any()) {
                maxId = _context.Customers.Max(e => e.CustomerId);
            }
            var customer = new Customer
            {
                CustomerId = maxId + 1,
                Name = name
            };
            _context.Customers.Add(customer);
            _context.SaveChanges();

            return RedirectToAction(nameof(Details));
        }
    }
}
