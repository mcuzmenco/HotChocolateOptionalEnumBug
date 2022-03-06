using System.Threading.Tasks;

namespace WebApplication1
{
	public class Query
	{
		public Task<Book> Book(int id)
		{
			return Task.FromResult(new Book { Title = "C# in depth", Author = "Jon Skeet", Id = id });
		}
	}

	public class Mutation
	{
		public Task<Book> AddBook(BookInput input)
		{
			return Task.FromResult(new Book()
			{
				Author = input.Author,
				Title = input.Title,
				Id = 1,
				Genre = input.Genre
			});
		}
	}


	public class Book
	{
		public int Id { get; set; }
		public string Title { get; set; }
		public string Author { get; set; }
		public Genre Genre { get; set; }
	}

	public enum Genre
	{
		Unknown = 0,
		NonFiction = 1,
		Fiction = 2
	}

	public class BookInput
	{
		public string Title { get; set; }
		public string Author { get; set; }

		public Genre Genre { get; set; }

	}
}
