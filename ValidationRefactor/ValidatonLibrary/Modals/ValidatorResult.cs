using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ValidatonLibrary.Modals
{
    public class ValidatorResult
    {
        public bool IsValid { get; set; } = true;
        public List<PropertyResult> PropertyResults { get; set; } = new List<PropertyResult>();
    }
}
