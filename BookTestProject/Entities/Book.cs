﻿using System.ComponentModel.DataAnnotations.Schema;

namespace BookTestProject.Entities{
    public class Book{
        public int Id { get; set; }

        public string Name { get; set; }

        public Author Author { get; set; }

        public string Isbn { get; set; }
    }
}