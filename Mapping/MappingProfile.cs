using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using vega.Controllers.Resources;
using vega.Core.models;

namespace vega.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile(){
            //Domain to API Resource
            CreateMap<Make, MakeResource>();
            CreateMap<Make, KeyValuePairResource>();
            CreateMap<Model, KeyValuePairResource>();
            CreateMap<Feature, KeyValuePairResource>();
            CreateMap<Vehicle, SaveVehicleResource>()
                .ForMember(vr => vr.Contact, opt => opt.MapFrom(v => new ContactResource { Name = v.Contact.Name, Email = v.Contact.Email, Phone = v.Contact.Phone } ))
                .ForMember(vr => vr.Features, opt => opt.MapFrom(v => v.Features.Select(vf => vf.FeatureId)));
            CreateMap<Vehicle, VehicleResource>()
                .ForMember(vr => vr.Make, opt => opt.MapFrom(v => v.Model.Make))
                .ForMember(vr => vr.Contact, opt => opt.MapFrom(v => new ContactResource { Name = v.Contact.Name, Email = v.Contact.Email, Phone = v.Contact.Phone } ))
                .ForMember(vr => vr.Features, opt => opt.MapFrom(v => v.Features.Select(vf => new KeyValuePairResource {Id = vf.Feature.Id, Name = vf.Feature.Name})));


            //API Resource to Domain
            CreateMap<SaveVehicleResource, Vehicle>()
                .ForMember(v => v.Id, opt => opt.Ignore())
                .ForMember(v => v.Features, opt => opt.Ignore())
                .AfterMap((vr, v) => {
                    //Find features that are no longer selected
                    var removedFeatures = v.Features.Where(f=> !vr.Features.Contains(f.FeatureId));
                    //Loop through removed and actually remove them
                    foreach (var f in removedFeatures)
                    {
                        v.Features.Remove(f);
                    }

                    var addedFeatures = vr.Features.Where(id => !v.Features.Any(f => f.FeatureId == id)).Select(id => new VehicleFeature {FeatureId = id});
                    //Add new features
                    foreach (var f in addedFeatures)
                    {
                        v.Features.Add(f);
                    }
                });
            CreateMap<ContactResource, Contact>();
        }
    }
}