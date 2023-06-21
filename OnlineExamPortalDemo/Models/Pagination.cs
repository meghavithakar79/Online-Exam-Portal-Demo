using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineExamPortalDemo.Models
{
	public class Pagination
	{
		private const int maxPageSize = 10;

		[NotMapped]
		public int PageNumber { get; set; } = 1;

		private int _pageSize = 1;


		[NotMapped]
		public int PageSize
		{
			get => _pageSize;
			set => _pageSize = value > maxPageSize ? maxPageSize : value;
		}
	}
}
