using Newtonsoft.Json;
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
        public static List<FloorModel> GetFloors()
        {
            string url = "https://navigator.useful.su/api/v1/floors";
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
                        ID = Convert.ToInt32(user.id),
                        Floor = Convert.ToInt32(user.floor),
                        Name = user.name.ToString(),
                    };
                    List<ShopModel> shops = new List<ShopModel>();
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

                        CategoryModel category = new CategoryModel
                        {
                            IconURI = shop.category.iconUri.ToString(),
                            ID = Convert.ToInt32(shop.category.id),
                            Name = shop.category.name.ToString(),
                            Icon = shop.category.icon.ToString(),
                            Shops = null
                        };
                        shopModel.Category = category;
                    }

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
            string url = $"https://navigator.useful.su/api/v1/categories";
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
                    categories.Add(category);
                }  
                

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return categories;
        }
        public static CategoryModel GetCategory(int id)
        {
            string url = $"https://navigator.useful.su/api/v1/categories/{id}";
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
            string url = "https://navigator.useful.su/api/v1/shop";
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
                    shops.Add(shopModel);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return shops;
        }
        public static ShopModel GetShop(int id)
        {
            string url = $"https://navigator.useful.su/api/v1/shop/{id}";
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
                url = $"https://navigator.useful.su/api/v1/shopphotos";
            }
            else
            {
                url = $"https://navigator.useful.su/api/v1/shopphotos/{id}";
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

        public static void GetNews(int? id =null) { }
        public static void GetTCInfo() { }
        public static void GetStations() { }
        public static void GetRules() { }
        public static void GetContacts() { }
    }
}
