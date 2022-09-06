using Ephyta.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ephyta.ViewModel
{
    public class ListReviewKolViewModel
    {
        public PagedList.IPagedList<ReviewKol> ReviewKols { get; set; }
        public SelectList SelectProducts { get; set; }
        public int? ProductId { get; set; }
        public string Name { get; set; }
        public IEnumerable<Product> Products { get; set; }

    }

    public class InsertReviewKolViewModel
    {
        public ReviewKol ReviewKol { get; set; }
        public IEnumerable<Product> Products { get; set; }
        public SelectList SelectProducts { get; set; }


        public int? ProductId { get; set; }
    }
    public class InsertReviewViewModel
    {
        public Review Review { get; set; }
        public IEnumerable<Product> Products { get; set; }
        public SelectList SelectProducts { get; set; }


        public int? ProductId { get; set; }
    }

    public class ListReviewViewModel
    {
        public PagedList.IPagedList<Review> Reviews { get; set; }
        public SelectList SelectProducts { get; set; }
        public int? ProductId { get; set; }
        public string Name { get; set; }

        public IEnumerable<Product> Products { get; set; }
    }
}