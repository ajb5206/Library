using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Library.Models;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Security.Claims;

namespace Library.Controllers
{
	[Authorize]
	public class BooksController : Controller
	{
		
		private readonly LibraryContext _db;
		private readonly UserManager <ApplicationUser> _userManager;
		public BooksController(UserManager <ApplicationUser> userManager, LibraryContext db)
		{
			_userManager = userManager;
			_db = db;
		}
		[AllowAnonymous]
		public ActionResult Index(string searchString)
		{
			IQueryable<Book> model = _db.Books;
			if(!string.IsNullOrEmpty(searchString))
			{
				model = model.Where(model => model.Title.Contains(searchString));
			}
			return View(model.ToList());
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

		public ActionResult Edit(int id)
		{
			var thisBook = _db.Books.FirstOrDefault(book => book.BookId == id);
			return View(thisBook);
		}

		[HttpPost]
		public ActionResult Edit(Book book, int AuthorId)
		{
			_db.Entry(book).State = EntityState.Modified;
			_db.SaveChanges();
			return RedirectToAction("Index");
		}
		[AllowAnonymous]
		public ActionResult Details(int id)
		{
			var thisBook = _db.Books
				.Include(book => book.JoinEntities)
				.ThenInclude(join => join.Author)
				.FirstOrDefault(book => book.BookId ==id);
			return View(thisBook);
		}

		public ActionResult AddAuthor(int id)
		{
			var thisBook = _db.Books.FirstOrDefault(book => book.BookId == id);
			ViewBag.AuthorId = new SelectList(_db.Authors, "AuthorId", "Name");
			return View(thisBook);
		}

		[HttpPost]
		public ActionResult AddAuthor(Book book, int AuthorId)
		{	
			if (AuthorId != 0)
			{
				if (_db.AuthorBook.Any(x => x.AuthorId == AuthorId && x.BookId == book.BookId) == false)
			{
				_db.AuthorBook.Add(new AuthorBook() { AuthorId = AuthorId, BookId = book.BookId});
			}
			}
			_db.SaveChanges();
			return RedirectToAction("Index");
		}

		public ActionResult Delete(int id)
		{
			var thisBook = _db.Books.FirstOrDefault(book => book.BookId ==id );
			return View(thisBook);
		}

		[HttpPost, ActionName("Delete")]
		public ActionResult DeleteConfirmed(int id)
		{
			var thisBook = _db.Books.FirstOrDefault( book => book.BookId == id );
			_db.Books.Remove(thisBook);
			_db.SaveChanges();
			return RedirectToAction("Index");
		}

		[HttpPost]
		public ActionResult DeleteAuthor(int joinId)
		{
			var joinEntry = _db.AuthorBook.FirstOrDefault(entry => entry.AuthorBookId == joinId );
			_db.AuthorBook.Remove(joinEntry);
			_db.SaveChanges();
			return RedirectToAction("Index");
		}
	}
}