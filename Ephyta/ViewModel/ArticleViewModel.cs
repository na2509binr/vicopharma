using Ephyta.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ephyta.ViewModel
{
    public class ListArticleViewModel
    {
        public PagedList.IPagedList<Article> Articles { get; set; }
        public SelectList SelectCategories { get; set; }
        public int? CategoryId { get; set; }
        public string Name { get; set; }
    }
    public class InsertArticleViewModel
    {
        public Article Article { get; set; }
        public IEnumerable<ArticleCategory> Categories { get; set; }
        public SelectList SelectCategories { get; set; }
        public int? CategoryId { get; set; }
    }
}