﻿using System.ComponentModel.DataAnnotations;

namespace Anime_Studio.Models
{
    public class Manga
    {
        [Key] public int Id { get; set; }

        [Required] public string Title { get; set; }

        [Required] public string Author { get; set; }

        [Required] public string Artist { get; set; }

        [Required] [MaxLength(20)] public string ISBN { get; set; }
        public string PublishingCompany { get; set; }
        public string Price { get; set; }

        public string Cover { get; set; }
    }
}