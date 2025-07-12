using AutoMapper;
using RestaurantAPI.Dtos.AboutDtos;
using RestaurantAPI.Dtos.AdminLogDtos;
using RestaurantAPI.Dtos.CategoryDtos;
using RestaurantAPI.Dtos.ChefDtos;
using RestaurantAPI.Dtos.CompanyInfoDtos;
using RestaurantAPI.Dtos.EventDtos;
using RestaurantAPI.Dtos.FeatureDtos;
using RestaurantAPI.Dtos.GalleryImageDtos;
using RestaurantAPI.Dtos.IconDtos;
using RestaurantAPI.Dtos.MessageDtos;
using RestaurantAPI.Dtos.ProductDtos;
using RestaurantAPI.Dtos.ReservationDtos;
using RestaurantAPI.Dtos.ServiceDtos;
using RestaurantAPI.Dtos.TestimonialDtos;
using RestaurantAPI.Dtos.WebLogDtos;
using RestaurantAPI.Entities;
using ResultCompanyInfoDto = RestaurantAPI.Dtos.CompanyInfoDtos.ResultCompanyInfoDto;

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

            CreateMap<Feature, ResultFeatureDto>().ReverseMap();
            CreateMap<Feature, UpdateFeatureDto>().ReverseMap();

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

            CreateMap<CompanyInfo, ResultCompanyInfoDto>().ReverseMap();
            CreateMap<CompanyInfo, UpdateCompanyInfoDto>().ReverseMap();

            CreateMap<Event, ResultEventDto>().ReverseMap();
            CreateMap<Event, CreateEventDto>().ReverseMap();
            CreateMap<Event, UpdateEventDto>().ReverseMap();
            CreateMap<Event, GetByIDEventDto>().ReverseMap();

            CreateMap<About, ResultAboutDto>().ReverseMap();
            CreateMap<About, UpdateAboutDto>().ReverseMap();

            CreateMap<AdminLog, ResultAdminLogDto>().ReverseMap();
            CreateMap<AdminLog, CreateAdminLogDto>().ReverseMap();

            CreateMap<WebLog, CreateWebLogDto>().ReverseMap();
            CreateMap<WebLog, ResultWebLogDto>().ReverseMap();

            CreateMap<Icon, ResultIconDto>().ReverseMap();
            CreateMap<Icon, CreateIconDto>().ReverseMap();
            CreateMap<Icon, UpdateIconDto>().ReverseMap();
            CreateMap<Icon, GetByIDIconDto>().ReverseMap();
        }
    }
}