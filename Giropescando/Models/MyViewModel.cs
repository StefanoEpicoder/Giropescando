using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Giropescando.Models
{
    public class MyViewModel
    {
        public LoginViewModel Login { get; set; }
        public List<Cards> Cards { get; set; }
    }
}