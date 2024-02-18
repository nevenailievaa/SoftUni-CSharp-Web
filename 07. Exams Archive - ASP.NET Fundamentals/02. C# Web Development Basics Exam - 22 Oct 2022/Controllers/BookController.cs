namespace Library.Controllers
{
    using Library.Contracts;
    using Library.Models;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System.Security.Claims;

    [Authorize]
    public class BookController : Controller
    {
        private readonly IBookService bookService;

        public BookController(IBookService bookService)
        {
            this.bookService = bookService;
        }

        public async Task<IActionResult> All()
        {
            var allBooks = await bookService.GetAllBooksAsync();

            return View(allBooks);
        }

        public async Task<IActionResult> Mine()
        {
            var mineBooks = await bookService.GetMineBooksAsync(GetUserById());

            return View(mineBooks);
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            AddBookViewModel model = await bookService.GetNewAddBookModelAsync();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddBookViewModel book)
        {
            await bookService.AddBookAsync(book);
            return RedirectToAction("All");
        }

        private string GetUserById()
        {
            string id = string.Empty;

            if (User != null)
            {
                id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            }

            return id;
        }

        [HttpPost]
        public async Task<IActionResult> AddToCollection(int id)
        {
            var book = await bookService.GetBookByIdAsync(id);

            if (book == null)
            {
                return RedirectToAction(nameof(All));
            }

            var userId = GetUserById();

            await bookService.AddBookToCollectionAsync(userId, book);

            return RedirectToAction(nameof(All));
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFromCollection(int id)
        {
            var book = await bookService.GetBookByIdAsync(id);
            var userId = GetUserById();

            await bookService.RemoveBookFromCollectionAsync(userId, book);

            return RedirectToAction(nameof(Mine));
        }
    }
}