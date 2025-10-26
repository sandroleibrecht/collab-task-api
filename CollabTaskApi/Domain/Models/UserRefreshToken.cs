﻿namespace CollabTaskApi.Domain.Models
{
	public class UserRefreshToken
	{
		public int Id { get; set; }
		public int UserId { get; set; }
		public string Token { get; set; } = string.Empty;
		public DateTime ExpiresAt { get; set; }
		public DateTime CreatedAt { get; set; }

		public User User { get; set; } = null!;
	}
}
