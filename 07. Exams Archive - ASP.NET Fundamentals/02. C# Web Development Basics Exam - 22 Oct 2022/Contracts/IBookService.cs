namespace Library.Contracts
{
    using Library.Models;

    public interface IBookService
    {
        Task<BookViewModel?> GetBookByIdAsync(int id);
        Task<IEnumerable<AllBookViewModel>> GetAllBooksAsync();
        Task<IEnumerable<MineBookViewModel>> GetMineBooksAsync(string userId);
        Task AddBookToCollectionAsync(string userId, BookViewModel book);
        Task RemoveBookFromCollectionAsync(string userId, BookViewModel book);
        Task<AddBookViewModel> GetNewAddBookModelAsync();
        Task AddBookAsync(AddBookViewModel book);
    }
}