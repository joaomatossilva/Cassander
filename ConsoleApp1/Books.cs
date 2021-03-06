﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp1
{
    using System.ComponentModel.Design.Serialization;

    public class Books
    {
        public Guid id { get; set; }
        public DateTimeOffset creation_date { get; set; }
        public int stock { get; set; }
        public string name { get; set; }

        public AuthorTitle Authortitle { get; set; }
    }

    public class AuthorTitle
    {
        public string Author { get; set; }
        public string Title { get; set; }
    }
}
