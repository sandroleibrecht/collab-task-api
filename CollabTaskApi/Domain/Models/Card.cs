﻿namespace CollabTaskApi.Domain.Models
{
	public class Card
	{
		public int Id { get; set; }
		public string Name { get; set; } = string.Empty;
		public string? Description { get; set; }
		public int Order {  get; set; }
		public DateTime CreatedAt { get; set; }
	}
}
