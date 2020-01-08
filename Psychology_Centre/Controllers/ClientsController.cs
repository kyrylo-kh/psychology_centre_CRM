﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Psychology_Centre.Data;
using Psychology_Centre.Models;
using Psychology_Centre.ViewModels;

namespace Psychology_Centre.Controllers
{
    public class ClientsController : Controller
    {
        private readonly ApplicationContext _context;

        public ClientsController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: Clients
        public async Task<IActionResult> Index(string searchSurname, string searchEmail, int page = 1, 
            SortState sortOrder = SortState.SurnameAsc)
        {
            int pageSize = 3;

            // Фильтрация
            IQueryable<Client> clients = from c in _context.Clients
                                         select c;

            if (!String.IsNullOrEmpty(searchSurname))
            {
                clients = clients.Where(s => s.Surname.Contains(searchSurname));
            }

            if (!String.IsNullOrEmpty(searchEmail))
            {
                clients = clients.Where(s => s.Email.Contains(searchEmail));
            }

            // Сортировка
            switch (sortOrder)
            {
                case SortState.SurnameDesk:
                    clients = clients.OrderByDescending(s => s.Surname);
                    break;
                case SortState.SurnameAsc:
                    clients = clients.OrderBy(s => s.Surname);
                    break;
                case SortState.EmailDesc:
                    clients = clients.OrderByDescending(s => s.Email);
                    break;
                case SortState.EmailAsc:
                    clients = clients.OrderBy(s => s.Email);
                    break;
                default:
                    clients = clients.OrderBy(s => s.Surname);
                    break;
            }

            // Пагинация
            var count = await clients.CountAsync();
            var items = await clients.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            // Модель представления
            IndexViewModel viewModel = new IndexViewModel
            {
                PageViewModel = new PageViewModel(count, page, pageSize),
                SortViewModel = new SortViewModel(sortOrder),
                FilterViewModel = new FilterViewModel(searchSurname, searchEmail),
                Clients = items
            };

            return View(viewModel);
        }

        // GET: Clients/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = await _context.Clients
                .FirstOrDefaultAsync(m => m.Id == id);
            if (client == null)
            {
                return NotFound();
            }

            return View(client);
        }

        // GET: Clients/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Clients/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Surname,Midname,IsSpecial,Email,PhoneNumberFirst,PhoneNumberSecond,PhoneNumberUnformat,PhoneNumberOther,IdFacebook,ComfyCallTime,Gender,Birthday,Age,FamilyStatus,IsHaveChildren,Status,Source,FieldOfActivity,Post,City,Region,Metro,SelectionFactors,Activity")] Client client)
        {
            if (ModelState.IsValid)
            {
                _context.Add(client);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(client);
        }

        // GET: Clients/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = await _context.Clients.FindAsync(id);
            if (client == null)
            {
                return NotFound();
            }
            return View(client);
        }

        // POST: Clients/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Surname,Midname,IsSpecial,Email,PhoneNumberFirst,PhoneNumberSecond,PhoneNumberUnformat,PhoneNumberOther,IdFacebook,ComfyCallTime,Gender,Birthday,Age,FamilyStatus,IsHaveChildren,Status,Source,FieldOfActivity,Post,City,Region,Metro,SelectionFactors,Activity")] Client client)
        {
            if (id != client.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(client);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClientExists(client.Id))
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
            return View(client);
        }

        // GET: Clients/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = await _context.Clients
                .FirstOrDefaultAsync(m => m.Id == id);
            if (client == null)
            {
                return NotFound();
            }

            return View(client);
        }

        // POST: Clients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var client = await _context.Clients.FindAsync(id);
            _context.Clients.Remove(client);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClientExists(int id)
        {
            return _context.Clients.Any(e => e.Id == id);
        }
    }
}
