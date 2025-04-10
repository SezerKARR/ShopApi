namespace ShopApi.Data;

using Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Repository;

public interface IUnitOfWork {
    IProductFilterValueRepository ProductFilterValueRepository { get; }
    IUserRepository UserRepository { get;  }
    IBrandRepository BrandRepository { get; }
    IProductRepository ProductRepository { get; }
    ICategoryRepository CategoryRepository { get; }
    ICommentRepository CommentRepository { get; }
    IFilterRepository FilterRepository { get; }
    IMainCategoryRepository MainCategoryRepository { get; }
    IFilterValueRepository FilterValueRepository { get; }
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
    public ICommentRepository CommentRepository { get; }
    public IFilterRepository FilterRepository { get; }
    public IMainCategoryRepository MainCategoryRepository { get; }
    public IFilterValueRepository FilterValueRepository { get; }
    private IDbContextTransaction? _transaction;
    public UnitOfWork(AppDbContext context, IProductRepository productRepository, ICommentRepository commentRepository,
        ICategoryRepository categoryRepository, IMainCategoryRepository mainCategoryRepository, IFilterRepository filterRepository,
        IFilterValueRepository filterValueRepository, IBrandRepository brandRepository, IUserRepository userRepository, IProductFilterValueRepository productFilterValueRepository) {
        _context = context;
        ProductRepository = productRepository;
        CategoryRepository = categoryRepository;
        MainCategoryRepository = mainCategoryRepository;
        FilterRepository = filterRepository;
        FilterValueRepository = filterValueRepository;
        BrandRepository = brandRepository;
        UserRepository = userRepository;
        ProductFilterValueRepository = productFilterValueRepository;
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