using Ephyta.Models;
using System;

namespace Ephyta.DAL
{
    public class UnitOfWork : IDisposable
    {
        private readonly DataEntities _context = new DataEntities();
        private GenericRepository<Admin> _adminRepository;
        private GenericRepository<ArticleCategory> _artcategoryRepository;
        private GenericRepository<Article> _articleRepository;
        private GenericRepository<Banner> _bannerRepository;
        private GenericRepository<Contact> _contactRepository;
        private GenericRepository<ConfigSite> _configRepository;
        private GenericRepository<Feedback> _feedbackRepository;
        private GenericRepository<Product> _productRepository;
        private GenericRepository<ProductCategory> _procategoryRepository;
        private GenericRepository<Video> _videoRepository;
        private GenericRepository<Register> _registerRepository;
        private GenericRepository<Review> _reviewRepository;
        private GenericRepository<Cart> _cartRepository;
        private GenericRepository<Order> _orderRepository;
        private GenericRepository<OrderDetail> _orderdetailRepository;
        private GenericRepository<City> _cityRepository;
        private GenericRepository<District> _districtRepository;
        private GenericRepository<Ward> _wardRepository;
        private GenericRepository<ReviewKol> _reviewkolRepository;
        private GenericRepository<DiscountCode> _discountcodeRepository;


        public GenericRepository<DiscountCode> DiscountCodeRepository => _discountcodeRepository ?? (_discountcodeRepository = new GenericRepository<DiscountCode>(_context));

        public GenericRepository<ReviewKol> ReviewKolRepository => _reviewkolRepository ?? (_reviewkolRepository = new GenericRepository<ReviewKol>(_context));

        public GenericRepository<City> CityRepository =>
           _cityRepository ?? (_cityRepository = new GenericRepository<City>(_context));

        public GenericRepository<District> DistrictRepository =>
            _districtRepository ?? (_districtRepository = new GenericRepository<District>(_context));

        public GenericRepository<Ward> WardRepository =>
            _wardRepository ?? (_wardRepository = new GenericRepository<Ward>(_context));
        public GenericRepository<Cart> CartRepository => _cartRepository ?? (_cartRepository = new GenericRepository<Cart>(_context));
        public GenericRepository<Order> OrderRepository => _orderRepository ?? (_orderRepository = new GenericRepository<Order>(_context));
        public GenericRepository<OrderDetail> OrderDetailRepository => _orderdetailRepository ?? (_orderdetailRepository = new GenericRepository<OrderDetail>(_context));
        public GenericRepository<Review> ReviewRepository => _reviewRepository ?? (_reviewRepository = new GenericRepository<Review>(_context));
        public GenericRepository<Register> RegisterRepository => _registerRepository ?? (_registerRepository = new GenericRepository<Register>(_context));
        public GenericRepository<Video> VideoRepository => _videoRepository ?? (_videoRepository = new GenericRepository<Video>(_context));
        public GenericRepository<Product> ProductRepository => _productRepository ?? (_productRepository = new GenericRepository<Product>(_context));
        public GenericRepository<ProductCategory> ProductCategoryRepository => _procategoryRepository ?? (_procategoryRepository = new GenericRepository<ProductCategory>(_context));
        public GenericRepository<ConfigSite> ConfigSiteRepository => _configRepository ?? (_configRepository = new GenericRepository<ConfigSite>(_context));
        public GenericRepository<Feedback> FeedbackRepository => _feedbackRepository ?? (_feedbackRepository = new GenericRepository<Feedback>(_context));
        public GenericRepository<Contact> ContactRepository => _contactRepository ?? (_contactRepository = new GenericRepository<Contact>(_context));
        public GenericRepository<Banner> BannerRepository => _bannerRepository ?? (_bannerRepository = new GenericRepository<Banner>(_context));
        public GenericRepository<Article> ArticleRepository => _articleRepository ?? (_articleRepository = new GenericRepository<Article>(_context));
        public GenericRepository<ArticleCategory> ArticleCategoryRepository => _artcategoryRepository ?? (_artcategoryRepository = new GenericRepository<ArticleCategory>(_context));
        public GenericRepository<Admin> AdminRepository => _adminRepository ?? (_adminRepository = new GenericRepository<Admin>(_context));
        public void Save()
        {
            _context.SaveChanges();
        }
        private bool _disposed;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}