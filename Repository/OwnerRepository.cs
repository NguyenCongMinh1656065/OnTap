using AutoMapper;
using Ôn_tập.Data;
using Ôn_tập.Interfaces;
using Ôn_tập.Models;

namespace Ôn_tập.Repository
{
    public class OwnerRepository : IOwnerRepository
    {
        private DataContext _context;

        public OwnerRepository(DataContext context) 
        {
            _context = context;
        }
        public bool Create(Owner owner)
        {
            _context.Add(owner);
            return Save();
        }

        public bool Delete(Owner owner)
        {
            _context.Remove(owner);
            return Save();
        }

        public Owner GetOwner(int ownerId)
        {
            return _context.Owners.Where(o => o.Id == ownerId).FirstOrDefault();
        }

        public ICollection<Owner> GetOwnerOfAPokemon(int pokeId)
        {
            return _context.PokemonOwners.Where(p => p.Pokemon.Id == pokeId).Select(o => o.Owner).ToList();
        }

        public ICollection<Owner> GetOwners()
        {
            return _context.Owners.ToList();
        }

        public ICollection<Pokemon> GetPokemonByOwner(int ownerId)
        {
            return _context.PokemonOwners.Where(o => o.Owner.Id == ownerId).Select(p => p.Pokemon).ToList();
        }

        public bool OwnerExists(int ownerId)
        {
            return _context.Owners.Any(o => o.Id == ownerId);
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Update(Owner owner)
        {
            _context.Update(owner);
            return Save();
        }
    }
}
