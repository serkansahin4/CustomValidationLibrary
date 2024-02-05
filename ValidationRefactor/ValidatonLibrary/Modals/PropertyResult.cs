using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ValidatonLibrary.Modals
{
    public class PropertyResult
    {
        public string PropertyName { get; set; }
        public bool IsValid { get; set; } = true;
        public string GlobalErrorMessage { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
    }
}
