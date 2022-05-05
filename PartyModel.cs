using System;
using System.Collections.Generic;

namespace Snippet.Models.Domain.Parties
{
	public class Party
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Code { get; set; }
		public string Logo { get; set; }
		public string SiteUrl { get; set; }
		public string ColorHEX { get; set; }
		public int StatusId { get; set; }
		public string StatusName { get; set; }
		public int RegionTypeId { get; set; }
		public string RegionName { get; set; }
		public int LocationId { get; set; }
		public string LocationLineOne { get; set; }
		public bool IsCoalition { get; set; }
		public DateTime RegistrationDate { get; set; }
		public List<PartyBase> Members { get; set; }
	}
}
