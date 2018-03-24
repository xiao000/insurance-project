/**
 * Created by Administrator on 2017/12/11.
 */

$(function() {
    ///////////////////////////////////////////////////////////////////
        // 图片上传
        var tmpl = '<li class="weui-uploader__file" style="background-image:url(#url#)"></li>',
        $gallery = $("#gallery"), 
        $galleryImg = $("#galleryImg"),
        $uploaderInput = $("#uploaderInput"),
        $uploaderFiles = $("#uploaderFiles"),
        _imgNum=""
        ;

        var maxCount = 4;
        var fileNum_old = $('.weui-uploader__file').length;
        $uploaderInput.on("change", function(e){
            var src, url = window.URL || window.webkitURL || window.mozURL, files = e.target.files;
            // alert(files.length);
            // if(files.length>=3){
            //     $(".weui-uploader__input-box").hide();
            //     $("#uploaderInput").hide();
            // }
            
            for (var i = 0, len = files.length; i < len; ++i) {
                
                var file = files[i];
                if($('.weui-uploader__file').length==maxCount){
                    $(".weui-uploader__input-box").hide();
                }
                if ($('.weui-uploader__file').length >= maxCount) {  
                    return;  
                }
                if (url) {
                    src = url.createObjectURL(file);
                } else {
                    src = e.target.result;
                }

                $uploaderFiles.append($(tmpl.replace('#url#', src)));
                if(fileNum_old+files.length>=4){
                  $(".weui-uploader__info").text(4+'/'+maxCount)
                }
            }
            if($('.weui-uploader__file').length==maxCount){
                $(".weui-uploader__input-box").hide();
            }
            var fileNum = $('.weui-uploader__file').length;
            $(".weui-uploader__info").text(fileNum+'/'+maxCount)
            
        });
        $uploaderFiles.on("click", "li", function(){
            $galleryImg.attr("style", this.getAttribute("style"));
            _imgNum = $(this).index();
            $gallery.fadeIn(100);
        });
        $gallery.on("click", function(){
            $gallery.fadeOut(100);
        });
        $(".weui-gallery__del").click(function () {
            $(".weui-uploader__file").eq(_imgNum).remove();
            $(".weui-uploader__input-box").show();
            var fileNum = $('.weui-uploader__file').length;
            $(".weui-uploader__info").text(fileNum + '/' + maxCount)
        });
        /////////////////////////////////////////////////////////////////
});