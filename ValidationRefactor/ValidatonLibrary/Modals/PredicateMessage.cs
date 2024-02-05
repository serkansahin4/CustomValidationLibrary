using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ValidatonLibrary.Modals
{
    public class PredicateMessages<T>
    {
        public string Message { get; set; }
        public Predicate<T> Predicate { get; set; }
    }
}
