//@summary: Gallery to represent gallery of images.
//@Components
//
//  - Gallery        : Represents the core functionality of the gallery.
//  - GalleryRenderer: Used to build the UI of the gallery.
//                     This is separate from the Gallery events/settings/behaviour
//                     so that potentially the gallery can be made to look different
//                     without affecting the behaviour
//  - GalleryHelper  : Contains various helper functions that can be reused.
//
//    <style type="text/css">
//        .gallery { border:solid 1px black; width:400px; }
//        .gallery .name { color:#333333; font-size:15; }
//        .gallery .desc { color:#999999; font-size:13; }
//        .gallery .images {}
//        .gallery .images .detail {}
//        .gallery .info {}
//        .gallery .info .title {}
//        .gallery .info .caption { color:#cccccc; font-size:11;}
//        .gallery .number {font-family:Verdana; color:#999999; font-size:9px; font-weight:normal; } 
//        .gallery .navarea {background-color:Black; }
//        .gallery .navarea .inner {background-color:Black; margin-left:auto; margin-right:auto; }
//        .gallery .navarea .inner .nav { } 
//        .gallery .navarea .inner .nav a { color:White; text-decoration: none; font-size:13px; }
//        .gallery .navarea .inner .nav a:link { color:White; text-decoration: none; }
//        .gallery .navarea .inner .nav a:visited { color:White; text-decoration: none; }
//        .gallery .navarea .inner .nav a:hover { color:White; text-decoration: none; }
//        .gallery .navarea .inner .list { } 
//        .gallery .navarea .inner .list a { margin:2px 2px 2px 2px; }
//    </style>
//
//@Example:
//  var imageData = 
//  {
//      Name  : "Testing of images",
//      Description: "This is the description of the group of images",
//      Titles: ["Title for image1", "Title for image2", "Title for image3", "Title for image4",
//               "Title for image5", "Title for image6", "Title for image7", "Title for image8"],
//      Links : ["/images/sample_1.jpg","/images/sample_2.jpg","/images/sample_3.jpg","/images/sample_4.jpg",
//               "/images/sample_5.jpg","/images/sample_6.jpg","/images/sample_7.jpg","/images/sample_8.jpg"],
//      Captions  : ["image 1", "image 2", "image 3", "image 4", "image 5", "image 6", "image 7", "image 8"],
//      ThumbNails: [],
//      Dims  : ["100wX100h","100wX100h","100wX100h","100wX100h","100wX100h","100wX100h","100wX100h","100wX100h"]
//  };
function Gallery(data, settings) {
    this.Helper = new GalleryHelper(this);
    this.Renderer = new GalleryRenderer(this, this.Helper);
    this.Images = data;
    this.LastIndex = 0;
    this.CurrentIndex = 0;
    this.TotalImages = data.Links == null ? 0 : data.Links.length;
    this.Settings = settings;
    this.OnImageDetailClicked = null;
    this.OnNavigationChanged = null;
    this.OnDisplayComplete = null;
    this.OnInitComplete = null;


    //@summary: Initialize the settings with the settings dictionary supplied.
    this.Init = function () {
        // Check that various settings have been supplied and/or default them.
        this.CheckOrDefault("JSObjectName",     "_gallery");
        this.CheckOrDefault("GalleryId",        "gallery_01");
        this.CheckOrDefault("DivId",            "gallery_01");
        this.CheckOrDefault("IsPaginated",      false);        
        this.CheckOrDefault("PageSize",         10);
        this.CheckOrDefault("MaxPanelWidth",    400);
        this.CheckOrDefault("MaxPanelHeight",   400);
        this.CheckOrDefault("IsAjaxBased",      false);
        this.CheckOrDefault("AjaxUrl",          "");
        this.CheckOrDefault("NumberInfoFormat", "{0} of {1}");
        this.CheckOrDefault("PreviousText",     "<");
        this.CheckOrDefault("NextText",         ">");
        this.CheckOrDefault("AutoPlayEnabled",  false);
        this.CheckOrDefault("AutoPlaySpeed",    1000);
        this.CheckOrDefault("ThumbNailWidth",   60);
        this.CheckOrDefault("ThumbNailHeight",  60);

        // Create empty captions/dimensions/thumbnails if not supplied.
        var isEmptyList = function (lst) { return lst == null || lst.length == 0 };
        if (isEmptyList(this.Images.Titles))    this.Images.Titles = this.CreateEmtpyList(this.TotalImages);
        if (isEmptyList(this.Images.Captions))  this.Images.Captions = this.CreateEmtpyList(this.TotalImages);
        if (isEmptyList(this.Images.ThumbNails)) this.Images.ThumbNails = this.CreateEmtpyList(this.TotalImages);
        if (isEmptyList(this.Images.Dims))      this.Images.Dims = this.CreateEmtpyList(this.TotalImages);
        if (this.IsNullOrEmpty(this.Images.Name)) this.Images.Name = "";
        if (this.IsNullOrEmpty(this.Images.Description)) this.Images.Description = "";

        // Have the renderder initialize the UI.
        this.Renderer.Init();
    };


    //@summary: Display the images.
    this.Display = function () {
        // Initialzie and show first image.
        this.Init();
        this.OnSelect(0);
    };


    //@summary: Handler for when an imagelink( small image ) is clicked.
    this.OnSelect = function (ndx) {
        if (ndx < 0 || ndx >= this.Images.Links.length)
            return;

        this.LastIndex = this.CurrentIndex;
        this.CurrentIndex = ndx;
        this.Renderer.Display();
    };


    //@summary: Handle for navigating to next image.
    this.OnNext = function () {
        this.OnSelect(this.CurrentIndex + 1);
    };


    //@summary: Handle for navigating to previous image
    this.OnPrevious = function () {
        this.OnSelect(this.CurrentIndex - 1);
    };


    //@summary: Activates the autoplay feature.
    this.SetAutoPlay = function (enabled) {
        this.Settings.AutoPlayEnabled = enabled;
        if (this.Settings.AutoPlayEnabled) this.OnAutoPlayNext();
    };


    //@summary: Used to continue the auto playing of images.
    this.OnAutoPlayNext = function () {
        // On?
        if (!this.Settings.AutoPlayEnabled) return;

        // Check the index.
        if (this.CurrentIndex >= this.Images.Links.length - 1) {
            this.Settings.AutoPlayEnabled = false;
            return;
        }
        this.OnNext();
        window.setTimeout(this.Settings.JSObjectName + ".OnAutoPlayNext()", this.Settings.AutoPlaySpeed);
    };


    this.GetImageId = function (ndx) { return "gallery_img_" + ndx; };

    this.IsNullOrEmpty = function (str) { return str == null || str == undefined || str == ""; };

    this.CreateEmtpyList = function (count) {
        var items = [];
        for (var ndx = 0; ndx < count; ndx++)
            items.push("");
        return items;
    };


    //@summary: Get the value of the setting if available or default value otherwise.
    this.CheckOrDefault = function (key, defaultValue) {
        if(this.Settings[key] == null) 
            this.Settings[key] = defaultValue;
    };
}



///////////////////////////////////////////////////////////////////////
//@summary: Helper class for gallery.
///////////////////////////////////////////////////////////////////////
function GalleryHelper(gallery) {

    this.Gallery = Gallery;


    //@summary: Display the image links.
    //@note: This will update the links if scrolling is enabled.
    this.GetPageRange = function (gallery) {
        if (!gallery.Settings.IsPaginated)
            return [0, gallery.Images.Links.length - 1];

        // Scrolling logic applicable?
        // # of links more than ScrollSize ?
        if (gallery.Images.Links.length <= gallery.Settings.PageSize)
            return [0, gallery.Images.Links.length - 1];


        // Simple math.
        // 10 images. CurrentImage = 8
        var isLessEqualPageSize = gallery.Images.Links.length <= gallery.Settings.PageSize;
        var pageNum = Math.floor(gallery.CurrentIndex / gallery.Settings.PageSize);
        var startIndex = isLessEqualPageSize ? 0 : pageNum * gallery.Settings.PageSize;
        var endIndex = isLessEqualPageSize
                     ? gallery.Images.Links.length - 1
                     : Math.min(startIndex + (gallery.Settings.PageSize - 1), gallery.Images.Links.length - 1);
        return [startIndex, endIndex];
    };


    //@summary: Gets and formats the resource string associated with the key supplied
    // using the arg array values.
    //@param key: The key representing the resource string to get.
    //@param args: An array of values to use for replacing/formating values in the resource string
    //@example: Resources.Format("OrderWithArgs", [3.14, 'abc', 'foo'])
    this.Format = function (msg, args) {
        if (args == null || args.length == 0) return msg;

        for (var ndx = 0; ndx < args.length; ndx++)
            msg = msg.replace(new RegExp('\\{' + ndx + '\\}', 'gm'), args[ndx]);

        return msg;
    };


    //@summary: Gets the proportional width and height of an image that can fit in the max width, max height supplied.
    this.GetFittedImageDimensions = function(width, height, maxWidth, maxHeight)
    {
        // Already fitted?
        if(width < maxWidth && height < maxHeight ) 
            return [width, height];

        // Width is more
        if( width > maxWidth && height < maxHeight )
            return [maxWidth, height * (maxWidth/width)];

        // Height is more
        if( height > maxHeight && width < maxWidth )
            return [width * (maxHeight/height), maxHeight];

        // Both width and height are more
        return [maxWidth, height * (maxWidth/width)];
    };
}



////////////////////////////////////////////////////////////////////////
//@summary: Class to render the image gallery.
//@note: This is the default renderer for the gallery.
//       If a custom gallery UI is required the following methods need
//       to be implemented as they are used by the gallery component.
//       - Init();
//       - Display();
////////////////////////////////////////////////////////////////////////
function GalleryRenderer(gallery, helper) {
    this.Gallery = gallery;
    this.Helper = helper;

    this.Init = function () {
        // Set up the divs areas.
        var name    = '<div id="imgname" class="name">'         + this.Gallery.Images.Name + '</div>';
        var desc    = '<div id="imgdesc" class="desc">'         + this.Gallery.Images.Description + '</div>';
        var detail  = '<div id="imgdetails" class="images">'    + this.BuildImages() + '</div>';
        var info    = '<div id="imginfo" class="info">'         + this.BuildInfo() + '</div>';        
        var nav     = '<div id="imgnavarea" class="navarea">'   + this.BuildNavArea() + '</div>';
        var number  = '<div id="imgnumber" class="number"></div>';
        var html    = name + desc + detail + nav + number + info;
        document.getElementById(this.Gallery.Settings.DivId).innerHTML = html;
    };


    //@summary: Displays the gallery.
    this.Display = function () {
        this.DisplayImage();
        this.DisplayInfo();
        this.DisplayNavigation();
        this.DisplayNumber();
    };


    //@summary: Display the image
    this.DisplayImage = function () {
        // Hide the last one.
        document.getElementById(this.Gallery.GetImageId(this.Gallery.LastIndex)).style.display = "none";
        document.getElementById(this.Gallery.GetImageId(this.Gallery.CurrentIndex)).style.display = "block";
    };


    //@summary: Display the image number e.g. "Image 2 of 10"
    this.DisplayNumber = function () {
        var html = this.Helper.Format(this.Gallery.Settings.NumberInfoFormat, [this.Gallery.CurrentIndex + 1, this.Gallery.Images.Links.length]);
        document.getElementById("imgnumber").innerHTML = html;
    };


    //@summary: Display the image number e.g. "Image 2 of 10"
    this.DisplayNavigation = function () {
        var html = this.BuildNavArea();
        document.getElementById("imgnavarea").innerHTML = html;
    };


    //@summary: Dispaly the caption for the selected image.                        
    this.DisplayInfo = function () {
        document.getElementById("imgtitle").innerHTML = this.Gallery.Images.Titles[this.Gallery.CurrentIndex];
        document.getElementById("imgcaption").innerHTML = this.Gallery.Images.Captions[this.Gallery.CurrentIndex];
    };


    //@summary: Build images for pre-loading.
    this.BuildImages = function () {
        var html = "";
        for (var ndx = 0; ndx < this.Gallery.Images.Links.length; ndx++) {
            var pattern = '<div id="{0}" class="detail" style="display:none"><img border="0" alt="{1}" src="{2}" {3} /></div>';
            var imageId = this.Gallery.GetImageId(ndx);
            var dimensions = "";
            if(this.Gallery.Images.Dims[ndx] != null)
            {
                // More than gallery size?
                var dims = this.Helper.GetFittedImageDimensions(this.Gallery.Images.Dims[ndx][0], this.Gallery.Images.Dims[ndx][1], 
                            this.Gallery.Settings.MaxPanelWidth, this.Gallery.Settings.MaxPanelHeight);
                width = dims[0];
                height = dims[1];
                // Check for 0 width or height.
                if(width == 0 || height == 0 )
                    dimensions = ' ';
                else
                    dimensions = ' width="' + width + '" height="' + height + '" ';
            }
            html = html + this.Helper.Format(pattern, [imageId, this.Gallery.Images.Captions[ndx], this.Gallery.Images.Links[ndx], dimensions]);
        }
        return html;
    };


    //@summary: Build the navigation area.
    this.BuildInfo = function () {
        var html = '<div id="imgtitle" class="title"></div>'
                 + '<div id="imgcaption" class="caption"></div>';
        return html;
    };


    //@summary: Build the navigation area.
    this.BuildNavArea = function () {
        var html = '<div class="inner"><table align="center"><tr>'
                + this.BuildNav("gallery_prev", this.Gallery.Settings.PreviousText, this.Gallery.Settings.JSObjectName + ".OnPrevious();", true, "td")
                + '<td>' + '<div class="list">' + this.BuildLinks() + '</div>' + '</td>'
                + this.BuildNav("gallery_next", this.Gallery.Settings.NextText, this.Gallery.Settings.JSObjectName + ".OnNext();", true, "td")
                + '</tr></table></div>';
        return html;
    };


    //@summary: Builds the list of small images.
    this.BuildLinks = function () {
        var gallery = this.Gallery;
        var helper = this.Helper;
        var builder = function (thumbnailWidth, thumbnailHeight) {
            var pattern = '<a href="{0}" onclick="{1}"><img border="0" alt="{2}" src="{3}" width="{4}" height={5} /></a>';
            var html = "";
            var range = helper.GetPageRange(gallery);
            for (var ndx = range[0]; ndx <= range[1]; ndx++) {
                var desc = gallery.Images.Captions == null ? "" : gallery.Images.Captions[ndx];
                var args = ["#", gallery.Settings.JSObjectName + ".OnSelect(" + ndx + ");", desc, gallery.Images.Links[ndx], thumbnailWidth, thumbnailHeight];
                html = html + helper.Format(pattern, args);
            }
            return html;
        };
        var listhtml = builder(gallery.Settings.ThumbNailWidth, gallery.Settings.ThumbNailHeight);
        return listhtml;
    };


    //@summary: Builds the navigation button.( previous next ).
    this.BuildNav = function (id, text, js, wrap, tag) {
        var htm = '<div class="nav"><a id="{0}" href="#" onclick="{1}">{2}</a></div>';
        if (wrap) htm = "<" + tag + ">" + htm + "</" + tag + ">";
        return this.Helper.Format(htm, [id, js, text]);
    };
}



var gallerySampleData =
{
    Name: "Sample Gallery",
    Description: "Sample images for gallery demo",
    Titles: ["Jennifer", "Turtle", "Boxer", "Gaga", "Skier", "Jet", "Car", "House", "Bridge"],
    Captions: ["Friends", "Baby turtle", "Champion", "Singer", "Winter sports", "Fly high", "Travel", "West Side"],
    Dims: [[392,154], [392,154], [576,324], [392,154], [467,262], [392,154], [392,154], [467,262] ],
    Links: ["/images/sample_1.jpg", "/images/sample_2.jpg", "/images/sample_3.jpg", "/images/sample_4.jpg",
            "/images/sample_5.jpg", "/images/sample_6.jpg", "/images/sample_7.jpg", "/images/sample_8.jpg"],
    
};