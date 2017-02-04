﻿namespace bd.machine.webApp.ViewModels.Home
{
	using System.Collections.Generic;

	public class HomeIndexViewModel
	{
		public string Url { get; set; }
		
		public IEnumerable<HomeIndexHostViewModel> Hosts { get; set; }
	}
}
