using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;

namespace Anime_Studio.Models.ViewModels
{
    public class MangaVM
    {
        public Manga Manga { get; set; }
        public IEnumerable<SelectListItem> CategoryList { get; set; } //in order to use the drop down for category we need to install Microsoft.AspNetCore.Mvc.ViewFeatures
    }
}
