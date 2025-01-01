using Books.Api.Entity;

namespace Books.Api.Services
{
    public interface IBookRepository
    {
        Task<IEnumerable<BookEntity>> GetBooksAsync();
        Task<BookEntity?> GetBookByIdAsync(int bookId);
        Task AddBookAsync(BookEntity book);
        Task<bool> SaveChangesAsync();
        void DeleteBookAsync(BookEntity book);
    }
}
