namespace ShopApi.Data;

using Interface;
using Microsoft.EntityFrameworkCore;
using Repository;

public interface IUnitOfWork
{
    IBrandRepository BrandRepository { get; set; }
    IProductRepository ProductRepository { get; }
    ICategoryRepository CategoryRepository { get; }
    ICommentRepository CommentRepository { get; }
    IFilterRepository FilterRepository { get; }
    IMainCategoryRepository MainCategoryRepository { get; }
    IFilterValueRepository FilterValueRepository { get; }
    Task<bool> CommitAsync();
}
public class UnitOfWork :IUnitOfWork{
    readonly AppDbContext _context;
    public IBrandRepository BrandRepository { get; set; }
    public IProductRepository ProductRepository { get; }
    public ICategoryRepository CategoryRepository { get; }
    public ICommentRepository CommentRepository { get; }
    public IFilterRepository FilterRepository { get; }
    public IMainCategoryRepository MainCategoryRepository { get; }
    public IFilterValueRepository FilterValueRepository { get; }
    public UnitOfWork(AppDbContext context, IProductRepository productRepository,ICommentRepository commentRepository,
        ICategoryRepository categoryRepository, IMainCategoryRepository mainCategoryRepository, IFilterRepository filterRepository,
        IFilterValueRepository filterValueRepository, IBrandRepository brandRepository) {
        _context = context;
        ProductRepository = productRepository;
        CategoryRepository = categoryRepository;
        MainCategoryRepository = mainCategoryRepository;
        FilterRepository = filterRepository;
        FilterValueRepository = filterValueRepository;
        BrandRepository = brandRepository;
        CommentRepository = commentRepository;
    }

    public async Task<bool> CommitAsync() {
        try
        {
            return await _context.SaveChangesAsync() > 0;
        }
        catch (DbUpdateException ex)
        {
            Console.WriteLine($"Error saving changes: {ex.Message}");
            if (ex.InnerException != null)
            {
                Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
            }
        }
        return false;
    }
  
}

