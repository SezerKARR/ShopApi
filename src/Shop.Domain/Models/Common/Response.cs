namespace Shop.Domain.Models.Common;

public record Response<T>
 {
     public bool Success { get; }
     public string? Message { get; }
     public T? Resource { get; }
 
     public Response(T resource)
     {
         Success = true;
         Message = null;
         Resource = resource ?? throw new ArgumentNullException(nameof(resource));
     }
 
     public Response(string message)
     {
         Success = false;
         Message = message;
         Resource = default; // Success=false olduğunda Resource kullanılamaz
     }
 }