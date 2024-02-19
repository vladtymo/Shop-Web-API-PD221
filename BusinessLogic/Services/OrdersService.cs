using AutoMapper;
using BusinessLogic.DTOs;
using BusinessLogic.Interfaces;
using BusinessLogic.Models;
using DataAccess.Data;
using DataAccess.Data.Entities;
using DataAccess.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace BusinessLogic.Services
{
    internal class OrdersService : IOrdersService
    {
        private readonly IMapper mapper;
        private readonly IRepository<Order> ordersR;
        private readonly IRepository<Product> productsR;

        //private readonly ShopDbContext context;

        private readonly ICartService cartService;
        private readonly IEmailSender emailSender;
        //private readonly IViewRender viewRender;
        private readonly UserManager<User> userManager;

        public OrdersService(IMapper mapper, 
                            IRepository<Order> ordersR,
                            IRepository<Product> productsR,
                            ICartService cartService,
                            IEmailSender emailSender,
                            // IViewRender viewRender,
                            UserManager<User> userManager)
        {
            this.mapper = mapper;
            this.ordersR = ordersR;
            this.productsR = productsR;
            this.cartService = cartService;
            this.emailSender = emailSender;
            //this.viewRender = viewRender;
            this.userManager = userManager;
        }

        public async Task Create(string userId)
        {
            var ids = cartService.GetProductIds();
            var products = productsR.Get(x => ids.Contains(x.Id));

            User user = await userManager.FindByIdAsync(userId) ?? throw new Exception("User not found!");

            var order = new Order()
            {
                Date = DateTime.Now,
                UserId = userId,
                Products = products.ToList(),
                TotalPrice = products.Sum(x => x.Price),
            };

            ordersR.Insert(order);
            ordersR.Save();

            // send order summary email
            //string html = this.viewRender.Render("MailTemplates/OrderSummary", new OrderSummaryModel()
            //{
            //    UserName = user.UserName!,
            //    Products = mapper.Map<IEnumerable<ProductDto>>(products),
            //    TotalPrice = 5656
            //});

            //await emailSender.SendEmailAsync("datolo1825@fahih.com", $"Order #{12}", html);
        }

        public IEnumerable<OrderDto> GetAllByUser(string userId)
        {
            var items = ordersR.Get(x => x.UserId == userId);
            return mapper.Map<IEnumerable<OrderDto>>(items);
        }
    }
}
