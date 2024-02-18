namespace Library.Services
{
    using Library.Contracts;
    using Library.Data;
    using Library.Data.Models;
    using Library.Models;
    using Microsoft.EntityFrameworkCore;

    public class BookService : IBookService
    {
        private readonly LibraryDbContext dbContext;

        public BookService(LibraryDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<IEnumerable<AllBookViewModel>> GetAllBooksAsync()
        {
            return await dbContext.Books
                .AsNoTracking()
                .Select(b => new AllBookViewModel()
                {
                    Id = b.Id,
                    Title = b.Title,
                    Author = b.Author,
                    ImageUrl = b.ImageUrl,
                    Rating = b.Rating,
                    Category = b.Category.Name
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<MineBookViewModel>> GetMineBooksAsync(string userId)
        {
            return await dbContext.IdentityUsersBooks
                .AsNoTracking()
                .Where(iub => iub.CollectorId == userId)
                .Select(iub => new MineBookViewModel()
                 {
                     Id = iub.Book.Id,
                     Title = iub.Book.Title,
                     Author = iub.Book.Author,
                     ImageUrl = iub.Book.ImageUrl,
                     Description = iub.Book.Description,
                     Category = iub.Book.Category.Name
                 })
                .ToListAsync();
        }

        public async Task<BookViewModel?> GetBookByIdAsync(int id)
        {
            return await dbContext.Books
                .AsNoTracking()
                .Where(b => b.Id == id)
                .Select(b => new BookViewModel()
                {
                    Id = b.Id,
                    Title = b.Title,
                    Author = b.Author,
                    ImageUrl = b.ImageUrl,
                    Description = b.Description,
                    Rating = b.Rating,
                    CategoryId = b.CategoryId
                })
                .FirstOrDefaultAsync();

            throw new NotImplementedException();
        }

        public async Task AddBookToCollectionAsync(string userId, BookViewModel book)
        {
            bool alreadyAdded = await dbContext.IdentityUsersBooks
                .AnyAsync(iub => iub.CollectorId == userId && iub.BookId == book.Id);

            if (!alreadyAdded)
            {
                var userBook = new IdentityUserBook()
                {
                    CollectorId = userId,
                    BookId = book.Id
                };

                await dbContext.IdentityUsersBooks.AddAsync(userBook);
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task RemoveBookFromCollectionAsync(string userId, BookViewModel book)
        {
            var userBook = new IdentityUserBook()
            {
                CollectorId = userId,
                BookId = book.Id
            };

            dbContext.IdentityUsersBooks.Remove(userBook);
            await dbContext.SaveChangesAsync();
        }

        public async Task<AddBookViewModel> GetNewAddBookModelAsync()
        {
            var categories = await dbContext.Categories
                .Select(c => new CategoryViewModel()
                {
                    Id = c.Id,
                    Name = c.Name,
                }).ToListAsync();

            var model = new AddBookViewModel()
            {
                Categories = categories
            };

            return model;
        }

        public async Task AddBookAsync(AddBookViewModel addBook)
        {
            if (!dbContext.Books.Any(b => b.Id == addBook.Id))
            {
                Book book = new Book()
                {
                    Title = addBook.Title,
                    Author = addBook.Author,
                    Description = addBook.Description,
                    ImageUrl = addBook.Url,
                    Rating = addBook.Rating,
                    CategoryId = addBook.CategoryId
                };

                dbContext.Books.Add(book);
                dbContext.SaveChanges();
            }
        }
    }
}