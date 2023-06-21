using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace OnlineExamPortalDemo.Models
{
	public class PageList<T>:List<T>
	{
		public PageList(List<T> items, int count, int pageNumber, int pageSize)
		{
			Page = new Page
			{
				TotalCount = count,
				PageSize = pageSize,
				CurrentPage = pageNumber,
				TotalPages = (int)Math.Ceiling(count / (double)pageSize)
			};
			AddRange(items);
		}
		public Page Page { get; set; }
		public static async Task<PageList<T>> ToPagedList(IQueryable<T> query, int pageNumber, int pageSize)
		{
			var count = await query.CountAsync();
			var items = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
			return new PageList<T>(items, count, pageNumber, pageSize);
		}
	}
}
