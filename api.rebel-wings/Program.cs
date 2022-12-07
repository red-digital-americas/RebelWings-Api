using api.rebel_wings.ActionFilter;
using api.rebel_wings.Mapper;
using AutoMapper;
using biz.bd2.Repository.Articulos;
using biz.fortia.Repository.RH;
using biz.rebel_wings.Models.Email;
using biz.rebel_wings.Repository.Alarm;
using biz.rebel_wings.Repository.CashRegisterShortage;
using biz.rebel_wings.Repository.CatStatusSalesExpectations;
using biz.rebel_wings.Repository.Permission;
using biz.rebel_wings.Repository.Role;
using biz.rebel_wings.Repository.SalesExpectations;
using biz.rebel_wings.Repository.TabletSafeKeeping;
using biz.rebel_wings.Repository.Tip;
using biz.rebel_wings.Repository.ToSetTable;
using biz.rebel_wings.Repository.BanosMatutino;
using biz.rebel_wings.Repository.User;
using biz.rebel_wings.Repository.ValidateAttendance;
using biz.rebel_wings.Repository.ValidationGas;
using biz.rebel_wings.Repository.WaitlistTable;
using biz.rebel_wings.Services.Email;
using biz.rebel_wings.Services.Logger;
using dal.bd1.DBContext;
using dal.bd2.DBContext;
using dal.bd2.Repository.Articulos;
using dal.fortia.DBContext;
using dal.fortia.Repository.RH;
using dal.rebel_wings.DBContext;
using dal.rebel_wings.Repository.Alarm;
using dal.rebel_wings.Repository.CashRegisterShortage;
using dal.rebel_wings.Repository.CatStatusSalesExpectations;
using dal.rebel_wings.Repository.LivingRoomBathroomCleaning;
using dal.rebel_wings.Repository.Permission;
using dal.rebel_wings.Repository.Role;
using dal.rebel_wings.Repository.SalesExpectations;
using dal.rebel_wings.Repository.TabletSafeKeeping;
using dal.rebel_wings.Repository.Tip;
using dal.rebel_wings.Repository.ToSetTable;
using dal.rebel_wings.Repository.BanosMatutino;
using dal.rebel_wings.Repository.User;
using dal.rebel_wings.Repository.ValidateAttendance;
using dal.rebel_wings.Repository.ValidationGas;
using dal.rebel_wings.Repository.WaitlistTable;
using dal.rebel_wings.Services.Email;
using dal.rebel_wings.Services.Logger;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;
using biz.bd1.Repository.Albaran;
using biz.bd1.Repository.Remision;
using biz.rebel_wings.Entities;
using biz.rebel_wings.Repository.Albaran;
using biz.rebel_wings.Repository.AudioVideo;
using biz.rebel_wings.Repository.Bar;
using biz.rebel_wings.Repository.BarCleaning;
using biz.rebel_wings.Repository.Bathroom;
using biz.rebel_wings.Repository.BathRoomsOverallStatus;
using biz.rebel_wings.Repository.CheckTable;
using biz.rebel_wings.Repository.Inventario;
using biz.rebel_wings.Repository.Kitchen;
using biz.rebel_wings.Repository.CompleteProductsInOrder;
using biz.rebel_wings.Repository.Dashboard;
using biz.rebel_wings.Repository.DrinksTemperature;
using biz.rebel_wings.Repository.EntriesChargedAsDeliveryNote;
using biz.rebel_wings.Repository.Fridge;
using biz.rebel_wings.Repository.FridgeSalon;
using biz.rebel_wings.Repository.Fryer;
using biz.rebel_wings.Repository.GeneralCleaning;
using biz.rebel_wings.Repository.Order;
using biz.rebel_wings.Repository.OrderScheduleReview;
using biz.rebel_wings.Repository.PeopleCounting;
using biz.rebel_wings.Repository.PrecookedChicken;
using biz.rebel_wings.Repository.RequestTransfer;
using biz.rebel_wings.Repository.RiskProduct;
using biz.rebel_wings.Repository.Salon;
using biz.rebel_wings.Repository.SatisfactionSurvey;
using biz.rebel_wings.Repository.Spotlight;
using biz.rebel_wings.Repository.Station;
using biz.rebel_wings.Repository.Task;
using biz.rebel_wings.Repository.Ticket;
using biz.rebel_wings.Repository.Ticketing;
using biz.rebel_wings.Repository.TicketTable;
using biz.rebel_wings.Repository.WashBasinWithSoapPaper;
using dal.bd1.Repository.Albaran;
using dal.bd1.Repository.Remision;
using dal.rebel_wings.Repository.Albaran;
using dal.rebel_wings.Repository.AudioVideo;
using dal.rebel_wings.Repository.Bar;
using dal.rebel_wings.Repository.BarCleaning;
using dal.rebel_wings.Repository.Bathroom;
using dal.rebel_wings.Repository.BathRoomsOverallStatus;
using dal.rebel_wings.Repository.CheckTable;
using dal.rebel_wings.Repository.Inventario;
using dal.rebel_wings.Repository.Kitchen;
using dal.rebel_wings.Repository.CompleteProductsInOrder;
using dal.rebel_wings.Repository.Dashboard;
using dal.rebel_wings.Repository.DrinksTemperature;
using dal.rebel_wings.Repository.EntriesChargedAsDeliveryNote;
using dal.rebel_wings.Repository.Fridge;
using dal.rebel_wings.Repository.FridgeSalon;
using dal.rebel_wings.Repository.Fryer;
using dal.rebel_wings.Repository.GeneralCleaning;
using dal.rebel_wings.Repository.Order;
using dal.rebel_wings.Repository.OrderScheduleReview;
using dal.rebel_wings.Repository.PeopleCounting;
using dal.rebel_wings.Repository.PrecookedChicken;
using dal.rebel_wings.Repository.RequestTransfer;
using dal.rebel_wings.Repository.RiskProduct;
using dal.rebel_wings.Repository.Salon;
using dal.rebel_wings.Repository.SatisfactionSurvey;
using dal.rebel_wings.Repository.Spotlight;
using dal.rebel_wings.Repository.Station;
using dal.rebel_wings.Repository.Task;
using dal.rebel_wings.Repository.Ticket;
using dal.rebel_wings.Repository.Ticketing;
using dal.rebel_wings.Repository.TicketTable;
using dal.rebel_wings.Repository.WashBasinWithSoapPaper;


var builder = WebApplication.CreateBuilder(args);




// Add services to the container.

builder.Services.AddControllers();
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var connectionStringFortia = builder.Configuration.GetConnectionString("Fortia");
var connectionStringDB1 = builder.Configuration.GetConnectionString("DB1");
var connectionStringBD2 = builder.Configuration.GetConnectionString("DB2");
builder.Services.AddDbContext<Db_Rebel_WingsContext>(options => options.UseSqlServer(connectionString))
    .AddDbContext<BDFORTIAContext>(options => options.UseSqlServer(connectionStringFortia))
    .AddDbContext<BD1Context>(options => options.UseSqlServer(connectionStringDB1))
    .AddDbContext<BD2Context>(options => options.UseSqlServer(connectionStringBD2));
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailConfigurations"));

builder.Services.AddCors(options => options.AddPolicy("CorsPolicy",
            builder =>
            {
                builder
                .AllowAnyMethod()
                .AllowAnyHeader()
                .WithOrigins("http://localhost")
                .WithOrigins("http://localhost:4200")
                .WithOrigins("http://localhost:8100")
                .WithOrigins("http://demo-minimalist.com")
                .WithOrigins("http://34.237.214.147")
                .WithOrigins("https://my.premierds.com/")
                .WithOrigins("Ionic://localhost")
                .WithOrigins("capacitor://localhost")
                .WithOrigins("http://localhost:63410")
                .AllowCredentials();
            }));

builder.Services.AddSingleton<IEmailService, EmailService>();
builder.Services.AddSingleton<ILoggerManager, LoggerManager>();
builder.Services.AddScoped<ValidationFilterAttribute>();
#region REPOSITORIES
builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<IRoleRepository, RoleRepository>();
builder.Services.AddTransient<IPermissionRepository, PermissionRepository>();
builder.Services.AddTransient<IValidationGasRepository, ValidationGasRepository>();
builder.Services.AddTransient<IToSetTableRepository, ToSetTableRepository>();
builder.Services.AddTransient<IBanosMatutinoRepository, BanosMatutinoRepository>();
builder.Services.AddTransient<ICatStatusStockChickenRepository, CatStatusSalesExpectationRepository>();
builder.Services.AddTransient<IStockChickenRepository, SalesExpectationRepository>();
builder.Services.AddTransient<IStockChickeUsedRepository, StockChickeUsedRepository>();
builder.Services.AddTransient<IWaitlistTableRepository, WaitlistTableRepository>();
builder.Services.AddTransient<IValidationGasRepository, ValidationGasRepository>();
builder.Services.AddTransient<IRHTrabRepository, RHTrabRepository>();
builder.Services.AddTransient<IValidateAttendanceRepository, ValidateAttendanceRepository>();
builder.Services.AddTransient<IAlarmRepository, AlarmRepository>();
builder.Services.AddTransient<ITipRepository, TipRepository>();
builder.Services.AddTransient<ITabletSafeKeepingRepository, TabletSafeKeepingRepository>();
builder.Services.AddTransient<ILivingRoomBathroomCleaningRepository, LivingRoomBathroomCleaningRepository>();
builder.Services.AddTransient<ICashRegisterShortageRepository, CashRegisterShortageRepository>();
builder.Services.AddTransient<IRiskProductRepository, RiskProductRepository>();
builder.Services.AddTransient<biz.bd1.Repository.Articulos.IArticulosRespository, dal.bd1.Repository.Articulos.ArticulosRepository>();
builder.Services.AddTransient<biz.bd2.Repository.Articulos.IArticulosRepository, dal.bd2.Repository.Articulos.ArticulosRepository>();
builder.Services.AddTransient<ICatAmountRepository, CatAmountRepository>();
builder.Services.AddTransient<IRequestTransferRepository, RequestTransferRepository>();
builder.Services.AddTransient<IStockChickenByBranchRepository, StockChickenByBranchRepository>();
builder.Services.AddTransient<ITicketRepository, TicketRepository>();
builder.Services.AddTransient<IPhotoTicketRepository, PhotoTicketRepository>();
builder.Services.AddTransient<ITaskRepository, TaskRepository>();
builder.Services.AddTransient<ITaskBranchRepository, TaskBranchRepository>();
builder.Services.AddTransient<IAlbaranRepository, AlbaranRepository>();
builder.Services.AddTransient<biz.bd2.Repository.Albaran.IAlbaranRepository, dal.bd2.Repository.Albaran.AlbaranRepository>();
builder.Services.AddTransient<IAlbaranesRepository, AlbaranesRepository>();
builder.Services.AddTransient<ICatStatusAlbaranRepository, CatStatusAlbaranRepository>();
builder.Services.AddTransient<IOrderRepository, OrderRepository>();
builder.Services.AddTransient<IPhotoOrderRepository, PhotoOrderRepository>();
builder.Services.AddTransient<IPhotoPeopleCountingRepository, PhotoPeopleCountingRepository>();
builder.Services.AddTransient<IFridgeRepository, FridgeRepository>();
builder.Services.AddTransient<IPhotoFridgeRepository, PhotoFridgeRepository>();
builder.Services.AddTransient<IPrecookedChickenRepository, PrecookedChickenRepository>();
builder.Services.AddTransient<IPhotoPrecookedChickenRepository, PhotoPrecookedChickenRepository>();
builder.Services.AddTransient<ICompleteProductsInOrderRepository, CompleteProductsInOrderRepository>();
builder.Services.AddTransient<IPhotoCompleteProductsInOrderRepository, PhotoCompleteProductsInOrderRepository>();
builder.Services.AddTransient<IFryerCleaningRepository, FryerCleaningRepository>();
builder.Services.AddTransient<IPhotoFryerCleaningRepository, PhotoFryerCleaningRepository>();
builder.Services.AddTransient<IPeopleCountingRepository, PeopleCountingRepository>();
builder.Services.AddTransient<ISatisfactionSurveyRepository, SatisfactionSurveyRepository>();
builder.Services.AddTransient<IGeneralCleaningRepository, GeneralCleaningRepository>();
builder.Services.AddTransient<IPhotoGeneralCleaningRepository, PhotoGeneralCleaningRepository>();
builder.Services.AddTransient<IStationRepository, StationRepository>();
builder.Services.AddTransient<IPhotoStationRepository, PhotoStationRepository>();
builder.Services.AddTransient<IAudioVideoRepository, AudioVideoRepository>();
builder.Services.AddTransient<IPhotoDrinksTemperatureRepository, PhotoDrinksTemperatureRepository>();
builder.Services.AddTransient<IDrinksTemperatureRepository, DrinksTemperatureRepository>();
builder.Services.AddTransient<ISpotlightRepository, SpotlightRepository>();
builder.Services.AddTransient<IBarCleaningRepository, BarCleaningRepository>();
builder.Services.AddTransient<IPhotoBarCleaningRepository, PhotoBarCleaningRepository>();
builder.Services.AddTransient<IFridgeSalonRepository, FridgeSalonRepository>();
builder.Services.AddTransient<IPhotoFridgeSalonRepository, PhotoFridgeSalonRepository>();
builder.Services.AddTransient<IWashBasinWithSoapPaperRepository, WashBasinWithSoapPaperRepository>();
builder.Services.AddTransient<IBathRoomsOverallStatusRepository, BathRoomsOverallStatusRepository>();
builder.Services.AddTransient<IPhotoWashBasinWithSoapPaperRepository, PhotoWashBasinWithSoapPaperRepository>();
builder.Services.AddTransient<ITicketTableRepository, TicketTableRepository>();
builder.Services.AddTransient<IPhotoTicketTableRepository, PhotoTicketTableRepository>();
builder.Services.AddTransient<IEntriesChargedAsDeliveryNoteRepository, EntriesChargedAsDeliveryNoteRepository>();
builder.Services.AddTransient<IPhotoEntriesChargedAsDeliveryNoteRepository, PhotoEntriesChargedAsDeliveryNoteRepository>();
builder.Services.AddTransient<ICheckTableRepository, CheckTableRepository>();
builder.Services.AddTransient<IInventarioRepository, InventarioRepository>();
builder.Services.AddTransient<IBathroomRepository, BathroomRepository>();
builder.Services.AddTransient<ISalonRepository, SalonRepository>();
builder.Services.AddTransient<IKitchenRepository, KitchenRepository>();
builder.Services.AddTransient<IOrderScheduleReviewRepository, OrderScheduleReviewRepository>();
builder.Services.AddTransient<IPhotoOrderScheduleReviewRepository, PhotoOrderScheduleReviewRepository>();
builder.Services.AddTransient<IBarRepository, BarRepository>();
builder.Services.AddTransient<ITicketingRepository, TicketingRepository>();
builder.Services.AddTransient<IPhotoTicketingRepository, PhotoTicketingRepository>();
builder.Services.AddTransient<ICommentTicketingRepository, CommentTicketingRepository>();
builder.Services.AddTransient<ICatTicketingRepository, CatTicketingRepository>();
builder.Services.AddTransient<ICatBranchLocateRepository, CatBranchLocateRepository>();
builder.Services.AddTransient<IPhotoBarRepository, PhotoBarRepository>();
builder.Services.AddTransient<IPhotoBathroomRepository, PhotoBathroomRepository>();
builder.Services.AddTransient<IPhotoAudioVideoRepository, PhotoAudioVideoRepository>();
builder.Services.AddTransient<IPhotoBathRoomsOverallStatusRepository, PhotoBathRoomsOverallStatusRepository>();
builder.Services.AddTransient<IPhotoValidateAttendanceRepository, PhotoValidateAttendanceRepository>();
builder.Services.AddTransient<IPhotoSpotlightRepository, PhotoSpotlightRepository>();
builder.Services.AddTransient<IPhotoSatisfactionSurveyRepository, PhotoSatisfactionSurveyRepository>();
builder.Services.AddTransient<IPhotoSalonRepository, PhotoSalonRepository>();
builder.Services.AddTransient<IPhotoKitchenRepository, PhotoKitchenRepository>();
builder.Services.AddTransient<IDashboardRepository, DashboardRepository>();
builder.Services.AddTransient<IPhotoToSetTableRepository, PhotoToSetTableRepository>();
builder.Services.AddTransient<IPhotoBanosMatutinoRepository, PhotoBanosMatutinoRepository>();
builder.Services.AddTransient<IPhotoWaitlistTableRepository, PhotoWaitlistTableRepository>();
builder.Services.AddTransient<IPhotoValidationGasRepository, PhotoValidationGasRepository>();
builder.Services.AddTransient<IPhotoTipRepository, PhotoTipRepository>();
builder.Services.AddTransient<IPhotoLivingRoomBathroomCleaningRepository, PhotoLivingRoomBathroomCleaningRepository>();
builder.Services.AddTransient<IPhotoTabletSafeKeepingRepository, PhotoTabletSafeKeepingRepository>();
builder.Services.AddTransient<IPhotoAlarmRepository, PhotoAlarmRepository>();
builder.Services.AddTransient<IPhotoCashRegisterShortageRepository, PhotoCashRegisterShortageRepository>();
builder.Services.AddTransient<IRemisionRepository, RemisionRepository>();
builder.Services.AddTransient<biz.bd2.Repository.Remision.IRemisionRepository, dal.bd2.Repository.Remision.RemisionRepository>();
builder.Services.AddTransient<biz.bd1.Repository.Sucursal.ISucursalRepository, dal.bd1.Repository.Sucursal.SucursalRepository>();
builder.Services.AddTransient<biz.bd2.Repository.Sucursal.ISucursalRepository, dal.bd2.Repository.Sucursal.SucursalRepository>();
builder.Services.AddTransient<biz.bd1.Repository.Tesoreria.ITesoreriaRepository, dal.bd1.Repository.Tesoreria.TesoreriaRepository>();
builder.Services.AddTransient<biz.bd2.Repository.Tesoreria.ITesoreriaRepository, dal.bd2.Repository.Tesoreria.TesoreriaRepository>();
builder.Services.AddTransient<biz.bd1.Repository.Stock.IStockRepository, dal.bd1.Repository.Stock.StockRepository>();
builder.Services.AddTransient<biz.bd2.Repository.Stock.IStockRepository, dal.bd2.Repository.Stock.StockRepository>();
builder.Services.AddTransient<biz.bd2.Repository.PedidoEntrega.ITPedidosEntregaRepository, dal.bd2.Repository.PedidoEntrega.TPedidosEntregaRepository>();
builder.Services.AddTransient<biz.bd1.Repository.PedidoEntrega.ITPedidosEntregaRepository, dal.bd1.Repository.PedidoEntrega.TPedidosEntregaRepository>();
builder.Services.AddTransient<biz.bd2.Repository.PedidoEntrega.ITFotosPedidosEntregaRepository, dal.bd2.Repository.PedidoEntrega.TFotosPedidosEntregaRepository>();
builder.Services.AddTransient<biz.bd1.Repository.PedidoEntrega.ITFotosPedidosEntregaRepository, dal.bd1.Repository.PedidoEntrega.TFotosPedidosEntregaRepository>();
builder.Services.AddTransient<biz.bd2.Repository.PedidoEntrega.ITEstatusPedidosEntregaRepository, dal.bd2.Repository.PedidoEntrega.TEstatusPedidosEntregaRepository>();
builder.Services.AddTransient<biz.bd1.Repository.PedidoEntrega.ITEstatusPedidosEntregaRepository, dal.bd1.Repository.PedidoEntrega.TEstatusPedidosEntregaRepository>();
#endregion
// Auto Mapper Configurations
var mapperConfig = new MapperConfiguration(mc =>
{
    mc.AddProfile(new MapperProfile());
});
IMapper mapper = mapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper);
builder.Services.AddMvc(config =>
{
    config.RespectBrowserAcceptHeader = true;
    config.InputFormatters.Add(new XmlSerializerInputFormatter(config));
    config.OutputFormatters.Add(new XmlSerializerOutputFormatter());
})
    .AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReadCommentHandling = System.Text.Json.JsonCommentHandling.Skip;
}).SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = builder.Configuration["Jwt:Issuer"],
                        ValidAudience = builder.Configuration["Jwt:Issuer"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
                    };
                });
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "api.rebel_wings", Version = "v1" });
    // Set the comments path for the Swagger JSON and UI.
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
    //c.IncludeXmlComments(GetXmlCommentsPathForModels());
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please insert JWT with Bearer into field",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] { }
                    }
                });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "api.rebel_wings v1"));
//}
app.UseCors("CorsPolicy");
app.UseSwaggerUI(c =>
{
    app.UseSwagger().UseDeveloperExceptionPage();
#if DEBUG
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "api.rebel_wings v1");
#else
    c.SwaggerEndpoint("/back/api_rebel_wings/swagger/v1/swagger.json", "api.rebel_wings v1");
#endif
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
