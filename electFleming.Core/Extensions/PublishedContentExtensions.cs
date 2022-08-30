using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Extensions;

namespace electFleming.Core.Extensions
{
    public static class PublishedContentExtensions
    {
        public static string GetAltText(this IPublishedContent mediaItem)
        {
            if (mediaItem != null && mediaItem.HasProperty("altText") && mediaItem.HasValue("altText"))
            {
                return mediaItem.Value<string>("altText");
            }

            return "";
        }

        public static string GetMetaDescription(this IPublishedContent content)
        {
            if (content == null)
                throw new ArgumentNullException("Content is null");

            var text = "";
            if (content.HasProperty("metaDescription") && content.HasValue("metaDescription"))
            {
                text = content.Value<string>("metaDescription");
            }
            else if (content.HasProperty("SubTitle") && content.HasValue("SubTitle"))
            {
                text = content.Value<string>("SubTitle");
            }
            else
            {
                var n = content;
                var nodeIsRoot = (n.Parent == null);
                while (!nodeIsRoot && text != "")
                {
                    n = n.Parent;
                    if (n == null)
                    {
                        nodeIsRoot = true;
                    }
                    else if (n.HasProperty("metaDescription") && n.HasValue("metaDescription"))
                    {
                        text = n.Value<string>("metaDescription");
                    }
                    else if (n.HasProperty("SubTitle") && n.HasValue("SubTitle"))
                    {
                        text = n.Value<string>("SubTitle");
                    }
                }
            }

            if (text == "")
                text = content.Name;

            return text;
        }

        public static string GetMetaTitle(this IPublishedContent content)
        {
			if (content == null) 
				throw new ArgumentNullException("Content is null");

            var title = "";
            if (content.HasProperty("metaTitle") && content.HasValue("metaTitle"))
            {
                title = content.Value<string>("metaTitle");
            }
            else if (content.HasProperty("Title") && content.HasValue("Title"))
            {
                title = content.Value<string>("Title");
            }
            else
            {
                var n = content;
                var nodeIsRoot = (n.Parent == null);
                while (!nodeIsRoot && title != "")
                {
                    n = n.Parent;
                    if (n == null)
                    {
                        nodeIsRoot = true;
                    }
                    else if (n.HasProperty("metaTitle") && n.HasValue("metaTitle"))
                    {
                        title = n.Value<string>("metaTitle");
                    }
                    else if (n.HasProperty("Title") && n.HasValue("Title"))
                    {
                        title = n.Value<string>("Title");
                    }
                }               
            }

            if (title == "")
                title = content.Name;

            return title;
        }


        public static string GetPageAuthor(this IPublishedContent content)
        {
            if (content == null)
                throw new ArgumentNullException("Content is null");

            var author = "";

            if (content.HasProperty("Author") && content.HasValue("Author"))
            {
                author = content.Value<string>("Author");
            }
            if (content.HasProperty("metaAuthor") && content.HasValue("metaAuthor"))
            {
                author = content.Value<string>("metaAuthor");
            }
            else
            {
                var n = content;
                var nodeIsRoot = (n.Parent == null);
                while (!nodeIsRoot && author != "")
                {
                    n = n.Parent;
                    if (n == null)
                    {
                        nodeIsRoot = true;
                    }
                    else if (n.HasProperty("Author") && n.HasValue("Author"))
                    {
                        author = n.Value<string>("Author");
                    }
                    else if (n.HasProperty("metaTitle") && n.HasValue("metaTitle"))
                    {
                        author = n.Value<string>("metaAuthor");
                    }
                }
            }
            
            return author;
        }

        public static string GetPageImageUrl(this IPublishedContent content)
        {
            if (content == null)
                throw new ArgumentNullException("Content is null");

            var width = 600;
            var height = 400; ;
            var imageUrl = "";
            

            if (content.HasProperty("MainImage") && content.HasValue("MainImage"))
            {
                var croppedImage = content.Value<MediaWithCrops>("MainImage");
                var cropped_url = croppedImage.GetCropUrl(width, height);
                if (cropped_url == null)
                {
                    var imageList = content.Value<IEnumerable<IPublishedContent>>("mainImage");
                    foreach (var imageItem in imageList)
                    {
                        var img_url = imageItem.GetCropUrl(width, height);
                        if (img_url != null)
                        {
                            imageUrl = img_url;
                            break;
                        }
                    }
                }
                else
                {
                    imageUrl = cropped_url;
                }
            }            
            else
            {
                var n = content;
                var nodeIsRoot = (n.Parent == null);
                while (!nodeIsRoot && imageUrl == "")
                {
                    n = n.Parent;
                    if (n == null)
                    {
                        nodeIsRoot = true;
                    }
                    else if (n.HasProperty("MainImage") && n.HasValue("MainImage"))
                    {
                        var croppedImage = n.Value<MediaWithCrops>("MainImage");
                        var cropped_url = croppedImage.GetCropUrl(width, height);
                        if (cropped_url == null)
                        {
                            var imageList = n.Value<IEnumerable<IPublishedContent>>("mainImage");
                            foreach (var imageItem in imageList)
                            {
                                var img_url = imageItem.GetCropUrl(width, height);
                                if (img_url != null)
                                {
                                    imageUrl = img_url;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            imageUrl = cropped_url;
                        }
                    }                    
                }
            }
            if (imageUrl.Length > 0)
            {
                imageUrl = "https://flemingforputnam.com/" + imageUrl.TrimStart('/');
            }

            return imageUrl;
        }
        public static string GetPageTitle(this IPublishedContent content)
        {
            if (content == null)
                throw new ArgumentNullException("Content is null");

            var title = "";
            
            if (content.HasProperty("Title") && content.HasValue("Title"))
            {
                title = content.Value<string>("Title");
            }
            if (content.HasProperty("metaTitle") && content.HasValue("metaTitle"))
            {
                title = content.Value<string>("metaTitle");
            }
            else
            {
                var n = content;
                var nodeIsRoot = (n.Parent == null);
                while (!nodeIsRoot && title != "")
                {
                    n = n.Parent;
                    if (n == null)
                    {
                        nodeIsRoot = true;
                    }                    
                    else if (n.HasProperty("Title") && n.HasValue("Title"))
                    {
                        title = n.Value<string>("Title");
                    }
                    else if (n.HasProperty("metaTitle") && n.HasValue("metaTitle"))
                    {
                        title = n.Value<string>("metaTitle");
                    }
                }
            }

            if (title == "")
                title = content.Name;

            return title;
        }

   

        public static string GetCanonicalUrl(this IPublishedContent content)
        {
            if (content != null && content.HasProperty("canonicalUrl") && content.HasValue("canonicalUrl"))
            {
                return content.Value<string>("canonicalUrl");
            }

            return content.Url(mode: UrlMode.Absolute);
        }

        public static string GetSiteName(this IPublishedContent content)
        {
            var homePage = content.AncestorOrSelf("homePage");
            if (homePage != null && homePage.HasProperty("siteName") && homePage.HasValue("siteName"))
            {
                return homePage.Value<string>("siteName");
            }

            return "Portfolio";
        }
    }
}