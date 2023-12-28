using AutoMapper;
using Ôn_tập.Dto;
using Ôn_tập.Migrations;
using Ôn_tập.Models;

namespace Ôn_tập.Helpes
{
    public class MappingProfiles : Profile

    {
        public MappingProfiles()
        {

            CreateMap<Pokemon, PokemonDto>();
            CreateMap<PokemonDto, Pokemon>();
            CreateMap<Country, CountryDto>();
            CreateMap<CountryDto, Country>();
            CreateMap<OwnerDto, Owner>();
            CreateMap<Owner, OwnerDto>();
            CreateMap<CategoryDto, Category>();
            CreateMap<Category, CategoryDto>();
            CreateMap<ReviewDto, Review>();
            CreateMap<Review , ReviewDto>();
            CreateMap<Reviewer, ReviewerDto>();
            CreateMap<ReviewerDto, Reviewer>();
        }
    }
}
