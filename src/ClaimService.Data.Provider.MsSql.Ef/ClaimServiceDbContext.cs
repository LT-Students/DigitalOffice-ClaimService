using Microsoft.EntityFrameworkCore;

namespace ClaimService.Data.Provider.MsSql.Ef;

public class ClaimServiceDbContext : DbContext, IDataProvider
{
	public ClaimServiceDbContext(DbContextOptions<ClaimServiceDbContext> options)
		: base(options)
	{
	}
}
