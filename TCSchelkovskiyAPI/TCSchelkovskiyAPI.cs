﻿using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCSchelkovskiyAPI.Models;
namespace TCSchelkovskiyAPI
{
    public static class TCSchelkovskiyAPI
    {
        static RestRequest request;
        static IRestResponse response;

        public const string HOST = "https://navigator.useful.su";
        public static List<FloorModel> GetFloors()
        {
            string url = HOST + "/api/v1/floors";
            List<FloorModel> floors = new List<FloorModel>();
            try
            {
                RestClient client = new RestClient(url);
                request = new RestRequest(Method.GET);
                request.AddHeader("Accept", "application/json");
                request.AddHeader("Content-Type", "application/json");
                response = client.Execute(request);

                dynamic data = JsonConvert.DeserializeObject(response.Content);

                foreach (var user in data)
                {
                    FloorModel floor = new FloorModel
                    {
                        ImagesPrefix = user.imagesPrefix.ToString(),
                        ID = Convert.ToInt32(user.id),
                        Floor = Convert.ToInt32(user.floor),
                        Name = user.name.ToString(),
                    };
                    List<ShopModel> shops = new List<ShopModel>();
                    try
                    {
                        foreach (var shop in user.shops)
                        {
                            ShopModel shopModel = new ShopModel
                            {
                                IconURI = shop.iconUri.ToString(),
                                ID = Convert.ToInt32(shop.id),
                                Name = shop.name.ToString(),
                                Icon = shop.icon.ToString(),
                                Description = shop.description.ToString(),
                                Phone = shop.phone.ToString(),
                            };
                        }
                    }
                    catch { }


                    floor.Shops = shops;
                    floors.Add(floor);

                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return floors;
        }
        public static List<CategoryModel> GetCategories()
        {
            string url = HOST + $"/api/v1/categories";
            List<CategoryModel> categories = new List<CategoryModel>();
            try
            {
                RestClient client = new RestClient(url);
                request = new RestRequest(Method.GET);
                request.AddHeader("Accept", "application/json");
                request.AddHeader("Content-Type", "application/json");
                response = client.Execute(request);

                dynamic data = JsonConvert.DeserializeObject(response.Content);
                foreach (var cat in data)
                {
                    CategoryModel category = new CategoryModel
                    {
                        IconURI = cat.iconUri.ToString(),
                        ID = Convert.ToInt32(cat.id),
                        Name = cat.name.ToString(),
                        Icon = cat.icon.ToString(),
                    };
                    List<ShopModel> shopModels = new List<ShopModel>();
                    try
                    {
                        foreach (var shop in cat.shops)
                        {
                            try
                            {
                                ShopModel shopModel = new ShopModel
                                {
                                    IconURI = shop.imagesPrefix.ToString() + shop.iconUri.ToString(),
                                    ID = Convert.ToInt32(shop.id),
                                    Name = shop.name.ToString(),
                                    Icon = shop.icon.ToString(),
                                    Description = shop.description.ToString(),
                                    Phone = shop.phone.ToString(),
                                    //Floor = new FloorModel
                                    //{
                                    //    ID = Convert.ToInt32(shop.floor.id),
                                    //    Floor = Convert.ToInt32(shop.floor.floor),
                                    //    Name = shop.floor.name.ToString(),
                                    //    Shops = null
                                    //},
                                };
                                List<PhotoModel> shopPhotos = new List<PhotoModel>();
                                try
                                {
                                    if (shop.images != null)
                                    {
                                        foreach (var photo in shop.images)
                                        {
                                            PhotoModel shopPhoto = new PhotoModel
                                            {
                                                Image = photo.image.ToString(),
                                                ImageURI = photo.imageUri.ToString(),
                                            };
                                        }
                                    }
                                }
                                catch { }
                                shopModel.Photos = shopPhotos;
                                shopModels.Add(shopModel);
                            }
                            catch { }

                            
                        }
                    }
                    catch { }
                    categories.Add(category);
                }  
                

            }
            catch (Exception ex)
            {
                Debug.WriteLine("Искл");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine(ex.StackTrace);
                
            }
            return categories;
        }
        public static CategoryModel GetCategory(int id)
        {
            string url = HOST + $"/api/v1/categories/{id}";
            try
            {
                RestClient client = new RestClient(url);
                request = new RestRequest(Method.GET);
                request.AddHeader("Accept", "application/json");
                request.AddHeader("Content-Type", "application/json");
                response = client.Execute(request);

                dynamic data = JsonConvert.DeserializeObject(response.Content);
                foreach (var cat in data)
                {
                    CategoryModel category = new CategoryModel
                    {
                        IconURI = cat.iconUri.ToString(),
                        ID = Convert.ToInt32(cat.id),
                        Name = cat.name.ToString(),
                        Icon = cat.icon.ToString(),
                    };
                    List<ShopModel> shopModels = new List<ShopModel>();
                    foreach (var shop in cat.shops)
                    {
                        ShopModel shopModel = new ShopModel
                        {
                            IconURI = shop.iconUri.ToString(),
                            ID = Convert.ToInt32(shop.id),
                            Name = shop.name.ToString(),
                            Icon = shop.icon.ToString(),
                            Description = shop.description.ToString(),
                            Phone = shop.phone.ToString(),
                            Floor = new FloorModel
                            {
                                ID = Convert.ToInt32(shop.floor.id),
                                Floor = Convert.ToInt32(shop.floor.floor),
                                Name = shop.floor.name.ToString(),
                                Shops = null
                            },
                        };
                        List<PhotoModel> shopPhotos = new List<PhotoModel>();
                        foreach (var photo in shop.photos)
                        {
                            PhotoModel shopPhoto = new PhotoModel
                            {
                                Image = photo.image.ToString(),
                                ImageURI = photo.imageUri.ToString(),
                            };
                        }
                        shopModel.Photos = shopPhotos;
                        shopModels.Add(shopModel);
                    }
                    return category;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return new CategoryModel { };
        }
        public static List<ShopModel> GetShops()
        {
            string url = HOST + "/api/v1/shop";
            List<ShopModel> shops = new List<ShopModel>();
            try
            {
                RestClient client = new RestClient(url);
                request = new RestRequest(Method.GET);
                request.AddHeader("Accept", "application/json");
                request.AddHeader("Content-Type", "application/json");
                response = client.Execute(request);

               
                dynamic data = JsonConvert.DeserializeObject(response.Content);
                foreach (var shop in data)
                {
                    Debug.WriteLine("++");
                    ShopModel shopModel = new ShopModel
                    {
                        IconURI = shop.iconUri.ToString(),
                        ID = Convert.ToInt32(shop.id),
                        Name = shop.name.ToString(),
                        Icon = shop.icon.ToString(),
                        Description = shop.description.ToString(),
                        Phone = shop.phone.ToString(),
                        Floor = new FloorModel
                        {
                            ID = Convert.ToInt32(shop.floor.id),
                            Floor = Convert.ToInt32(shop.floor.floor),
                            Name = shop.floor.name.ToString(),
                            Shops = null
                        },
                        Category = new CategoryModel
                        {
                            IconURI = shop.category.ToString(),
                            ID = Convert.ToInt32(shop.category.id),
                            Name = shop.category.name.ToString(),
                            Icon = shop.category.icon.ToString(),
                            Shops = null
                        }
                    };
                    List<PhotoModel> shopPhotos = new List<PhotoModel>();
                    if (shop.photos != null)
                    {
                        foreach (var photo in shop.photos)
                        {
                            PhotoModel shopPhoto = new PhotoModel
                            {
                                Image = photo.image.ToString(),
                                ImageURI = photo.imageUri.ToString(),
                            };
                            shopPhotos.Add(shopPhoto);
                        }
                        shopModel.Photos = shopPhotos;
                        
                    }
                    shops.Add(shopModel);
                    
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);
                Debug.WriteLine(ex.Message);
            }
            Debug.WriteLine("метод выполнен");
            return shops;
        }
        public static ShopModel GetShop(int id)
        {
            string url = HOST + $"/api/v1/shop/{id}";
            try
            {
                RestClient client = new RestClient(url);
                request = new RestRequest(Method.GET);
                request.AddHeader("Accept", "application/json");
                request.AddHeader("Content-Type", "application/json");
                response = client.Execute(request);

                dynamic data = JsonConvert.DeserializeObject(response.Content);
                foreach (var shop in data)
                {
                    ShopModel shopModel = new ShopModel
                    {
                        IconURI = shop.iconUri.ToString(),
                        ID = Convert.ToInt32(shop.id),
                        Name = shop.name.ToString(),
                        Icon = shop.icon.ToString(),
                        Description = shop.description.ToString(),
                        Phone = shop.phone.ToString(),
                        Floor = new FloorModel
                        {
                            ID = Convert.ToInt32(shop.floor.id),
                            Floor = Convert.ToInt32(shop.floor.floor),
                            Name = shop.floor.name.ToString(),
                            Shops = null
                        },
                        Category = new CategoryModel
                        {
                            IconURI = shop.category.ToString(),
                            ID = Convert.ToInt32(shop.category.id),
                            Name = shop.name.ToString(),
                            Icon = shop.icon.ToString(),
                            Shops = null
                        }
                    };
                    List<PhotoModel> shopPhotos = new List<PhotoModel>();
                    foreach (var photo in shop.photos)
                    {
                        PhotoModel shopPhoto = new PhotoModel
                        {
                            Image = photo.image.ToString(),
                            ImageURI = photo.imageUri.ToString(),
                        };
                    }
                    shopModel.Photos = shopPhotos;
                    return shop;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return new ShopModel { };
        }
        public static List<ShopGalleryModel> GetShopsGallery(int? id = null)
        {
            string url = "";
            if (id == null)
            {
                url = HOST + $"/api/v1/shopphotos";
            }
            else
            {
                url = HOST + $"/api/v1/shopphotos/{id}";
            }

            
            List<ShopGalleryModel> gallery = new List<ShopGalleryModel>();
            try
            {
                RestClient client = new RestClient(url);
                request = new RestRequest(Method.GET);
                request.AddHeader("Accept", "application/json");
                request.AddHeader("Content-Type", "application/json");
                response = client.Execute(request);

                dynamic data = JsonConvert.DeserializeObject(response.Content);
                foreach (var item in data)
                {
                    ShopGalleryModel galleryModel = new ShopGalleryModel
                    {
                        Image = item.image.ToString(),
                        ImageURI = item.imageUri.ToString(),
                    };


                    ShopModel shopModel = new ShopModel
                    {
                        IconURI = item.shop.iconUri.ToString(),
                        ID = Convert.ToInt32(item.shop.id),
                        Name = item.shop.name.ToString(),
                        Icon = item.shop.icon.ToString(),
                        Description = item.shop.description.ToString(),
                        Phone = item.shop.phone.ToString(),
                        Floor = new FloorModel
                        {
                            ID = Convert.ToInt32(item.shop.floor.id),
                            Floor = Convert.ToInt32(item.shop.floor.floor),
                            Name = item.shop.floor.name.ToString(),
                            Shops = null
                        },
                        Category = new CategoryModel
                        {
                            IconURI = item.shop.category.ToString(),
                            ID = Convert.ToInt32(item.shop.category.id),
                            Name = item.shop.name.ToString(),
                            Icon = item.shop.icon.ToString(),
                            Shops = null
                        }
                    };
                    List<PhotoModel> shopPhotos = new List<PhotoModel>();
                    foreach (var photo in item.shop.photos)
                    {
                        PhotoModel shopPhoto = new PhotoModel
                        {
                            Image = photo.image.ToString(),
                            ImageURI = photo.imageUri.ToString(),
                        };
                    }
                    shopModel.Photos = shopPhotos;

                    galleryModel.Shop = shopModel;
                    gallery.Add(galleryModel);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return gallery;
        }
        public static List<ParkingModel> GetParking()
        {
            List<ParkingModel> parkingFloors = new List<ParkingModel>();
            string url = HOST + "/api/v1/parking";
            RestClient client = new RestClient(url);
            request = new RestRequest(Method.GET);
            request.AddHeader("Accept", "application/json");
            request.AddHeader("Content-Type", "application/json");
            response = client.Execute(request);

            dynamic data = JsonConvert.DeserializeObject(response.Content);
            foreach (var floor in data)
            {
                try
                {
                    ParkingModel model = new ParkingModel
                    {
                        ImagesPrefix = floor.imagesPrefix.ToString(),
                        Image = floor.image.ToString(),
                        ID = Convert.ToInt32(floor.id),
                        Floor = Convert.ToInt32(floor.floor),
                    };
                    parkingFloors.Add(model);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.StackTrace);
                    Debug.WriteLine(ex.Message);
                }
            }

            return parkingFloors;
        }
        public static List<RuleModel> GetRules() 
        {
            List<RuleModel> rules = new List<RuleModel>();
            string url = HOST + "/api/v1/rules";
            RestClient client = new RestClient(url);
            request = new RestRequest(Method.GET);
            request.AddHeader("Accept", "application/json");
            request.AddHeader("Content-Type", "application/json");
            response = client.Execute(request);

            dynamic data = JsonConvert.DeserializeObject(response.Content);
            foreach (var rule in data)
            {
                try
                {
                    RuleModel model = new RuleModel()
                    {
                       ImagesPrefix = rule.imagesPrefix.ToString(),
                       Title = rule.title.ToString()
                    };
                    List<string> imagesUrls = new List<string>();
                    try
                    {
                        foreach (var img in rule.images)
                        {
                            imagesUrls.Add(img.ToString());
                        }
                    }
                    catch
                    {

                    }
                    model.Images = imagesUrls;
                    rules.Add(model);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.StackTrace);
                    Debug.WriteLine(ex.Message);
                }
            }

            return rules;
        }
        public static List<string> GetBanners()
        {
            List<string> imgUrls = new List<string>();
            string url = HOST + "/api/v1/banners";
            RestClient client = new RestClient(url);
            request = new RestRequest(Method.GET);
            request.AddHeader("Accept", "application/json");
            request.AddHeader("Content-Type", "application/json");
            response = client.Execute(request);


            dynamic data = JsonConvert.DeserializeObject(response.Content);
            foreach (var img in data)
            {
                try
                {
                    imgUrls.Add(img.image.ToString());
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.StackTrace);
                    Debug.WriteLine(ex.Message);
                }
            }

            return imgUrls;
        }
        public static ContactsModel GetContacts()
        {
            string url = HOST + $"/api/v1/contacts";
            try
            {
                RestClient client = new RestClient(url);
                request = new RestRequest(Method.GET);
                request.AddHeader("Accept", "application/json");
                request.AddHeader("Content-Type", "application/json");
                response = client.Execute(request);

                dynamic data = JsonConvert.DeserializeObject(response.Content);
                ContactsModel model = new ContactsModel()
                {
                    ID = Convert.ToInt32(data.id),
                    Address = data.address.ToString(),
                    Phone = data.phone.ToString(),
                    TimeWork = data.time_work.ToString(),
                };
                return model;

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return new ContactsModel { };
        }

        public static AboutMallModel AboutMall() 
        {
            string url = HOST + $"/api/v1/aboutmall";
            try
            {
                RestClient client = new RestClient(url);
                request = new RestRequest(Method.GET);
                request.AddHeader("Accept", "application/json");
                request.AddHeader("Content-Type", "application/json");
                response = client.Execute(request);

                dynamic data = JsonConvert.DeserializeObject(response.Content);
                AboutMallModel aboutMallModel = new AboutMallModel()
                {
                    Description = data[0].description.ToString(),
                    ImagesPrefix = data[0].imagesPrefix.ToString(),
                    MallName = data[0].mall_name.ToString(),
                };
                List<string> images = new List<string>();
                foreach (var img in data[0].images)
                {
                    images.Add(img.ToString());
                }
                aboutMallModel.Images = images;
                return aboutMallModel;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return new AboutMallModel { };

        }
        public static List<VacancyModel> GetVacancies(int? id = null)
        {
            string url = "";
            if (id == null)
            {
                url = HOST + $"/api/v1/vacancy";
            }
            else
            {
                url = HOST + $"/api/v1/vacancy/{id}";
            }
            List<VacancyModel> vacancies = new List<VacancyModel>();
            try
            {
                RestClient client = new RestClient(url);
                request = new RestRequest(Method.GET);
                request.AddHeader("Accept", "application/json");
                request.AddHeader("Content-Type", "application/json");
                response = client.Execute(request);

                dynamic data = JsonConvert.DeserializeObject(response.Content);
                foreach (var item in data)
                {
                    VacancyModel vacancy = new VacancyModel
                    {
                        ID = Convert.ToInt32(item.id),
                        Name = item.name.ToString(),
                        Contact = item.contact.ToString()
                    };
                    List<VacancyBlock> blocks = new List<VacancyBlock>();
                    foreach (var block in item.vacancy_blocks)
                    {
                        blocks.Add(new VacancyBlock
                        {
                            ID = Convert.ToInt32(block.id),
                            Title = block.title.ToString(),
                            Description = block.description.ToString()
                        });
                    }
                    vacancy.VacancyBlocks = blocks;
                    vacancies.Add(vacancy);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return vacancies;
        }

        public static List<PromoModel> GetPromos()
        {
            string url = HOST + "/api/v1/promo";
            List<ShopModel> shops = new List<ShopModel>();
            RestClient client = new RestClient(url);
            request = new RestRequest(Method.GET);
            request.AddHeader("Accept", "application/json");
            request.AddHeader("Content-Type", "application/json");
            response = client.Execute(request);

            List<PromoModel> promos = new List<PromoModel>();
            dynamic data = JsonConvert.DeserializeObject(response.Content);
            foreach (var pr in data)
            {
                try
                {
                    PromoModel promoModel = new PromoModel
                    {
                        ID = Convert.ToInt32(pr.id),
                        Title = pr.title.ToString(),
                        Description = pr.description.ToString(),
                        Ended = pr.ended.ToString(),
                        Shop = new ShopModel
                        {
                            IconURI = $"{pr.shop.iconUri}",
                            ID = Convert.ToInt32(pr.shop.id),
                            Icon = pr.shop.icon.ToString(),
                            Name = pr.shop.name,
                            Description = pr.shop.description.ToString(),
                            Phone = pr.shop.phone.ToString(),
                        }
                    };
                    promos.Add(promoModel);

                }
                catch
                {

                }
            }
            return promos;
        }
        public static List<TerminalModel> GetTerminals()
        {
            string url = HOST + "/api/v1/terminals";
            List<TerminalModel> terminals = new List<TerminalModel>();
            RestClient client = new RestClient(url);
            request = new RestRequest(Method.GET);
            request.AddHeader("Accept", "application/json");
            request.AddHeader("Content-Type", "application/json");
            response = client.Execute(request);

            dynamic data = JsonConvert.DeserializeObject(response.Content);
            foreach (var station in data)
            {
               
                try
                {
                    TerminalModel terminal = new TerminalModel()
                    {
                        ID = Convert.ToInt32(station.id),
                        Name = station.name.ToString(),
                    };
                    try
                    {
                        terminal.Floor = new FloorModel
                        {
                            ID = Convert.ToInt32(station.floor.id),
                            Floor = Convert.ToInt32(station.floor.floor),
                            Name = station.floor.name.ToString(),
                        };
                    }
                    catch { }

                    terminals.Add(terminal);

                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.StackTrace);
                    Debug.WriteLine(ex.Message);
                }
            }
           
            return terminals;
        }
    }
}
