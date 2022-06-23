using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vocabulary.Database
{
    internal class User
    {
        public int Id { get; set; }

        public UserSource Source { get; set; }

        public string SourceId { get; set; }
    }
}
