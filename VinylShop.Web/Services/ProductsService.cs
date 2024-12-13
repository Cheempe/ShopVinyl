using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using VinylShop.Web.Data;
using VinylShop.Web.Data.Entities;
using VinylShop.Web.Extensions;
using VinylShop.Web.Models;
using VinylShop.Web.Models.Product;
using static System.Net.Mime.MediaTypeNames;

namespace VinylShop.Web.Services
{
    public class ProductsService(AppDbContext context) : IProductsService
    {
        private readonly AppDbContext _context = context;

        public async Task AddAsync(CreateProductModel model, CancellationToken cancellationToken = default)
        {
            byte[] coverBytes;
            using (var ms = new MemoryStream())
            {
                await model.Cover.CopyToAsync(ms);
                coverBytes = ms.ToArray();
            }

            VinylEntity vinyl = new()
            {
                Name = model.Name,
                Author = model.Author,
                Year = model.Year,
                GenreId = model.GenreId.Value,
                RecordLabel = model.RecordLabel,
                Description = model.Description,
                Price = model.Price.Value,
                Cover = coverBytes
            };

            await _context.Vinyls.AddAsync(vinyl, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            VinylEntity? item = await _context.Vinyls.FirstOrDefaultAsync(v => v.Id == id, cancellationToken);
            if (item == null) return;
            _context.Vinyls.Remove(item);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<List<LiteProductModel>> GetAllAsync(
            int skip = 0,
            int take = 12,
            List<string>? selectedGenres = null,
            string? name = null,
            decimal? minPrice = null,
            decimal? maxPrice = null,
            CancellationToken cancellationToken = default)
        {
            IQueryable<VinylEntity> vinyls = _context.Vinyls.AsQueryable();
            if (selectedGenres is not null && selectedGenres.Count > 0)
                vinyls = vinyls.Where(v => selectedGenres.Contains(v.Genre.Name));

            if (!string.IsNullOrEmpty(name))
                vinyls = vinyls.Where(v => v.Name.ToLower().Contains(name.ToLower()));

            if (minPrice is not null)
                vinyls = vinyls.Where(v => v.Price >= minPrice);

            if (maxPrice is not null)
                vinyls = vinyls.Where(v => v.Price <= maxPrice);

            List<LiteProductModel> result = await vinyls
                .Skip(skip)
                .Take(take)
                .Select(v => new LiteProductModel()
                {
                    Id = v.Id,
                    Name = v.Name,
                    Author = v.Author,
                    Genre = v.Genre.Name,
                    Price = v.Price

                }).ToListAsync(cancellationToken);

            return result;
        }

        public async Task<ProductModel?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            VinylEntity? item = await _context.Vinyls
                .Include(v => v.Genre)
                .FirstOrDefaultAsync(v => v.Id == id, cancellationToken);

            return item?.ToViewModel();
        }

        public async Task<int> GetCountAsync(
            List<string>? selectedGenres = null, 
            string? name = null, 
            decimal? minPrice = null,
            decimal? maxPrice = null, 
            CancellationToken cancellationToken = default)
        {
            IQueryable<VinylEntity> vinyls = _context.Vinyls.AsQueryable();
            if (selectedGenres is not null && selectedGenres.Count > 0)
                vinyls = vinyls.Where(v => selectedGenres.Contains(v.Genre.Name));

            if (!string.IsNullOrEmpty(name))
                vinyls = vinyls.Where(v => v.Name.ToLower().Contains(name.ToLower()));

            if (minPrice is not null)
                vinyls = vinyls.Where(v => v.Price >= minPrice);

            if (maxPrice is not null)
                vinyls = vinyls.Where(v => v.Price <= maxPrice);

            return await vinyls.CountAsync(cancellationToken);
        }

        public async Task<ImageModel> GetProductImageAsync(Guid productId, CancellationToken cancellationToken = default)
        {
            VinylEntity? item = await _context.Vinyls
                .FirstOrDefaultAsync(v => v.Id == productId, cancellationToken);

            return new ImageModel() { Id = productId, Content = item.Cover };
        }

        public Task<ProductModel> UpdateAsync(UpdateProductModel model, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
