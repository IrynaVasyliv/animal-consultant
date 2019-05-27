using System.Collections.Generic;
using AutoMapper;
using DemOffice.GenericCrud.Models;

namespace DemOffice.GenericCrud.Mapping
{
    /// <summary>Class MappingProfile.
    /// Implements the <see cref="AutoMapper.Profile"/></summary>
    internal class MappingProfile : Profile
    {
        public MappingProfile()
        {
        }

        /// <summary>Initializes a new instance of the <see cref="MappingProfile"/> class.</summary>
        /// <param name="setup">The setup.</param>
        public MappingProfile(IReadOnlyCollection<GenericSetup> setup)
        {
            foreach (var map in setup)
            {
                map.CreateMap(this);
            }
        }
    }
}
