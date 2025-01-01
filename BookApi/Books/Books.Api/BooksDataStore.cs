using Org.OpenAPITools.Models;

namespace Books.Api
{
    public class BooksDataStore
    {
        public List<Book> Books { get; set; }

        public BooksDataStore()
        {
            Books = new List<Book>()
        {
            new Book()
            {
                Id = 1,
                Title = "Test1"
            },

            new Book()
            {
                Id = 2,
                Title = "Test2"
            },

            new Book()
            {
                Id = 3,
                Title = "Test3"
            }
        };
        }
    }
}
