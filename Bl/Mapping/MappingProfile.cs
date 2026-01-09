using AutoMapper;
using Bl.DTOs;
using Domains;

namespace BL.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // ================= Product =================
            CreateMap<Product, ProductDto>().ReverseMap();
            CreateMap<CreateProductDto, Product>().ReverseMap();

            // ================= Cart =================
            CreateMap<Cart, CartDto>()
                .ForMember(dest => dest.TotalPrice, opt => opt.MapFrom(src => src.Items.Sum(i => i.Quantity * i.Product.Price)))
                .ReverseMap(); // ReverseMap يمكن تجاهل TotalPrice عند Update/Create

            CreateMap<CartItem, CartItemDto>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name))
                .ReverseMap();

            // ================= User =================
            CreateMap<User, RegisterUserDto>().ReverseMap();
            CreateMap<User, LoginUserDto>().ReverseMap();

            // ================= Category =================
            CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<CreateCategoryDto, Category>();

            // ================= Order =================
            CreateMap<Order, OrderDto>()
                .ForMember(dest => dest.TotalPrice, opt => opt.MapFrom(src => src.Items.Sum(i => i.Price * i.Quantity)))
                .ReverseMap();

            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name))
                .ReverseMap();

            CreateMap<OrderStatus, OrderStatusDto>().ReverseMap();
            CreateMap<CreateOrderDto, Order>()
                .ForMember(dest => dest.Items, opt => opt.Ignore()); // Items هنضيفهم من Cart


            CreateMap<CartItem, OrderItem>()
    .ForMember(dest => dest.Id, opt => opt.Ignore())      // EF سيملأ الـ Id
    .ForMember(dest => dest.OrderId, opt => opt.Ignore()) // EF سيملأ الـ OrderId
    .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Product.Price)); // نجيب السعر من المنتج

            // ================= Review =================
            CreateMap<Review, ReviewDto>().ReverseMap();
        }
    }
}
