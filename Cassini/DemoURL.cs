using System;
using System.Collections.Generic;

namespace Cassini
{
	public struct DemoURL
	{
		public const string Stanford = "http://comm.stanford.edu/wp-content/uploads/2013/01/stanford-campus.png";


			public static Dictionary<string, string> NASA = new Dictionary<string, string> {
			{"Cassini", "http://www.jpl.nasa.gov/images/cassini/20090202/pia03883-full.jpg"},
			{"Earth", "http://www.nasa.gov/sites/default/files/wave_earth_mosaic_3.jpg"},
			{"Saturn", "http://www.nasa.gov/sites/default/files/saturn_collage.jpg"}

		};


		public static Uri NASAImageNamed(string imageName)
		{
			string urlString;
			if (NASA.TryGetValue(imageName, out urlString))
			{
				return new Uri(urlString);
			}
			return null;
		}
	}
}