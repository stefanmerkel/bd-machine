﻿namespace bd.machine.dal.Implementations.Repositories
{
	using System;
	using Interfaces;
	using Interfaces.Repositories;

	public class FragmentRepository : IFragmentRepository
	{
		private readonly IContext _context;

		public FragmentRepository(IContext context) 
		{
			if (context == null)
				throw new ArgumentNullException("context");

			_context = context;
		}
	}
}
