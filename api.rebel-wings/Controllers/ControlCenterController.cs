using api.rebel_wings.ActionFilter;
using api.rebel_wings.Models.ApiResponse;
using api.rebel_wings.Models.ControlCenter;
using AutoMapper;
using biz.rebel_wings.Repository.Alarm;
using biz.rebel_wings.Repository.CashRegisterShortage;
using biz.rebel_wings.Repository.SalesExpectations;
using biz.rebel_wings.Repository.TabletSafeKeeping;
using biz.rebel_wings.Repository.Tip;
using biz.rebel_wings.Repository.ToSetTable;
using biz.rebel_wings.Repository.ValidateAttendance;
using biz.rebel_wings.Repository.ValidationGas;
using biz.rebel_wings.Repository.WaitlistTable;
using biz.rebel_wings.Services.Logger;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using api.rebel_wings.Extensions;
using api.rebel_wings.Models.SalesExpectations;
using biz.rebel_wings.Repository.Albaran;
using biz.rebel_wings.Repository.AudioVideo;
using biz.rebel_wings.Repository.Bar;
using biz.rebel_wings.Repository.BarCleaning;
using biz.rebel_wings.Repository.Bathroom;
using biz.rebel_wings.Repository.BathRoomsOverallStatus;
using biz.rebel_wings.Repository.CheckTable;
using biz.rebel_wings.Repository.CompleteProductsInOrder;
using biz.rebel_wings.Repository.DrinksTemperature;
using biz.rebel_wings.Repository.EntriesChargedAsDeliveryNote;
using biz.rebel_wings.Repository.Fridge;
using biz.rebel_wings.Repository.FridgeSalon;
using biz.rebel_wings.Repository.Fryer;
using biz.rebel_wings.Repository.GeneralCleaning;
using biz.rebel_wings.Repository.Kitchen;
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
using biz.rebel_wings.Repository.TicketTable;
using biz.rebel_wings.Repository.WashBasinWithSoapPaper;
using biz.rebel_wings.Repository.BanosMatutino;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;

namespace api.rebel_wings.Controllers
{
    /// <summary>
    /// COntrolador para Centro de Control
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class ControlCenterController : ControllerBase
    {

        private readonly IMapper _mapper;
        private readonly ILoggerManager _logger;
        private readonly IValidateAttendanceRepository _validateAttendanceRepository;
        private readonly IStockChickenRepository _salesExpectationRepository;
        private readonly IToSetTableRepository _toSetTableRepository;
        private readonly IBanosMatutinoRepository _banosMatutinoRepository;
        private readonly IValidationGasRepository _validationGasRepository;
        private readonly IWaitlistTableRepository _waitlistTableRepository;
        private readonly ITipRepository _tipRepository;
        private readonly ITabletSafeKeepingRepository _tabletSafeKeepingRepository;
        private readonly ILivingRoomBathroomCleaningRepository _livingRoomBathroomCleaningRepository;
        private readonly IAlarmRepository _alarmRepository;
        private readonly ICashRegisterShortageRepository _cashRegisterShortageRepository;
        private readonly IRiskProductRepository _riskProductRepository;
        private readonly IRequestTransferRepository _requestTransferRepository;
        private readonly IAlbaranesRepository _albaranesRepository;
        private readonly IPhotoToSetTableRepository _photoToSetTableRepository;


        #region Regional

        private readonly IOrderRepository _orderRepository;
        private readonly IFridgeRepository _fridgeRepository;
        private readonly IPrecookedChickenRepository _precookedChickenRepository;
        private readonly ICompleteProductsInOrderRepository _completeProductsInOrderRepository;
        private readonly IFryerCleaningRepository _fryerCleaningRepository;
        
        private readonly IPeopleCountingRepository _peopleCountingRepository;
        private readonly ISatisfactionSurveyRepository _satisfactionSurveyRepository;
        private readonly IGeneralCleaningRepository _generalCleaningRepository;
        private readonly IStationRepository _stationRepository;
        private readonly ISpotlightRepository _spotlightRepository;
        private readonly IDrinksTemperatureRepository _drinksTemperatureRepository;
        private readonly IAudioVideoRepository _audioVideoRepository;
        private readonly IBarCleaningRepository _barCleaningRepository;
        private readonly IFridgeSalonRepository _fridgeSalonRepository;
        
        private readonly IBathRoomsOverallStatusRepository _bathRoomsOverallStatusRepository;
        private readonly IWashBasinWithSoapPaperRepository _washBasinWithSoapPaperRepository;

        private readonly ITicketTableRepository _ticketTableRepository;
        private readonly IEntriesChargedAsDeliveryNoteRepository _entriesChargedAsDeliveryNoteRepository;
        private readonly IOrderScheduleReviewRepository _orderScheduleReviewRepository;
        private readonly ICheckTableRepository _checkTableRepository;
        
        private readonly IKitchenRepository _kitchenRepository;
        private readonly ISalonRepository _salonRepository;
        private readonly IBathroomRepository _bathroomRepository;
        private readonly IBarRepository _barRepository;
        
        private readonly IStockChickenByBranchRepository _stockChickenByBranchRepository;
        private readonly IPhotoGeneralCleaningRepository _photoGeneralCleaningRepository;
        private readonly IPhotoDrinksTemperatureRepository _photoDrinksTemperatureRepository;
        private readonly IPhotoSpotlightRepository _photoSpotlightRepository;
        private readonly IPhotoWashBasinWithSoapPaperRepository _photoWashBasinWithSoapPaperRepository;
        private readonly IPhotoTicketTableRepository _photoTicketTableRepository;
        private readonly IPhotoKitchenRepository _photoKitchenRepository;
        private readonly IPhotoSalonRepository _photoSalonRepository;
        private readonly IPhotoBathroomRepository _photoBathroomRepository;
        private readonly IPhotoBarRepository _photoBarRepository;

    #endregion
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="mapper"></param>
    /// <param name="logger"></param>
    /// <param name="validateAttendanceRepository"></param>
    /// <param name="salesExpectationRepository"></param>
    /// <param name="toSetTableRepository"></param>
    /// <param name="banosMatutinoRepository"></param>
    /// <param name="validationGasRepository"></param>
    /// <param name="waitlistTableRepository"></param>
    /// <param name="tipRepository"></param>
    /// <param name="tabletSafeKeepingRepository"></param>
    /// <param name="livingRoomBathroomCleaningRepository"></param>
    /// <param name="alarmRepository"></param>
    /// <param name="cashRegisterShortageRepository"></param>
    /// <param name="riskProductRepository"></param>
    /// <param name="orderRepository"></param>
    /// <param name="fridgeRepository"></param>
    /// <param name="precookedChickenRepository"></param>
    /// <param name="completeProductsInOrderRepository"></param>
    /// <param name="fryerCleaningRepository"></param>
    /// <param name="peopleCountingRepository"></param>
    /// <param name="satisfactionSurveyRepository"></param>
    /// <param name="generalCleaningRepository"></param>
    /// <param name="stationRepository"></param>
    /// <param name="spotlightRepository"></param>
    /// <param name="drinksTemperatureRepository"></param>
    /// <param name="audioVideoRepository"></param>
    /// <param name="barCleaningRepository"></param>
    /// <param name="fridgeSalonRepository"></param>
    /// <param name="bathRoomsOverallStatusRepository"></param>
    /// <param name="washBasinWithSoapPaperRepository"></param>
    /// <param name="ticketTableRepository"></param>
    /// <param name="entriesChargedAsDeliveryNoteRepository"></param>
    /// <param name="orderScheduleReviewRepository"></param>
    /// <param name="checkTableRepository"></param>
    /// <param name="kitchenRepository"></param>
    /// <param name="salonRepository"></param>
    /// <param name="bathroomRepository"></param>
    /// <param name="barRepository"></param>
    /// <param name="requestTransferRepository"></param>
    /// <param name="albaranesRepository"></param>
    /// <param name="stockChickenByBranchRepository"></param>
    /// <param name="photoGeneralCleaningRepository"></param>
    /// <param name="photoDrinksTemperatureRepository"></param>
    /// <param name="photoSpotlightRepository"></param>
    /// <param name="photoWashBasinWithSoapPaperRepository"></param>
    /// <param name="photoTicketTableRepository"></param>
    /// <param name="photoKitchenRepository"></param>
    /// <param name="photoSalonRepository"></param>
    /// <param name="photoBathroomRepository"></param>
    /// <param name="photoBarRepository"></param>
    /// <param name="photoToSetTableRepository"></param>

    public ControlCenterController(IMapper mapper, ILoggerManager logger, IValidateAttendanceRepository validateAttendanceRepository,
            IStockChickenRepository salesExpectationRepository, IToSetTableRepository toSetTableRepository, IBanosMatutinoRepository banosMatutinoRepository,
            IValidationGasRepository validationGasRepository, IWaitlistTableRepository waitlistTableRepository,
            ITipRepository tipRepository,
            ITabletSafeKeepingRepository tabletSafeKeepingRepository,
            ILivingRoomBathroomCleaningRepository livingRoomBathroomCleaningRepository,
            IAlarmRepository alarmRepository,
            ICashRegisterShortageRepository cashRegisterShortageRepository,
            IRiskProductRepository riskProductRepository,
            IOrderRepository orderRepository,
            IFridgeRepository fridgeRepository,
            IPrecookedChickenRepository precookedChickenRepository,
            ICompleteProductsInOrderRepository completeProductsInOrderRepository,
            IFryerCleaningRepository fryerCleaningRepository,
            IPeopleCountingRepository peopleCountingRepository,
            ISatisfactionSurveyRepository satisfactionSurveyRepository,
            IGeneralCleaningRepository generalCleaningRepository,
            IStationRepository stationRepository,
            ISpotlightRepository spotlightRepository,
            IDrinksTemperatureRepository drinksTemperatureRepository, 
            IAudioVideoRepository audioVideoRepository,
            IBarCleaningRepository barCleaningRepository,
            IFridgeSalonRepository fridgeSalonRepository,
            IBathRoomsOverallStatusRepository bathRoomsOverallStatusRepository,
            IWashBasinWithSoapPaperRepository washBasinWithSoapPaperRepository,
            ITicketTableRepository ticketTableRepository,
            IEntriesChargedAsDeliveryNoteRepository entriesChargedAsDeliveryNoteRepository,
            IOrderScheduleReviewRepository orderScheduleReviewRepository,
            ICheckTableRepository checkTableRepository,
            IKitchenRepository kitchenRepository,
            ISalonRepository salonRepository,
            IBathroomRepository bathroomRepository,
            IBarRepository barRepository,
            IRequestTransferRepository requestTransferRepository,
            IAlbaranesRepository albaranesRepository,
            IStockChickenByBranchRepository stockChickenByBranchRepository,
            IPhotoGeneralCleaningRepository photoGeneralCleaningRepository,
            IPhotoDrinksTemperatureRepository photoDrinksTemperatureRepository,
            IPhotoSpotlightRepository photoSpotlightRepository,
            IPhotoWashBasinWithSoapPaperRepository photoWashBasinWithSoapPaperRepository,
            IPhotoTicketTableRepository photoTicketTableRepository,
            IPhotoKitchenRepository photoKitchenRepository,
            IPhotoSalonRepository photoSalonRepository,
            IPhotoBathroomRepository photoBathroomRepository,
            IPhotoBarRepository photoBarRepository, IPhotoToSetTableRepository photoToSetTableRepository)
    {

      _mapper = mapper;
      _logger = logger;
      _validateAttendanceRepository = validateAttendanceRepository;
      _salesExpectationRepository = salesExpectationRepository;
      _toSetTableRepository = toSetTableRepository;
      _banosMatutinoRepository = banosMatutinoRepository;
      _validationGasRepository = validationGasRepository;
      _waitlistTableRepository = waitlistTableRepository;
      _alarmRepository = alarmRepository;
      _tipRepository = tipRepository;
      _tabletSafeKeepingRepository = tabletSafeKeepingRepository;
      _livingRoomBathroomCleaningRepository = livingRoomBathroomCleaningRepository;
      _cashRegisterShortageRepository = cashRegisterShortageRepository;
      _riskProductRepository = riskProductRepository;
      _orderRepository = orderRepository;
      _fridgeRepository = fridgeRepository;
      _precookedChickenRepository = precookedChickenRepository;
      _completeProductsInOrderRepository = completeProductsInOrderRepository;
      _fryerCleaningRepository = fryerCleaningRepository;
      _peopleCountingRepository = peopleCountingRepository;
      _satisfactionSurveyRepository = satisfactionSurveyRepository;
      _generalCleaningRepository = generalCleaningRepository;
      _stationRepository = stationRepository;
      _spotlightRepository = spotlightRepository;
      _drinksTemperatureRepository = drinksTemperatureRepository;
      _audioVideoRepository = audioVideoRepository;
      _barCleaningRepository = barCleaningRepository;
      _fridgeSalonRepository = fridgeSalonRepository;
      _bathRoomsOverallStatusRepository = bathRoomsOverallStatusRepository;
      _washBasinWithSoapPaperRepository = washBasinWithSoapPaperRepository;
      _ticketTableRepository = ticketTableRepository;
      _entriesChargedAsDeliveryNoteRepository = entriesChargedAsDeliveryNoteRepository;
      _orderScheduleReviewRepository = orderScheduleReviewRepository;
      _checkTableRepository = checkTableRepository;
      _kitchenRepository = kitchenRepository;
      _salonRepository = salonRepository;
      _bathroomRepository = bathroomRepository;
      _barRepository = barRepository;
      _requestTransferRepository = requestTransferRepository;
      _albaranesRepository = albaranesRepository;
      _stockChickenByBranchRepository = stockChickenByBranchRepository;
      _photoGeneralCleaningRepository = photoGeneralCleaningRepository;
      _photoDrinksTemperatureRepository = photoDrinksTemperatureRepository;
      _photoSpotlightRepository = photoSpotlightRepository;
      _photoWashBasinWithSoapPaperRepository = photoWashBasinWithSoapPaperRepository;
      _photoTicketTableRepository = photoTicketTableRepository;
      _photoKitchenRepository = photoKitchenRepository;
      _photoSalonRepository = photoSalonRepository;
      _photoBathroomRepository = photoBathroomRepository;
      _photoBarRepository = photoBarRepository;
      _photoToSetTableRepository = photoToSetTableRepository;
    }

    /// <summary>
    /// GET:
    /// Regresa lista de tarjetas de centro de control por Sucursal y por Horario de trabajo
    /// </summary>
    /// <param name="branch">ID de Sucursal</param>
    /// <param name="workshift">
    /// <param name="menu1">
    /// || ID    || Turno     ||
    /// =====================
    /// ||   1   || MATUTINO  ||
    /// =====================
    /// ||   2   || VESPERTINO||
    /// =====================
    /// </param>
    /// <returns></returns>
    [HttpGet("{branch}/{workshift}/{menu1}/{idUser}/Manager")]
        [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
        public async Task<ActionResult<ApiResponse<List<ControlCenter>>>> Get(int branch, int workshift, int menu1, int idUser)
        {
            var response = new ApiResponse<ControlCenterData>();
      try
            {
                var data1 = new ControlCenterData();
                data1.ControlCenters = new List<ControlCenter>();
                DateTime today = DateTime.Now;
                var inicio = today.AbsoluteStart();
                var startDay = inicio.AddHours(7);
                var middleDay = inicio.AddHours(17);     //----> correcta
                var diant = middleDay.AddDays(-1);
                var endDay = inicio.AddHours(27);
                //var middleDay = today.AbsoluteEnd();
                //List<ControlCenter> controlCenter = new List<ControlCenter>();
                //CultureInfo cultures = new CultureInfo("es-MX");
                //startDay = startDay.ToUniversalTime();
                //middleDay = middleDay.ToUniversalTime();
                //endDay = endDay.ToUniversalTime();
                //inicio = inicio.ToUniversalTime();

                if (workshift == 1)
                {
                  switch (menu1)
                  {  
                   case 1:
                    data1.ControlCenters.Add(new ControlCenter
                    {
                        Name = "Validación de asistencias",
                        Description = "",
                        IsPercentageOrComplete = false,
                        IsComplete = _validateAttendanceRepository.GetAll().Any(e => e.BranchId == branch && e.CreatedDate >= startDay && e.CreatedDate <= middleDay && e.CreatedBy == idUser),
                        Percentage = 0,
                        Color = _validateAttendanceRepository.GetAll().Any(e => e.BranchId == branch && e.CreatedDate >= startDay && e.CreatedDate <= middleDay && e.CreatedBy == idUser)
                    ? "success" : "warning" //"warning"
                    });
                    data1.ControlCenters.Add(new ControlCenter()
                    {
                        Name = "VALIDACIÓN DE GAS",
                        Description = "",
                        IsPercentageOrComplete = false,
                        IsComplete = _validationGasRepository.GetAll().Any(e => e.Branch == branch && e.CreatedDate >= startDay && e.CreatedDate <= middleDay && e.CreatedBy == idUser),
                        Percentage = 0,
                        Color = _validationGasRepository.GetAll().Any(e => e.Branch == branch && e.CreatedDate >= startDay && e.CreatedDate <= middleDay && e.CreatedBy == idUser)
                        ? "success" : "warning",
                        Id = _validationGasRepository.GetAll().FirstOrDefault(e => e.Branch == branch && e.CreatedDate >= startDay && e.CreatedDate <= middleDay && e.CreatedBy == idUser)?.Id
                    });

                    var avancesm = 0;
                        var sm = _toSetTableRepository.GetAll().FirstOrDefault(e => e.Branch == branch && e.CreatedDate >= startDay && e.CreatedDate <= middleDay && e.CreatedBy == idUser);
                        if (sm == null) { avancesm = 0; }
                        else { if (_photoToSetTableRepository.GetAll().FirstOrDefault(a => a.ToSetTableId == sm.Id && a.Type == 1 && a.CreatedBy == idUser) != null) { avancesm = avancesm + 1; }
                               if (_photoToSetTableRepository.GetAll().FirstOrDefault(a => a.ToSetTableId == sm.Id && a.Type == 2 && a.CreatedBy == idUser) != null) { avancesm = avancesm + 1; }
                               if (_photoToSetTableRepository.GetAll().FirstOrDefault(a => a.ToSetTableId == sm.Id && a.Type == 3 && a.CreatedBy == idUser) != null) { avancesm = avancesm + 1; }
                               if (_photoToSetTableRepository.GetAll().FirstOrDefault(a => a.ToSetTableId == sm.Id && a.Type == 4 && a.CreatedBy == idUser) != null) { avancesm = avancesm + 1; }
                               if (_photoToSetTableRepository.GetAll().FirstOrDefault(a => a.ToSetTableId == sm.Id && a.Type == 5 && a.CreatedBy == idUser) != null) { avancesm = avancesm + 1; }
                               if (_photoToSetTableRepository.GetAll().FirstOrDefault(a => a.ToSetTableId == sm.Id && a.Type == 6 && a.CreatedBy == idUser) != null) { avancesm = avancesm + 1; }
                           
                               
                        }
                        bool completosm;
                        if (avancesm == 6) { completosm = true; } else { completosm = false; }
                    data1.ControlCenters.Add(new ControlCenter()
                    {
                        Name = "SALÓN MONTADO",
                        //Description = "",
                        //IsPercentageOrComplete = false,
                        //IsComplete = _toSetTableRepository.GetAll().Any(e => e.Branch == branch && e.CreatedDate >= startDay && e.CreatedDate <= middleDay && e.CreatedBy == idUser),
                        //Percentage = 0,
                        //Color = _toSetTableRepository.GetAll().Any(e => e.Branch == branch && e.CreatedDate >= startDay && e.CreatedDate <= middleDay && e.CreatedBy == idUser)
                        //? "success" : "warning",

                        Description = "",
                        IsPercentageOrComplete = false,
                        IsComplete = completosm,
                        Percentage = decimal.Parse(avancesm.ToString()),
                        Color = completosm ? "success" : "warning"
                        //Id = _toSetTableRepository.GetAll().FirstOrDefault(e => e.Branch == branch && e.CreatedDate >= startDay && e.CreatedDate <= middleDay && e.CreatedBy == idUser)?.Id
                    });
                    data1.ControlCenters.Add(new ControlCenter()
                    {
                        Name = "MESAS EN ESPERA",
                        Description = "",
                        IsPercentageOrComplete = false,
                        IsComplete = _waitlistTableRepository.GetAll().Any(e => e.Branch == branch && e.CreatedDate >= startDay && e.CreatedDate <= middleDay && e.CreatedBy == idUser),
                        Percentage = 0,
                        Color = _waitlistTableRepository.GetAll().Any(e => e.Branch == branch && e.CreatedDate >= startDay && e.CreatedDate <= middleDay && e.CreatedBy == idUser)
                        ? "success" : "warning",
                        Id = _waitlistTableRepository.GetAll().FirstOrDefault(e => e.Branch == branch && e.CreatedDate >= startDay && e.CreatedDate <= middleDay && e.CreatedBy == idUser)?.Id
                    });
                    
                    var salesExpectation = _stockChickenByBranchRepository.GetAll().FirstOrDefault(f =>
                        f.BranchId == branch && f.CreatedDate >= startDay && f.CreatedDate <= endDay);
                    var stockChicken = _mapper.Map<List<StockChickenGetDto>>(await _salesExpectationRepository.GetAll(branch));
                    var amount = stockChicken.Sum(s => s.Amount);
                    var sales = new SalesExpectationGet()
                    {
                        AmountTotal = salesExpectation?.Amount,
                        SalesExpectationTotal = salesExpectation?.SalesExpectations,
                        CompletePercentage = salesExpectation != null ? (decimal)amount * 100 / salesExpectation.Amount : 0
                    };
                    data1.ControlCenters.Add(new ControlCenter()
                    {
                        Name = "Stock de Pollo",
                        Description = "",
                        IsPercentageOrComplete = true,
                        IsComplete = false,
                        Percentage = sales.CompletePercentage.HasValue ? sales.CompletePercentage.Value : new decimal(),
                        Color = _salesExpectationRepository.GetAll().Any(e => e.Branch == branch && e.CreatedDate >= startDay && e.CreatedDate <= middleDay)
                        ? "success" : "warning"
                    });
                    data1.ControlCenters.Add(new ControlCenter()
                    {

                        

                        Name = "Volado de efectivo",
                        Description = "",
                        IsPercentageOrComplete = false,
                        IsComplete = _cashRegisterShortageRepository.GetAll().Any(e => e.BranchId == branch && e.CreatedDate >= startDay && e.CreatedDate <= endDay && e.CreatedBy == idUser),

                        Percentage = 0,
                        Color = _cashRegisterShortageRepository.GetAll().Any(e => e.BranchId == branch && e.CreatedDate >= startDay && e.CreatedDate <= endDay && e.CreatedBy == idUser)
                        ? "success" : "warning",
                        Id = _cashRegisterShortageRepository.GetAll().FirstOrDefault(e => e.BranchId == branch && e.CreatedDate >= startDay && e.CreatedDate <= endDay && e.CreatedBy == idUser)?.Id
                        
                    });
                    data1.ControlCenters.Add(new ControlCenter()
                    {
                        Name = "BAÑOS MATUTINO",
                        Description = "",
                        IsPercentageOrComplete = false,
                        IsComplete = _banosMatutinoRepository.GetAll().Any(e => e.Branch == branch && e.CreatedDate >= startDay && e.CreatedDate <= middleDay && e.CreatedBy == idUser),
                        Percentage = 0,
                        Color = _banosMatutinoRepository.GetAll().Any(e => e.Branch == branch && e.CreatedDate >= startDay && e.CreatedDate <= middleDay && e.CreatedBy == idUser)
                        ? "success" : "warning",
                        Id = _banosMatutinoRepository.GetAll().FirstOrDefault(e => e.Branch == branch && e.CreatedDate >= startDay && e.CreatedDate <= middleDay && e.CreatedBy == idUser)?.Id
                    });
                    data1.Progress = _fridgeRepository.ProgressTask(data1.ControlCenters.Count(x=>x.IsComplete.Equals(true)), 6);
                    break;
                    case 2:
                    data1.ControlCenters.Add(new ControlCenter()
                    {
                        Name = "CALENDARIO",
                        Description = "",
                        IsPercentageOrComplete = false,
                        IsComplete = _riskProductRepository.GetAll().Any(e => e.BranchId == branch && e.CreatedDate >= startDay && e.CreatedDate <= endDay && e.CreatedBy == idUser),
                        Percentage = 0,
                        Color = _riskProductRepository.GetAll().Any(e => e.BranchId == branch && e.CreatedDate >= startDay && e.CreatedDate <= endDay && e.CreatedBy == idUser)
                            ? "success" : "warning",
                        Id = _riskProductRepository.GetAll().FirstOrDefault(e => e.BranchId == branch && e.CreatedDate >= startDay && e.CreatedDate <= endDay && e.CreatedBy == idUser)?.Id
                    });
                    data1.ControlCenters.Add(new ControlCenter()
                    {
                        Name = "PRODUCTO EN RIESGO",
                        Description = "",
                        IsPercentageOrComplete = false,
                        IsComplete = _riskProductRepository.GetAll().Any(e => e.BranchId == branch && e.CreatedDate >= startDay && e.CreatedDate <= endDay && e.CreatedBy == idUser),
                        Percentage = 0,
                        Color = _riskProductRepository.GetAll().Any(e => e.BranchId == branch && e.CreatedDate >= startDay && e.CreatedDate <= endDay && e.CreatedBy == idUser)
                            ? "success" : "warning",
                        Id = _riskProductRepository.GetAll().FirstOrDefault(e => e.BranchId == branch && e.CreatedDate >= startDay && e.CreatedDate <= endDay && e.CreatedBy == idUser)?.Id
                    });
                    data1.ControlCenters.Add(new ControlCenter()
                    {
                        Name = "TRANSFERENCIAS",
                        Description = "",
                        IsPercentageOrComplete = false,
                        IsComplete = _requestTransferRepository.GetAll().Any(e => (e.FromBranchId == branch || e.ToBranchId == branch) && e.CreatedDate >= startDay && e.CreatedDate <= endDay && e.CreatedBy == idUser),
                        Percentage = 0,
                        Color = _requestTransferRepository.GetAll().Any(e => (e.FromBranchId == branch || e.ToBranchId == branch) && e.CreatedDate >= startDay && e.CreatedDate <= endDay && e.CreatedBy == idUser)
                        ? "success" : "warning",
                        Id = _requestTransferRepository.GetAll().FirstOrDefault(e => (e.FromBranchId == branch || e.ToBranchId == branch) && e.CreatedDate >= startDay && e.CreatedDate <= endDay && e.CreatedBy == idUser)?.Id
                    });
                    
                    break;
                  }
                }
                else if (workshift == 2)
                {
                  switch (menu1)
                  {  
                   case 1:
                     
                    if (DateTime.Now >=  inicio && DateTime.Now < inicio.AddHours(3)) {
                       middleDay = middleDay.AddDays(-1);
                       endDay = endDay.AddDays(-1);
                    }
                    data1.ControlCenters.Add(new ControlCenter()
                    {
                        Name = "Validación de asistencias",
                        Description = "",
                        IsPercentageOrComplete = false,
                        IsComplete = _validateAttendanceRepository.GetAll().Any(e => e.BranchId == branch && e.CreatedDate >= middleDay && e.CreatedDate <= endDay && e.CreatedBy == idUser),
                        Percentage = 0,
                        Color = _validateAttendanceRepository.GetAll().Any(e => e.BranchId == branch && e.CreatedDate >= middleDay && e.CreatedDate <= endDay && e.CreatedBy == idUser)
                        ? "success" : "warning"
                    });

                    data1.ControlCenters.Add(new ControlCenter()
                    {
                        Name = "MESAS EN ESPERA",
                        Description = "",
                        IsPercentageOrComplete = false,
                        IsComplete = _waitlistTableRepository.GetAll().Any(e => e.Branch == branch && e.CreatedDate >= middleDay && e.CreatedDate <= endDay && e.CreatedBy == idUser),
                        Percentage = 0,
                        Color = _waitlistTableRepository.GetAll().Any(e => e.Branch == branch && e.CreatedDate >= middleDay && e.CreatedDate <= endDay && e.CreatedBy == idUser)
                        ? "success" : "warning",
                        Id = _waitlistTableRepository.GetAll().FirstOrDefault(e => e.Branch == branch && e.CreatedDate >= middleDay && e.CreatedDate <= endDay && e.CreatedBy == idUser)?.Id
                    });



                    data1.ControlCenters.Add(new ControlCenter()
                    {
                        Name = "Stock de Pollo",
                        Description = "",
                        IsPercentageOrComplete = false,
                        IsComplete = false,
                        Percentage = 0,
                        Color = _salesExpectationRepository.GetAll().Any(e => e.Branch == branch && e.CreatedDate >= middleDay && e.CreatedDate <= endDay && e.CreatedBy == idUser)
                        ? "success" : "warning"
                    });
                    data1.ControlCenters.Add(new ControlCenter()
                    {
                        Name = "RESGUARDO DE PROPINA",
                        Description = "",
                        IsPercentageOrComplete = false,
                        IsComplete = _tipRepository.GetAll().Any(e => e.BranchId == branch && e.CreatedDate >= middleDay && e.CreatedDate <= endDay && e.CreatedBy == idUser),
                        Percentage = 0,
                        Color = _tipRepository.GetAll().Any(e => e.BranchId == branch && e.CreatedDate >= middleDay && e.CreatedDate <= endDay && e.CreatedBy == idUser)
                        ? "success" : "warning",
                        Id = _tipRepository.GetAll().FirstOrDefault(e => e.BranchId == branch && e.CreatedDate >= middleDay && e.CreatedDate <= endDay && e.CreatedBy == idUser)?.Id
                    });
                    data1.ControlCenters.Add(new ControlCenter()
                    {
                        Name = "LIMPIEZA DE SALÓN Y BAÑOS",
                        Description = "",
                        IsPercentageOrComplete = false,
                        IsComplete = _livingRoomBathroomCleaningRepository.GetAll().Any(e => e.BranchId == branch && e.CreatedDate >= middleDay && e.CreatedDate <= endDay && e.CreatedBy == idUser),
                        Percentage = 0,
                        Color = _livingRoomBathroomCleaningRepository.GetAll().Any(e => e.BranchId == branch && e.CreatedDate >= middleDay && e.CreatedDate <= endDay && e.CreatedBy == idUser)
                        ? "success" : "warning",
                        Id = _livingRoomBathroomCleaningRepository.GetAll().FirstOrDefault(e => e.BranchId == branch && e.CreatedDate >= middleDay && e.CreatedDate <= endDay && e.CreatedBy == idUser)?.Id
                    });
                    data1.ControlCenters.Add(new ControlCenter()
                    {
                        Name = "RESGUARDO DE TABLETA",
                        Description = "",
                        IsPercentageOrComplete = false,
                        IsComplete = _tabletSafeKeepingRepository.GetAll().Any(e => e.BranchId == branch && e.CreatedDate >= middleDay && e.CreatedDate <= endDay && e.CreatedBy == idUser),
                        Percentage = 0,
                        Color = _tabletSafeKeepingRepository.GetAll().Any(e => e.BranchId == branch && e.CreatedDate >= middleDay && e.CreatedDate <= endDay && e.CreatedBy == idUser)
                        ? "success" : "warning",
                        Id = _tabletSafeKeepingRepository.GetAll().FirstOrDefault(e => e.BranchId == branch && e.CreatedDate >= middleDay && e.CreatedDate <= endDay && e.CreatedBy == idUser)?.Id
                    });
                    data1.ControlCenters.Add(new ControlCenter()
                    {
                        Name = "ALARMA",
                        Description = "",
                        IsPercentageOrComplete = false,
                        IsComplete = _alarmRepository.GetAll().Any(e => e.BranchId == branch && e.CreatedDate >= middleDay && e.CreatedDate <= endDay && e.CreatedBy == idUser),
                        Percentage = 0,
                        Color = _alarmRepository.GetAll().Any(e => e.BranchId == branch && e.CreatedDate >= middleDay && e.CreatedDate <= endDay && e.CreatedBy == idUser)
                        ? "success" : "warning",
                        Id = _alarmRepository.GetAll().FirstOrDefault(e => e.BranchId == branch && e.CreatedDate >= middleDay && e.CreatedDate <= endDay && e.CreatedBy == idUser)?.Id
                    });
                    data1.ControlCenters.Add(new ControlCenter()
                    {
                        Name = "Volado de efectivo",
                        Description = "",
                        IsPercentageOrComplete = false,
                        IsComplete = _cashRegisterShortageRepository.GetAll().Any(e => e.BranchId == branch && e.CreatedDate >= middleDay && e.CreatedDate <= endDay && e.CreatedBy == idUser),
                        Percentage = 0,
                        Color = _cashRegisterShortageRepository.GetAll().Any(e => e.BranchId == branch && e.CreatedDate >= middleDay && e.CreatedDate <= endDay && e.CreatedBy == idUser)
                        ? "success" : "warning",
                        Id = _cashRegisterShortageRepository.GetAll().FirstOrDefault(e => e.BranchId == branch && e.CreatedDate >= middleDay && e.CreatedDate <= endDay && e.CreatedBy == idUser)?.Id
                    });


                    data1.Progress = _fridgeRepository.ProgressTask(data1.ControlCenters.Count(x=>x.IsComplete.Equals(true)), 7);
                    break;

                    case 2:
                    data1.ControlCenters.Add(new ControlCenter()
                    {
                        Name = "ALBARANES",
                        Description = "",
                        IsPercentageOrComplete = false,
                        IsComplete = _albaranesRepository.GetAll().Any(e => e.BranchId == branch && e.CreatedDate >= startDay && e.CreatedDate <= endDay && e.CreatedBy == idUser),
                        Percentage = 0,
                        Color = _albaranesRepository.GetAll().Any(e => e.BranchId == branch && e.CreatedDate >= startDay && e.CreatedDate <= endDay && e.CreatedBy == idUser)
                        ? "success" : "warning",
                        Id = _albaranesRepository.GetAll().FirstOrDefault(e => e.BranchId == branch && e.CreatedDate >= startDay && e.CreatedDate <= endDay && e.CreatedBy == idUser)?.Id
                    });
                    data1.ControlCenters.Add(new ControlCenter()
                    {
                        Name = "TRANSFERENCIAS",
                        Description = "",
                        IsPercentageOrComplete = false,
                        IsComplete = _requestTransferRepository.GetAll().Any(e => (e.FromBranchId == branch || e.ToBranchId == branch) && e.CreatedDate >= startDay && e.CreatedDate <= endDay && e.CreatedBy == idUser),
                        Percentage = 0,
                        Color = _requestTransferRepository.GetAll().Any(e => (e.FromBranchId == branch || e.ToBranchId == branch) && e.CreatedDate >= startDay && e.CreatedDate <= endDay && e.CreatedBy == idUser)
                        ? "success" : "warning",
                        Id = _requestTransferRepository.GetAll().FirstOrDefault(e => (e.FromBranchId == branch || e.ToBranchId == branch) && e.CreatedDate >= startDay && e.CreatedDate <= endDay && e.CreatedBy == idUser)?.Id
                    });
                    data1.ControlCenters.Add(new ControlCenter()
                    {
                        Name = "PRODUCTO EN RIESGO",
                        Description = "",
                        IsPercentageOrComplete = false,
                        IsComplete = _riskProductRepository.GetAll().Any(e => e.BranchId == branch && e.CreatedDate >= startDay && e.CreatedDate <= endDay && e.CreatedBy == idUser),
                        Percentage = 0,
                        Color = _riskProductRepository.GetAll().Any(e => e.BranchId == branch && e.CreatedDate >= startDay && e.CreatedDate <= endDay && e.CreatedBy == idUser)
                            ? "success" : "warning",
                        Id = _riskProductRepository.GetAll().FirstOrDefault(e => e.BranchId == branch && e.CreatedDate >= startDay && e.CreatedDate <= endDay && e.CreatedBy == idUser)?.Id
                    });
                    break;
                  }
                }
                
                //var res = _mapper.Map<StockChickenDto>(_salesExpectationRepository.Find(f => f.Id == id));
                //var res = data1.ControlCenters;
                response.Success = true;
                response.Message = "Consult was successfully";
                response.Result = data1;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                response.Success = false;
                response.Message = ex.ToString();
                return StatusCode(500, response);
            }
            return StatusCode(200, response);
        }
        /// <summary>
        /// GET:
        /// Regresa lista para Menu de Regional
        /// </summary>
        /// <param name="branch">ID => Sucursal</param>
        /// <param name="menu">Posición de menú</param>
        /// <param name="user">Posición de menú</param>
        /// <returns></returns>
        [HttpGet("{branch}/{menu}/{user}/Regional")]
        [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status405MethodNotAllowed)]
        public async Task<ActionResult<ApiResponse<ControlCenterData>>> GetRegional(int branch, int menu, int user)
        {
            var response = new ApiResponse<ControlCenterData>();
            try
            {
                var data = new ControlCenterData();
                data.ControlCenters = new List<ControlCenter>();
                var today = DateTime.Now.AddDays(-1);
                today = today.AbsoluteEnd();
                var utctoday = today.ToUniversalTime();

                switch (menu)
                {  
                    case 1:
                                             
                        //var fecha = _orderRepository.GetAll().FirstOrDefault(e => e.BranchId == branch && e.CreatedDate > today).CreatedDate.ToLocalTime();
                        //var fecha2 = _orderRepository.GetAll().FirstOrDefault(e => e.BranchId == branch && e.CreatedDate > today).CreatedDate;
                        data.ControlCenters.Add(new ControlCenter
                        {
                            Name = "ÓRDENES",
                            Description = "",
                            IsPercentageOrComplete = false,
                            IsComplete = _orderRepository.GetAll().Any(e => e.BranchId == branch && e.CreatedDate > utctoday && e.CreatedBy == user),
                            Percentage = 0,
                            Color = _orderRepository.GetAll().Any(e => e.BranchId == branch && e.CreatedDate > utctoday && e.CreatedBy == user) ? "success" : "warning"
                        });
                        data.ControlCenters.Add(new ControlCenter
                        {
                            Name = "Refrigeradores",
                            Description = "",
                            IsPercentageOrComplete = false,
                            IsComplete = _fridgeRepository.GetAll().Any(e => e.BranchId == branch && e.CreatedDate > utctoday && e.CreatedBy == user),
                            Percentage = 0,
                            Color = _fridgeRepository.GetAll().Any(e => e.BranchId == branch && e.CreatedDate > utctoday && e.CreatedBy == user) ? "success" : "warning"
                        });
                        data.ControlCenters.Add(new ControlCenter
                        {
                            Name = "Pollo con precocción",
                            Description = "",
                            IsPercentageOrComplete = false,
                            IsComplete = _precookedChickenRepository.GetAll().Any(e => e.BranchId == branch && e.CreatedDate > utctoday && e.CreatedBy == user),
                            Percentage = 0,
                            Color = _precookedChickenRepository.GetAll().Any(e => e.BranchId == branch && e.CreatedDate > utctoday && e.CreatedBy == user) ? "success" : "warning"
                        });
                        data.ControlCenters.Add(new ControlCenter
                        {
                            Name = "Productos completos y en Órden",
                            Description = "",
                            IsPercentageOrComplete = false,
                            IsComplete = _completeProductsInOrderRepository.GetAll().Any(e => e.BranchId == branch && e.CreatedDate > utctoday && e.CreatedBy == user),
                            Percentage = 0,
                            Color = _completeProductsInOrderRepository.GetAll().Any(e => e.BranchId == branch && e.CreatedDate > utctoday && e.CreatedBy == user) ? "success" : "warning"
                        });
                        data.ControlCenters.Add(new ControlCenter
                        {
                            Name = "Limpieza de freidoras",
                            Description = "",
                            IsPercentageOrComplete = false,
                            IsComplete = _fryerCleaningRepository.GetAll().Any(e => e.BranchId == branch && e.CreatedDate > utctoday && e.CreatedBy == user),
                            Percentage = 0,
                            Color = _fryerCleaningRepository.GetAll().Any(e => e.BranchId == branch && e.CreatedDate > utctoday && e.CreatedBy == user) ? "success" : "warning"
                        });
                        data.Progress = _fridgeRepository.ProgressTask(data.ControlCenters.Count(x=>x.IsComplete.Equals(true)), 4);
                        break;
                    case 2:
                        data.ControlCenters.Add(new ControlCenter
                        {
                            Name = "Conteo de personas",
                            Description = "",
                            IsPercentageOrComplete = false,
                            IsComplete = _peopleCountingRepository.GetAll().Any(e => e.BranchId == branch && e.CreatedDate > utctoday && e.CreatedBy == user),
                            Percentage = 0,
                            Color = _peopleCountingRepository.GetAll().Any(e => e.BranchId == branch && e.CreatedDate > utctoday && e.CreatedBy == user) ? "success" : "warning"
                        });

                        var avance3 = 0;
                        //var encuesta = _satisfactionSurveyRepository.Count(e => e.BranchId == branch && e.CreatedDate > today);
                        //var encuesta = _satisfactionSurveyRepository.;
                        var encuesta = _satisfactionSurveyRepository.GetAll().Where(e => e.BranchId == branch && e.CreatedDate > utctoday && e.CreatedBy == user);
                        if (encuesta.Count() == 0) { avance3 = 0; }
                        else{ avance3 = encuesta.Count(); }
                        bool completoe;
                        if (avance3 == 3) { completoe = true; } else { completoe = false; }
                        data.ControlCenters.Add(new ControlCenter
                        {
                            Name = "Encuesta",
                            Description = "",
                            IsPercentageOrComplete = false,
                            IsComplete = completoe,
                            Percentage = encuesta.Count(),
                            Color = completoe ? "success" : "warning"
                        });

                        var avance = 0;
                        var limpieza = _generalCleaningRepository.GetAll().FirstOrDefault(e => e.BranchId == branch && e.CreatedDate > utctoday && e.CreatedBy == user);
                        if (limpieza == null) { avance = 0; }
                        else { if (_photoGeneralCleaningRepository.GetAll().FirstOrDefault(a => a.GeneralCleaningId == limpieza.Id && a.Type == 1 && a.CreatedBy == user) != null) { avance = avance + 1; }
                               if (_photoGeneralCleaningRepository.GetAll().FirstOrDefault(a => a.GeneralCleaningId == limpieza.Id && a.Type == 2 && a.CreatedBy == user) != null) { avance = avance + 1; }
                               if (_photoGeneralCleaningRepository.GetAll().FirstOrDefault(a => a.GeneralCleaningId == limpieza.Id && a.Type == 3 && a.CreatedBy == user) != null) { avance = avance + 1; }
                               
                        }
                        bool completo;
                        if (avance == 3) { completo = true; } else { completo = false; }
                        data.ControlCenters.Add(new ControlCenter
                        {
                            Name = "Limpieza general",
                            Description = "",
                            IsPercentageOrComplete = false,
                            IsComplete = completo,
                            Percentage = decimal.Parse(avance.ToString()),
                            Color = completo ? "success" : "warning"
                        });
                        data.ControlCenters.Add(new ControlCenter
                        {
                            Name = "Estación",
                            Description = "",
                            IsPercentageOrComplete = false,
                            IsComplete = _stationRepository.GetAll().Any(e => e.BranchId == branch && e.CreatedDate > utctoday && e.CreatedBy == user),
                            Percentage = 0,
                            Color = _stationRepository.GetAll().Any(e => e.BranchId == branch && e.CreatedDate > utctoday && e.CreatedBy == user) ? "success" : "warning"
                        });
                        var avance1 = 0;
                        var bebidas = _drinksTemperatureRepository.GetAll().FirstOrDefault(e => e.BranchId == branch && e.CreatedDate > utctoday && e.CreatedBy == user);
                        if (bebidas == null) { avance1 = 0; }
                        else {
                             if (bebidas.Chope == true) { 
                               if (_photoDrinksTemperatureRepository.GetAll().FirstOrDefault(a => a.DrinkTemperatureId == bebidas.Id && a.Type == 1 && a.CreatedBy == user) != null) { avance1 = avance1 + 1; }
                               if (_photoDrinksTemperatureRepository.GetAll().FirstOrDefault(a => a.DrinkTemperatureId == bebidas.Id && a.Type == 2 && a.CreatedBy == user) != null) { avance1 = avance1 + 1; }
                               if (_photoDrinksTemperatureRepository.GetAll().FirstOrDefault(a => a.DrinkTemperatureId == bebidas.Id && a.Type == 3 && a.CreatedBy == user) != null) { avance1 = avance1 + 1; }
                             }
                             else { avance1 = 3; }
                        }
                        bool completob;
                        if (avance1 == 3) { completob = true; } else { completob = false; }
                        data.ControlCenters.Add(new ControlCenter
                        {
                            Name = "Temperatura de Bebidas",
                            Description = "",
                            IsPercentageOrComplete = false,
                            IsComplete = completob,
                            Percentage = decimal.Parse(avance1.ToString()),
                            Color = completob ? "success" : "warning"
                        });
                        data.ControlCenters.Add(new ControlCenter
                        {
                            Name = "Audio y Video",
                            Description = "",
                            IsPercentageOrComplete = false,
                            IsComplete = _audioVideoRepository.GetAll().Any(e => e.BranchId == branch && e.CreatedDate > utctoday && e.CreatedBy == user),
                            Percentage = 0,
                            Color = _audioVideoRepository.GetAll().Any(e => e.BranchId == branch && e.CreatedDate > utctoday && e.CreatedBy == user) ? "success" : "warning"
                        });
                        var avance2 = 0;
                        var focos = _spotlightRepository.GetAll().FirstOrDefault(e => e.BranchId == branch && e.CreatedDate > utctoday && e.CreatedBy == user);
                        if (focos == null) { avance2 = 0; }
                        else { if (_photoSpotlightRepository.GetAll().FirstOrDefault(a => a.SpotlightId == focos.Id && a.Type == 1 && a.CreatedBy == user) != null) { avance2 = avance2 + 1; }
                               if (_photoSpotlightRepository.GetAll().FirstOrDefault(a => a.SpotlightId == focos.Id && a.Type == 2 && a.CreatedBy == user) != null) { avance2 = avance2 + 1; }
                               
                               
                        }
                        bool completof;
                        if (avance2 == 2) { completof = true; } else { completof = false; }
                        data.ControlCenters.Add(new ControlCenter
                        {
                            Name = "Focos",
                            Description = "",
                            IsPercentageOrComplete = false,
                            IsComplete = completof,
                            Percentage = decimal.Parse(avance2.ToString()),
                            Color = completof ? "success" : "warning"
                        });
                        data.ControlCenters.Add(new ControlCenter
                        {
                            Name = "Limpieza en barra",
                            Description = "",
                            IsPercentageOrComplete = false,
                            IsComplete = _barCleaningRepository.GetAll().Any(e => e.BranchId == branch && e.CreatedDate > utctoday && e.CreatedBy == user),
                            Percentage = 0,
                            Color = _barCleaningRepository.GetAll().Any(e => e.BranchId == branch && e.CreatedDate > utctoday && e.CreatedBy == user) ? "success" : "warning"
                        });
                        data.ControlCenters.Add(new ControlCenter
                        {
                            Name = "Refrigeradores",
                            Description = "",
                            IsPercentageOrComplete = false,
                            IsComplete = _fridgeSalonRepository.GetAll().Any(e => e.BranchId == branch && e.CreatedDate > utctoday && e.CreatedBy == user),
                            Percentage = 0,
                            Color = _fridgeSalonRepository.GetAll().Any(e => e.BranchId == branch && e.CreatedDate > utctoday && e.CreatedBy == user) ? "success" : "warning"
                        });
                        data.Progress = _fridgeRepository.ProgressTask(data.ControlCenters.Count(x=>x.IsComplete.Equals(true)), 9);
                        break;
                    case 3:
                        data.ControlCenters.Add(new ControlCenter
                        {
                            Name = "Estado General",
                            Description = "",
                            IsPercentageOrComplete = false,
                            IsComplete = _bathRoomsOverallStatusRepository.GetAll().Any(e => e.BranchId == branch && e.CreatedDate > utctoday && e.CreatedBy == user),
                            Percentage = 0,
                            Color = _bathRoomsOverallStatusRepository.GetAll().Any(e => e.BranchId == branch && e.CreatedDate > utctoday && e.CreatedBy == user) ? "success" : "warning"
                        });

                        var avancelv = 0;
                        var lavabo = _washBasinWithSoapPaperRepository.GetAll().FirstOrDefault(e => e.BranchId == branch && e.CreatedDate > utctoday && e.CreatedBy == user);
                        if (lavabo == null) { avancelv = 0; }
                        else { if (_photoWashBasinWithSoapPaperRepository.GetAll().FirstOrDefault(a => a.WashbasinWithSoapPaperId == lavabo.Id && a.Type == 1 && a.CreatedBy == user) != null) { avancelv = avancelv + 1; }
                               if (_photoWashBasinWithSoapPaperRepository.GetAll().FirstOrDefault(a => a.WashbasinWithSoapPaperId == lavabo.Id && a.Type == 2 && a.CreatedBy == user) != null) { avancelv = avancelv + 1; }
                               
                               
                        }
                        bool completolv;
                        if (avancelv == 2) { completolv = true; } else { completolv = false; }
                        data.ControlCenters.Add(new ControlCenter
                        {
                            Name = "Lavabos con jabón y papel",
                            Description = "",
                            IsPercentageOrComplete = false,
                            IsComplete = completolv,
                            Percentage = decimal.Parse(avancelv.ToString()),
                            Color = completolv ? "success" : "warning"
                        });
                        data.Progress = _washBasinWithSoapPaperRepository.ProgressTask(data.ControlCenters.Count(x=>x.IsComplete.Equals(true)), 2);
                        break;
                    case 4:
                        var avancetm = 0;
                        var ticket = _ticketTableRepository.GetAll().FirstOrDefault(e => e.BranchId == branch && e.CreatedDate > utctoday && e.CreatedBy == user);
                        if (ticket == null) { avancetm = 0; }
                        else { if (_photoTicketTableRepository.GetAll().FirstOrDefault(a => a.TicketTableId == ticket.Id && a.Type == 1 && a.CreatedBy == user) != null) { avancetm = avancetm + 1; }
                               if (_photoTicketTableRepository.GetAll().FirstOrDefault(a => a.TicketTableId == ticket.Id && a.Type == 2 && a.CreatedBy == user) != null) { avancetm = avancetm + 1; }
                               
                               
                        }
                        bool completotm;
                        if (avancetm == 2) { completotm = true; } else { completotm = false; }
                        data.ControlCenters.Add(new ControlCenter
                        {
                            Name = "Ticket y Mesa",
                            Description = "",
                            IsPercentageOrComplete = false,
                            IsComplete = completotm,
                            Percentage = decimal.Parse(avancetm.ToString()),
                            Color = completotm ? "success" : "warning"
                        });
                        data.ControlCenters.Add(new ControlCenter
                        {
                            Name = "ENTRADAS CARGADAS COMO ALBARÁN ",
                            Description = "",
                            IsPercentageOrComplete = false,
                            IsComplete = _entriesChargedAsDeliveryNoteRepository.GetAll().Any(e => e.BranchId == branch && e.CreatedDate > utctoday && e.CreatedBy == user),
                            Percentage = 0,
                            Color = _entriesChargedAsDeliveryNoteRepository.GetAll().Any(e => e.BranchId == branch && e.CreatedDate > utctoday && e.CreatedBy == user) ? "success" : "warning"
                        });
                        data.ControlCenters.Add(new ControlCenter
                        {
                            Name = "Revisión de Pedido vs calendario",
                            Description = "",
                            IsPercentageOrComplete = false,
                            IsComplete = _orderScheduleReviewRepository.GetAll().Any(e => e.BranchId == branch && e.CreatedDate > utctoday && e.CreatedBy == user),
                            Percentage = 0,
                            Color = _orderScheduleReviewRepository.GetAll().Any(e => e.BranchId == branch && e.CreatedDate > utctoday && e.CreatedBy == user) ? "success" : "warning"
                        });
                        data.ControlCenters.Add(new ControlCenter
                        {
                            Name = "Revisión de Mesas",
                            Description = "",
                            IsPercentageOrComplete = false,
                            IsComplete = _checkTableRepository.GetAll().Any(e => e.BranchId == branch && e.CreatedDate > utctoday && e.CreatedBy == user),
                            Percentage = 0,
                            Color = _checkTableRepository.GetAll().Any(e => e.BranchId == branch && e.CreatedDate > utctoday && e.CreatedBy == user) ? "success" : "warning"
                        });
                        data.Progress = _checkTableRepository.ProgressTask(data.ControlCenters.Count(x=>x.IsComplete.Equals(true)), 4);
                        break;
                    case 5:

                        var avancemc = 0;
                        var mttoc = _kitchenRepository.GetAll().FirstOrDefault(e => e.BranchId == branch && e.CreatedDate > utctoday && e.CreatedBy == user);
                        if (mttoc == null) { avancemc = 0; }
                        else { if (_photoKitchenRepository.GetAll().FirstOrDefault(a => a.KitchenId == mttoc.Id && a.Type == 1 && a.CreatedBy == user) != null) { avancemc = avancemc + 1; }
                               if (_photoKitchenRepository.GetAll().FirstOrDefault(a => a.KitchenId == mttoc.Id && a.Type == 2 && a.CreatedBy == user) != null) { avancemc = avancemc + 1; }
                               if (_photoKitchenRepository.GetAll().FirstOrDefault(a => a.KitchenId == mttoc.Id && a.Type == 3 && a.CreatedBy == user) != null) { avancemc = avancemc + 1; }
                               if (_photoKitchenRepository.GetAll().FirstOrDefault(a => a.KitchenId == mttoc.Id && a.Type == 4 && a.CreatedBy == user) != null) { avancemc = avancemc + 1; }
                               if (_photoKitchenRepository.GetAll().FirstOrDefault(a => a.KitchenId == mttoc.Id && a.Type == 5 && a.CreatedBy == user) != null) { avancemc = avancemc + 1; }
                               if (_photoKitchenRepository.GetAll().FirstOrDefault(a => a.KitchenId == mttoc.Id && a.Type == 6 && a.CreatedBy == user) != null) { avancemc = avancemc + 1; }
                               if (_photoKitchenRepository.GetAll().FirstOrDefault(a => a.KitchenId == mttoc.Id && a.Type == 7 && a.CreatedBy == user) != null) { avancemc = avancemc + 1; }
                               if (_photoKitchenRepository.GetAll().FirstOrDefault(a => a.KitchenId == mttoc.Id && a.Type == 8 && a.CreatedBy == user) != null) { avancemc = avancemc + 1; }
                               if (_photoKitchenRepository.GetAll().FirstOrDefault(a => a.KitchenId == mttoc.Id && a.Type == 9 && a.CreatedBy == user) != null) { avancemc = avancemc + 1; }
                               if (_photoKitchenRepository.GetAll().FirstOrDefault(a => a.KitchenId == mttoc.Id && a.Type == 10 && a.CreatedBy == user) != null) { avancemc = avancemc + 1; }
                               if (_photoKitchenRepository.GetAll().FirstOrDefault(a => a.KitchenId == mttoc.Id && a.Type == 11 && a.CreatedBy == user) != null) { avancemc = avancemc + 1; }                             
                               
                        }
                        bool completomc;
                        if (avancemc == 11) { completomc = true; } else { completomc = false; }
                        data.ControlCenters.Add(new ControlCenter
                        {
                            Name = "Cocina",
                            Description = "",
                            IsPercentageOrComplete = false,
                            IsComplete = completomc,
                            Percentage = decimal.Parse(avancemc.ToString()),
                            Color = completomc ? "success" : "warning"
                        });

                        var avancems = 0;
                        var mttos = _salonRepository.GetAll().FirstOrDefault(e => e.BranchId == branch && e.CreatedDate > utctoday && e.CreatedBy == user);
                        if (mttos == null) { avancemc = 0; }
                        else { if (_photoSalonRepository.GetAll().FirstOrDefault(a => a.SalonId == mttos.Id && a.Type == 1 && a.CreatedBy == user) != null) { avancems = avancems + 1; }
                               if (_photoSalonRepository.GetAll().FirstOrDefault(a => a.SalonId == mttos.Id && a.Type == 2 && a.CreatedBy == user) != null) { avancems = avancems + 1; }
                               if (_photoSalonRepository.GetAll().FirstOrDefault(a => a.SalonId == mttos.Id && a.Type == 3 && a.CreatedBy == user) != null) { avancems = avancems + 1; }
                               if (_photoSalonRepository.GetAll().FirstOrDefault(a => a.SalonId == mttos.Id && a.Type == 4 && a.CreatedBy == user) != null) { avancems = avancems + 1; }
                               if (_photoSalonRepository.GetAll().FirstOrDefault(a => a.SalonId == mttos.Id && a.Type == 5 && a.CreatedBy == user) != null) { avancems = avancems + 1; }
                               if (_photoSalonRepository.GetAll().FirstOrDefault(a => a.SalonId == mttos.Id && a.Type == 6 && a.CreatedBy == user) != null) { avancems = avancems + 1; }
                               if (_photoSalonRepository.GetAll().FirstOrDefault(a => a.SalonId == mttos.Id && a.Type == 7 && a.CreatedBy == user) != null) { avancems = avancems + 1; }
                               if (_photoSalonRepository.GetAll().FirstOrDefault(a => a.SalonId == mttos.Id && a.Type == 8 && a.CreatedBy == user) != null) { avancems = avancems + 1; }
                           
                               
                        }
                        bool completoms;
                        if (avancems == 8) { completoms = true; } else { completoms = false; }
                        data.ControlCenters.Add(new ControlCenter
                        {
                            Name = "Salón",
                            Description = "",
                            IsPercentageOrComplete = false,
                            IsComplete = completoms,
                            Percentage = decimal.Parse(avancems.ToString()),
                            Color = completoms ? "success" : "warning"
                        });

                        var avancemb = 0;
                        var mttob = _bathroomRepository.GetAll().FirstOrDefault(e => e.BranchId == branch && e.CreatedDate > utctoday && e.CreatedBy == user);
                        if (mttob == null) { avancemb = 0; }
                        else { if (_photoBathroomRepository.GetAll().FirstOrDefault(a => a.BathroomId == mttob.Id && a.Type == 1 && a.CreatedBy == user) != null) { avancemb = avancemb + 1; }
                               if (_photoBathroomRepository.GetAll().FirstOrDefault(a => a.BathroomId == mttob.Id && a.Type == 2 && a.CreatedBy == user) != null) { avancemb = avancemb + 1; }
                               if (_photoBathroomRepository.GetAll().FirstOrDefault(a => a.BathroomId == mttob.Id && a.Type == 3 && a.CreatedBy == user) != null) { avancemb = avancemb + 1; }
                               if (_photoBathroomRepository.GetAll().FirstOrDefault(a => a.BathroomId == mttob.Id && a.Type == 4 && a.CreatedBy == user) != null) { avancemb = avancemb + 1; }                         
                               
                        }
                        bool completomb;
                        if (avancemb == 4) { completomb = true; } else { completomb = false; }
                        data.ControlCenters.Add(new ControlCenter
                        {
                            Name = "Baños",
                            Description = "",
                            IsPercentageOrComplete = false,
                            IsComplete = completomb,
                            Percentage = decimal.Parse(avancemb.ToString()),
                            Color = completomb ? "success" : "warning"
                        });

                        var avancembr = 0;
                        var mttobr = _barRepository.GetAll().FirstOrDefault(e => e.BranchId == branch && e.CreatedDate > utctoday && e.CreatedBy == user);
                        if (mttobr == null) { avancembr = 0; }
                        else { if (_photoBarRepository.GetAll().FirstOrDefault(a => a.BarId == mttobr.Id && a.Type == 1 && a.CreatedBy == user) != null) { avancembr = avancembr + 1; }
                               if (_photoBarRepository.GetAll().FirstOrDefault(a => a.BarId == mttobr.Id && a.Type == 2 && a.CreatedBy == user) != null) { avancembr = avancembr + 1; }
                               if (_photoBarRepository.GetAll().FirstOrDefault(a => a.BarId == mttobr.Id && a.Type == 3 && a.CreatedBy == user) != null) { avancembr = avancembr + 1; }
                               if (_photoBarRepository.GetAll().FirstOrDefault(a => a.BarId == mttobr.Id && a.Type == 4 && a.CreatedBy == user) != null) { avancembr = avancembr + 1; }                         
                               
                        }
                        bool completombr;
                        if (avancembr == 4) { completombr = true; } else { completombr = false; }
                        data.ControlCenters.Add(new ControlCenter
                        {
                            Name = "Barra",
                            Description = "",
                            IsPercentageOrComplete = false,
                            IsComplete = completombr,
                            Percentage = decimal.Parse(avancembr.ToString()),
                            Color = completombr ? "success" : "warning"
                        });
                        data.Progress = _barRepository.ProgressTask(data.ControlCenters.Count(x=>x.IsComplete.Equals(true)), 4);
                        break;
                    default:
                        response.Success = false;
                        response.Message = "Option no valid";
                        return StatusCode(405, response);
                }
                
                response.Success = true;
                response.Result = data;
                response.Message = "Consult was success";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                response.Success = false;
                response.Message = ex.Message;
                return StatusCode(500, response);
            }
            return StatusCode(200, response);
        }

    }
}
