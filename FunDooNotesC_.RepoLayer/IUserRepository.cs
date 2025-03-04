using FunDooNotesC_.DataLayer.Entities; // User entity ko use karne ke liye namespace include kiya gaya hai
using System.Threading.Tasks; // Asynchronous programming support karne ke liye

namespace FunDooNotesC_.RepoLayer
{
    // IUserRepository ek interface hai jo IRepository<User> ko inherit kar raha hai.
    // Iska matlab yeh hai ki yeh repository User entity ke liye specific operations provide karegi.
    public interface IUserRepository : IRepository<User>
    {
        // Yeh method ek specific user ko uske email ke basis pe fetch karega.
        // "Task<User?>" ka matlab hai ki yeh ek asynchronous operation hoga aur agar user na mile toh null return ho sakta hai.
        Task<User?> GetByEmailAsync(string email);
    }
}
