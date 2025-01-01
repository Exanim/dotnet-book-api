using Books.Api.DbContexts;
using Books.Api.Entity;
using Microsoft.EntityFrameworkCore;

namespace Books.Api.Services
{
    public class BookRepository : IBookRepository
    {
        private readonly BooksContext _context;

        public BookRepository(BooksContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task<IEnumerable<BookEntity>> GetBooksAsync()
        {
            return await _context.Books.ToListAsync();
        }
        public async Task<BookEntity?> GetBookByIdAsync(int bookId)
        {
            return await _context.Books.Where(x => x.Id == bookId).FirstOrDefaultAsync();
        }
        public async Task AddBookAsync(BookEntity book)
        {
            await _context.Books.AddAsync(book);
        }
        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync() >=0);
        }

        public void DeleteBookAsync(BookEntity book)
        {
            _context.Books.Remove(book);
        }
    }
}
