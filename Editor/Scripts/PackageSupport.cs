using System.Collections.Generic;
using System.Linq;
using UnityEditor.PackageManager;

namespace Rosalina {

	public static class PackageSupport {

		private const string PACKAGES_DIRECTORY = "Packages";

		private static readonly Dictionary<string, PackageSource> cache = new Dictionary<string, PackageSource>();

		static PackageSupport() {
			cache.Clear();
			
			var listRequest = Client.List(true, true);
			while (!listRequest.IsCompleted) {
				//block
			}

			foreach (var packageInfo in listRequest.Result.Where(info => info != null)) {
				cache.Add(packageInfo.name, packageInfo.source);
			}
		}
		
		public static bool IsFileInPackage(string path) {
			return path.StartsWith(PACKAGES_DIRECTORY);
		}

		public static bool IsPackageEmbedded(string path) {
			var split = path.Split('/');
			if (split[0] != PACKAGES_DIRECTORY) {
				return false;
			}

			var packageName = split[1];
			return GetPackageSource(packageName) == PackageSource.Embedded;
		}

		private static PackageSource GetPackageSource(string packageName) => cache[packageName];

	}

}