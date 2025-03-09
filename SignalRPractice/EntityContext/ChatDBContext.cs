using Microsoft.EntityFrameworkCore;

using SignalRPractice.Entities;

namespace SignalRPractice.EntityContext
{
	public class ChatDBContext(DbContextOptions dbContextOptions) : DbContext(dbContextOptions)
	{
		public DbSet<User> Users { get; set; }
		public DbSet<Token> Tokens { get; set; }
	}
}
