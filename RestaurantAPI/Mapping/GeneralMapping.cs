using AutoMapper;
using RestaurantAPI.Dtos.CategoryDtos;
using RestaurantAPI.Dtos.ChefDtos;
using RestaurantAPI.Dtos.ContactDtos;
using RestaurantAPI.Dtos.FeatureDtos;
using RestaurantAPI.Dtos.GalleryImageDtos;
using RestaurantAPI.Dtos.MessageDtos;
using RestaurantAPI.Dtos.ProductDtos;
using RestaurantAPI.Dtos.ReservationDtos;
using RestaurantAPI.Dtos.ServiceDtos;
using RestaurantAPI.Dtos.TestimonialDtos;
using RestaurantAPI.Entities;

namespace RestaurantAPI.Mapping
{
    public class GeneralMapping : Profile
    {
        public GeneralMapping()
        {
            CreateMap<Category, ResultCategoryDto>().ReverseMap();
            CreateMap<Category, CreateCategoryDto>().ReverseMap();
            CreateMap<Category, UpdateCategoryDto>().ReverseMap();
            CreateMap<Category, GetByIDCategoryDto>().ReverseMap();

            CreateMap<Chef, ResultChefDto>().ReverseMap();
            CreateMap<Chef, CreateChefDto>().ReverseMap();
            CreateMap<Chef, UpdateChefDto>().ReverseMap();
            CreateMap<Chef, GetByIDChefDto>().ReverseMap();

            CreateMap<Contact, ResultContactDto>().ReverseMap();
            CreateMap<Contact, CreateContactDto>().ReverseMap();
            CreateMap<Contact, UpdateContactDto>().ReverseMap();
            CreateMap<Contact, GetByIDContactDto>().ReverseMap();

            CreateMap<Feature, ResultFeatureDto>().ReverseMap();
            CreateMap<Feature, CreateFeatureDto>().ReverseMap();
            CreateMap<Feature, UpdateFeatureDto>().ReverseMap();
            CreateMap<Feature, GetByIDFeatureDto>().ReverseMap();

            CreateMap<GalleryImage, ResultGalleryImageDto>().ReverseMap();
            CreateMap<GalleryImage, CreateGalleryImageDto>().ReverseMap();
            CreateMap<GalleryImage, UpdateGalleryImageDto>().ReverseMap();
            CreateMap<GalleryImage, GetByIDGalleryImageDto>().ReverseMap();

            CreateMap<Message, ResultMessageDto>().ReverseMap();
            CreateMap<Message, CreateMessageDto>().ReverseMap();
            CreateMap<Message, UpdateMessageDto>().ReverseMap();
            CreateMap<Message, GetByIDMessageDto>().ReverseMap();

            CreateMap<Product, ResultProductDto>().ForMember(x => x.CategoryName, y => y.MapFrom(z => z.Category.CategoryName)).ReverseMap();
            CreateMap<Product, CreateProductDto>().ReverseMap();
            CreateMap<Product, UpdateProductDto>().ReverseMap();
            CreateMap<Product, GetByIDProductDto>().ForMember(x => x.CategoryName, y => y.MapFrom(z => z.Category.CategoryName)).ReverseMap();

            CreateMap<Reservation, ResultReservationDto>().ReverseMap();
            CreateMap<Reservation, CreateReservationDto>().ReverseMap();
            CreateMap<Reservation, UpdateReservationDto>().ReverseMap();
            CreateMap<Reservation, GetByIDReservationDto>().ReverseMap();

            CreateMap<Service, ResultServiceDto>().ReverseMap();
            CreateMap<Service, CreateServiceDto>().ReverseMap();
            CreateMap<Service, UpdateServiceDto>().ReverseMap();
            CreateMap<Service, GetByIDServiceDto>().ReverseMap();

            CreateMap<Testimonial, ResultTestimonialDto>().ReverseMap();
            CreateMap<Testimonial, CreateTestimonialDto>().ReverseMap();
            CreateMap<Testimonial, UpdateTestimonialDto>().ReverseMap();
            CreateMap<Testimonial, GetByIDTestimonialDto>().ReverseMap(); 
        }
    }
}