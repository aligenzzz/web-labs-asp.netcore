﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web_153505_Bybko.API.Data;
using Web_153505_Bybko.Domain.Entities;
using Web_153505_Bybko.API.Services.BookService;

namespace Web_153505_Bybko.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBookService _service;

        public BooksController(IBookService service)
        {
            _service = service;
        }

        // GET: api/Books
        [HttpGet]
        [Route("")]
        [Route("{genre}/pageno{pageno:int}/pagesize{pagesize:int}")]
        [Route("{genre}/pageno{pageno:int}")]
        [Route("{genre}/pagesize{pagesize:int}")]
        [Route("pageno{pageno:int}/pagesize{pagesize:int}")]
        [Route("pageno{pageno:int}")]
        [Route("{genre}")]
        public async Task<ActionResult<IEnumerable<Book>>> GetBooks(string? genre = "All", int pageNo = 1, int pageSize = 3)
        {
            var response = await _service.GetBooksListAsync(genre, pageNo, pageSize);

            return Ok(response);
        }

        // GET: api/Books/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Book>> GetBook(int id)
        {
            var response = await _service.GetBookByIdAsync(id);
            if (!response.Success)
                return NotFound();
            var book = response.Data;

            if (book == null)
                return NotFound();

            return Ok(response);
        }

        // PUT: api/Books/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id:int}")]
        public async Task<IActionResult> PutBook(int id, Book book)
        {
            if (id != book.Id)
                return BadRequest();

            try
            {
                await _service.UpdateBookAsync(id, book);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookExists(id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        // POST: api/Books
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Book>> PostBook(Book book)
        {
            await _service.CreateBookAsync(book);

            return CreatedAtAction("GetBook", new { id = book.Id }, book);
        }

        // DELETE: api/Books/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            await _service.DeleteBookAsync(id);

            return NoContent();
        }

        private bool BookExists(int id)
        {
            var response = _service.GetBookByIdAsync(id).Result;
            if (!response.Success || response.Data == null)
                return false;

            return true;
        }
    }
}
