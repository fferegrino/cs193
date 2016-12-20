using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Smashtag
{
	public class RecentSearches 
	{

		Queue<string> _searches; 
		public RecentSearches() 
		{
			_searches = new Queue<string>(100);
		}

		public void AddSearchTerm(string recentSearches)
		{
			if (_searches.Count >= 100)
				_searches.Dequeue();
			_searches.Enqueue(recentSearches);
		}

		public string[] Recent => _searches.ToArray();

		public string Get() => JsonConvert.SerializeObject( _searches);

		public bool Load(string searches)
		{
			if (String.IsNullOrEmpty(searches)) return false;

			var srch = JsonConvert.DeserializeObject<Queue<string>>(searches);
			if (srch != null)
			{
				_searches.Clear();
				while (srch.Count > 0)
				{
					_searches.Enqueue(srch.Dequeue());
				}
				return true;
			}
			return false;
		}
	}
}
