using Ephyta.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ephyta.ViewModel
{
    public class HomeViewModel
    {
        public IEnumerable<Banner> Banners { get; set; }
        public IEnumerable<Article> Articles { get; set; }
        public IEnumerable<ProductCategory> ProductCategories { get; set; }
        public IEnumerable<ItemProductViewModel> Products { get; set; }
        public IEnumerable<ItemProductViewModel> ComboProducts { get; set; }
        public IEnumerable<ItemProductViewModel> AllProducts { get; set; }
        public IEnumerable<Feedback> Feedbacks { get; set; }
    }
    public class HeaderViewModel
    {
        public IEnumerable<ProductCategory> ProductCategories { get; set; }
        public IEnumerable<ArticleCategory> ArticleCategories { get; set; }

        public IEnumerable<Product> Products { get; set; }
    }
    public class FooterViewModel
    {
        public IEnumerable<Article> Articles { get; set; }
        public IEnumerable<ArticleCategory> ArticleCategories { get;set; }
    }
    public class CategoryProductViewModel
    {
        public ProductCategory Category { get; set; }
        public IPagedList<ItemProductViewModel> Products { get; set; }
        public IEnumerable<ProductCategory> Categories { get; set; }
        public int CatId { get; set; }
        public string Sort { get; set; }

        public int? ParentId { get; set; }
    }
    public class ArticleDetailViewModel
    {
        public Article Article { get; set; }
        public IEnumerable<Article> Articles { get; set; }
    }
    public class ProductDetailViewModel
    {
        public Product Product { get; set; }
        public IEnumerable<ItemProductViewModel> Products { get; set; }

        public IEnumerable<ReviewKol> ReviewKols { get; set; }

        //public IEnumerable<Review> Reviews { get; set; }
        public IEnumerable<ItemProductViewModel> ProductHots { get; set; }

        public decimal Rating { get; set; }


    }
    public class MenuProductViewModel
    {
        public IEnumerable<Product> Products { get; set; }
        public IEnumerable<ProductCategory> ProductCategories { get; set; }

        public int? ParentId { get; set; }
    }
    public class CategoryAticleViewModel
    {
        public ArticleCategory Category { get; set; }
        public IPagedList<Article> Articles { get; set; }
        public IEnumerable<ArticleCategory> Categories { get; set; }
        public int CatId { get; set; }
    }
    public class MenuArticleViewModel
    {
        public IEnumerable<Article> Articles { get; set; }
        public IEnumerable<ArticleCategory> ArticleCategories { get; set; }
        public IEnumerable<Article> TopViews { get; set; }
    }
    public class SearchViewModel
    {
        public string Keywords { get; set; }
        public IPagedList<Product> Products { get; set; }
    }

    public class CommentFormViewModel
    {
        public Review Review { get; set; }
        public Product Product { get; set; }
    }

    public class GetReviewViewModel 
    {
        public IPagedList<Review> Reviews { get; set; }
        public int Id { get; set; }
    }


    public class ItemProductViewModel
    {
        public Product Product { get; set; }
        public decimal Rating { get; set; }

    }
}