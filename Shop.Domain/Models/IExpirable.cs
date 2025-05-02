namespace Shop.Domain.Models;

public interface IExpirable {
    DateTime ValidUntil { get; set; }
    bool IsExpired => DateTime.UtcNow > ValidUntil;
}