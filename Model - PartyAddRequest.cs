using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Snippet.Models.Requests.Parties
{
	public class PartyAddRequest
	{
		[Required]
		[StringLength(200, MinimumLength = 2)]
		public string Name { get; set; }
		[StringLength(50, MinimumLength = 2)]
		public string Code { get; set; }
		[StringLength(255, MinimumLength = 2)]
		public string Logo { get; set; }
		[StringLength(255, MinimumLength = 2)]
		public string SiteUrl { get; set; }
		[StringLength(7, MinimumLength = 7)]
		public string ColorHEX { get; set; }
		[Required]
		[Range(1, int.MaxValue)]
		public int StatusId { get; set; }
		[Required]
		[Range(1, int.MaxValue)]
		public int RegionTypeId { get; set; }
		[Range(1, int.MaxValue)]
		public int LocationId { get; set; }
		[Required]
		public bool IsCoalition { get; set; }
		[Required]
		public List<int> PartyBase { get; set; }
	}
}
