﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartGym.Core.Domain._Identity
{
	public class ApplicationUser : IdentityUser 
	{
		public string FirstName { get; set; } = string.Empty;
		public string LastName { get; set; } = string.Empty;
		public string Gender { get; set; } = string.Empty;
		public DateOnly BirthDay { get; set; }
		public int Height { get; set; }
		public decimal Weight { get; set; }
		public bool IsDisabled { get; set; }
	}
}
