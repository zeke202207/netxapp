using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetX.RBAC
{
    internal class Persional
    {
        private static Lazy<Persional> _instance = new Lazy<Persional>(() => new Persional());

        public static Persional Instance => _instance.Value;

        private Persional()
        {
        }

        public string JwtToken { get; set; }
    }
}
