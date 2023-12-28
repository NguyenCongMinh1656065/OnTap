using Ôn_tập.Models;

namespace Ôn_tập.Interfaces
{
    public interface IOwnerRepository
    {
        ICollection<Owner> GetOwners();
        Owner GetOwner(int ownerId);
        ICollection<Owner> GetOwnerOfAPokemon(int pokeId);
        ICollection<Pokemon> GetPokemonByOwner(int ownerId);
        bool OwnerExists(int ownerId);
        bool Create(Owner owner);
        bool Update(Owner owner);
        bool Delete(Owner owner);
        bool Save();

    }
}
