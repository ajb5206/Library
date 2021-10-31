using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Library.Models;
using System.Collections.Generic;
using System.Linq;

namespace Library.Controllers
{
	public class BooksController : Controller
	{
		private readonly LibraryContext _db;

		public BooksController(LibraryContext db)
		{
			_db = db;
		}

		public ActionResult Index()
		{
			return View(_db.Books.ToList());
		}

		public ActionResult Create()
		{
			ViewBag.AuthorId = new SelectList(_db.Authors, "AuthorId", "Name");
			return View();
		}

		[HttpPost]
		public ActionResult Create(Book book, int AuthorId)
		{
			_db.Books.Add(book);
			_db.SaveChanges();
			if (AuthorId !=0 )
			{
				_db.AuthorBook.Add(new AuthorBook() { AuthorId = AuthorId, BookId = book.BookId});
			}
			_db.SaveChanges();
			return RedirectToAction("Index");
		}
	}
}