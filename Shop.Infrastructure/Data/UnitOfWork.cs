namespace Shop.Infrastructure.Data;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Repository;

public interface IUnitOfWork {
    IProductFilterValueRepository ProductFilterValueRepository { get; }
    IUserRepository UserRepository { get;  }
    IBrandRepository BrandRepository { get; }
    IProductRepository ProductRepository { get; }
    ICategoryRepository CategoryRepository { get; }
    ISellerRepository SellerRepository { get; }
    ICommentRepository CommentRepository { get; }
    IFilterRepository FilterRepository { get; }
    IMainCategoryRepository MainCategoryRepository { get; }
    IFilterValueRepository FilterValueRepository { get; }
    IProductSellerRepository ProductSellerRepository { get;}
    IStockRepository StockRepository { get;  }
    IImageRepository ImageRepository { get; set; }
    Task BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackAsync();
    Task<bool> CommitAsync();
}

public class UnitOfWork : IUnitOfWork {
    readonly AppDbContext _context;
    public IProductFilterValueRepository ProductFilterValueRepository { get; }
    public IUserRepository UserRepository { get; }
    public IBrandRepository BrandRepository { get; }
    public IProductRepository ProductRepository { get; }
    public ICategoryRepository CategoryRepository { get; }
    public ISellerRepository SellerRepository { get; }
    public ICommentRepository CommentRepository { get; }
    public IFilterRepository FilterRepository { get; }
    public IMainCategoryRepository MainCategoryRepository { get; }
    public IFilterValueRepository FilterValueRepository { get; }
    public IProductSellerRepository ProductSellerRepository { get;  }
    public IStockRepository StockRepository { get; }
    public IImageRepository ImageRepository { get; set; }

    private IDbContextTransaction? _transaction;
    public UnitOfWork(AppDbContext context, IProductRepository productRepository, ICommentRepository commentRepository,
        ICategoryRepository categoryRepository, IMainCategoryRepository mainCategoryRepository, IFilterRepository filterRepository,
        IFilterValueRepository filterValueRepository, IBrandRepository brandRepository, IUserRepository userRepository, IProductFilterValueRepository productFilterValueRepository, IProductSellerRepository productSellerRepository, IStockRepository stockRepository, ISellerRepository sellerRepository, IImageRepository ımageRepository) {
        _context = context;
        ProductRepository = productRepository;
        CategoryRepository = categoryRepository;
        MainCategoryRepository = mainCategoryRepository;
        FilterRepository = filterRepository;
        FilterValueRepository = filterValueRepository;
        BrandRepository = brandRepository;
        UserRepository = userRepository;
        ProductFilterValueRepository = productFilterValueRepository;
        ProductSellerRepository = productSellerRepository;
        StockRepository = stockRepository;
        SellerRepository = sellerRepository;
        ImageRepository = ımageRepository;
        CommentRepository = commentRepository;
    }

    public async Task<bool> CommitAsync() {
        try { return await _context.SaveChangesAsync() > 0; }
        catch (DbUpdateException ex)
        {
            Console.WriteLine($"Error saving changes: {ex.Message}");
            if (ex.InnerException != null) { Console.WriteLine($"Inner Exception: {ex.InnerException.Message}"); }
        }
        return false;
    }
    public async Task BeginTransactionAsync() {
        _transaction = await _context.Database.BeginTransactionAsync();
    }

    // Transaction Commit işlemi
    public async Task CommitTransactionAsync() {
        if (_transaction != null)
        {
            await _context.SaveChangesAsync();
            await _transaction.CommitAsync();
        }
    }

    // Transaction Rollback işlemi
    public async Task RollbackAsync() {
        if (_transaction != null) { await _transaction.RollbackAsync(); }
    }
}