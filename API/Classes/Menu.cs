using API.Models;
using Grpc.Net.Client;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using ProductServices;
using ProtoBuf.Grpc.Configuration;
using StaticContentServices;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text.Json;
using System.Threading.Tasks;

namespace API.Classes
{


    public class dtoMenuMaster
    {
        public string MenuName { get; set; } = null!;
        public string DisplayName { get; set; } = null!;
        public string Routerlink { get; set; } = null!;
        public enmMenuDisplayType DisplayType { get; set; }
        public int DisplayOrder { get; set; }
        public List<dtoMenuMaster> ChildMenu { get; set; } = new();
    }

    public class Menu
    {
        
        private readonly IOptions<GRPCServices> _grpcServices;
        private List<dtoMenuMaster> _menuData = new();
        private string _language=Constant.DEFAULT_LANGUAGE;
        private Dictionary<string,string> _labelData = new();

        public Menu(IOptions<GRPCServices> grpcServices) 
        {
            _grpcServices = grpcServices;
        }

        public string Language { get { return _language; } set { _language = value; } }

        private void ReadFromFile()
        {
            var fullPath = Path.Combine(Constant.ROOT_PATH, Constant.MENU_JSON_PATH);
            var serializer = new Newtonsoft.Json.JsonSerializer();            
            using (var streamReader = new StreamReader(fullPath))
            using (var textReader = new JsonTextReader(streamReader))
            {
                _menuData.AddRange(serializer.Deserialize<List<dtoMenuMaster>>(textReader)!.OrderBy(p=>p.DisplayOrder));
            }            
        }
        private List<dtoMenuMaster> LoadCategory()
        {
            List<dtoMenuMaster> returnData = new List<dtoMenuMaster>();
            using var channel = GrpcChannel.ForAddress(_grpcServices.Value.ProductServices);
            {
                var client = new ICategoryService.ICategoryServiceClient(channel);
                mdlCategoryRequest requestData = new mdlCategoryRequest() {
                    IncludeActiveOnly=true,
                    IncludeAllLanguage=false,                    
                };
                requestData.Language.Add(_language);
                foreach (var tempCategory in client.GetAllCategoryIncludeSubCategory(requestData).Category.OrderBy(p => p.DefaultName))
                {
                    var cDetail=tempCategory.CategoryDetail.FirstOrDefault();
                    dtoMenuMaster cData = new dtoMenuMaster()
                    {
                        MenuName = tempCategory.DefaultName,
                        DisplayName = cDetail?.Name ?? tempCategory.DefaultName,
                        DisplayOrder = 0,
                        DisplayType = enmMenuDisplayType.AlwaysVisible,
                        Routerlink = $"{tempCategory.CategoryId}/{tempCategory.DefaultName}",
                        ChildMenu=new()
                    };
                    foreach (var tempSubCategory in tempCategory.SubCategory.OrderBy(p=>p.DefaultName))
                    {
                        var sDetail = tempSubCategory.SubCategoryDetail.FirstOrDefault();
                        dtoMenuMaster sData = new dtoMenuMaster()
                        {
                            MenuName = tempSubCategory.DefaultName,
                            DisplayName = sDetail?.Name ?? tempSubCategory.DefaultName,
                            DisplayOrder = 0,
                            DisplayType = enmMenuDisplayType.AlwaysVisible,
                            Routerlink = $"category/{tempSubCategory.SubCategoryId}/{tempCategory.DefaultName}/{tempSubCategory.DefaultName}"
                        };
                        cData.ChildMenu.Add(sData);
                    }
                    returnData.Add(cData);
                }
                return returnData;

            }

        }

        private List<dtoMenuMaster> LoadAboutUs()
        {
            List<dtoMenuMaster> returnData = new List<dtoMenuMaster>();
            using var channel = GrpcChannel.ForAddress(_grpcServices.Value.StaticWebServices);
            {
                var client = new  IStaticWeb.IStaticWebClient(channel);
                mdlAboutUsRequest requestData = new mdlAboutUsRequest()
                {
                    IncludeDetails=false,
                    IncludeAllLanguage = false,                    
                };
                requestData.Language.Add(_language);
                foreach (var responseData in client.GetAboutUs(requestData).AboutUs) 
                {
                    var cDetail = responseData.AboutUsDetail.FirstOrDefault();
                    dtoMenuMaster cData = new dtoMenuMaster()
                    {
                        MenuName = responseData.DefaultName,
                        DisplayName = cDetail?.Heading ?? responseData.DefaultName,
                        DisplayOrder = 0,
                        DisplayType = enmMenuDisplayType.AlwaysVisible,
                        Routerlink = $"about/{responseData.AboutUsId}/{responseData.DefaultName}",
                        ChildMenu = new()
                    };
                    returnData.Add(cData);
                }
            }
            return returnData;
        }

        private List<dtoMenuMaster> LoadJoinUs()
        {
            List<dtoMenuMaster> returnData = new List<dtoMenuMaster>();
            using var channel = GrpcChannel.ForAddress(_grpcServices.Value.StaticWebServices);
            {
                var client = new IStaticWeb.IStaticWebClient(channel);
                mdlJoinUsRequest requestData = new mdlJoinUsRequest()
                {
                    IncludeDetails = false,
                    IncludeAllLanguage = false,
                };
                requestData.Language.Add(_language);
                foreach (var responseData in client.GetJoinUs(requestData).JoinUs)
                {
                    var cDetail = responseData.JoinUsDetail.FirstOrDefault();
                    dtoMenuMaster cData = new dtoMenuMaster()
                    {
                        MenuName = responseData.DefaultName,
                        DisplayName = cDetail?.Heading ?? responseData.DefaultName,
                        DisplayOrder = 0,
                        DisplayType = enmMenuDisplayType.AlwaysVisible,
                        Routerlink = $"join/{responseData.JoinUsId}/{responseData.DefaultName}",
                        ChildMenu = new()
                    };
                    returnData.Add(cData);
                }
            }
            return returnData;
        }

        private void LoadLabel(List<Tuple<string,string>> Keys)
        {
            using var channel = GrpcChannel.ForAddress(_grpcServices.Value.StaticWebServices);
            {
                var client = new IStaticWeb.IStaticWebClient(channel);
                mdlLabelRequest requestData = new mdlLabelRequest()
                {
                    IncludeAllLanguage = false
                };
                requestData.Language.Add(_language);
                requestData.Names.AddRange(Keys.Select(p=>p.Item1));
                var responseData=client.GetLabel(requestData);
                _labelData = (from t1 in Keys
                                  join t2 in responseData.Labels on t1.Item1 equals t2.DefaultName into t1t2
                                  from _t1t2 in t1t2.DefaultIfEmpty()
                                  select new { key = t1.Item1, value =  _t1t2?.LabelDetail?.FirstOrDefault()?.Name ?? t1.Item2 })
                                .ToDictionary(p => p.key, q => q.value);

                

            }
        }


        private void ChangeDisplayName(List<dtoMenuMaster> menuDatas)
        {
            foreach (var mData in menuDatas)
            {
                if (_labelData.ContainsKey(mData.MenuName))
                {
                    mData.DisplayName = _labelData[mData.MenuName];
                }
                if (mData.ChildMenu.Count > 0)
                {
                    ChangeDisplayName(mData.ChildMenu);
                }
            }
        }

        private async Task< List<dtoMenuMaster>> GetProcessedMenu()
        {
            Task taskFromFile = Task.Run(()=>ReadFromFile());            
            var taskCategoryMenu = Task.Run(() => LoadCategory());
            var taskAboutUsMenu = Task.Run(() => LoadAboutUs());
            var taskJoinUsMenu = Task.Run(() => LoadAboutUs());
            await taskFromFile;
            List<Tuple< string,string>> labelData = new List<Tuple<string,string>>();
            labelData.AddRange(_menuData.Select(p=>new Tuple<string,string>( p.MenuName,p.DisplayName )));
            labelData.AddRange(_menuData.SelectMany(p => p.ChildMenu.Select(p => new Tuple<string, string>(p.MenuName,p.DisplayName))));
            labelData.AddRange(_menuData.SelectMany(p => p.ChildMenu.SelectMany(q => q.ChildMenu.Select(r=> new Tuple<string, string>(r.MenuName,r.DisplayName)))));
            var taskLabel = Task.Run(() => LoadLabel(labelData ));
            await Task.WhenAll(taskLabel, taskCategoryMenu, taskAboutUsMenu, taskJoinUsMenu);
            ChangeDisplayName(_menuData);
            _menuData.ForEach(p =>
            {
                if (p.MenuName.Equals("Rent", StringComparison.OrdinalIgnoreCase))
                {
                    p.ChildMenu.AddRange(taskCategoryMenu.Result);
                }
                if (p.MenuName.Equals("AboutUs", StringComparison.OrdinalIgnoreCase))
                {
                    p.ChildMenu.AddRange(taskAboutUsMenu.Result);
                }
                if (p.MenuName.Equals("JoinUs", StringComparison.OrdinalIgnoreCase))
                {
                    p.ChildMenu.AddRange(taskJoinUsMenu.Result);                    
                }
            });
           return _menuData;

        }

        public async Task<List<dtoMenuMaster>> GeMenuAsync(InMemoryCache memoryCache)
        {
            Func<Task<List<dtoMenuMaster>>> proc = GetProcessedMenu;
            return await memoryCache.GetOrCreateAsync($"menu_{_language}", proc);
        }
    }
}
