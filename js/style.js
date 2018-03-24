$(document).ready(function(){
	//导航菜单切换
    var urlStr = window.location.href;
    $('#ins_headermenu ul li a').each(function () {
        var href = $(this).attr('href');
        if(urlStr.match(href)){
       		$(this).parent('li').addClass('active').siblings('li').removeClass('active');
       	}
        
    });
    
});


$(document).load(function(){
	//主要内容高度
    $head=$("#ins_header").height()+30;
	$rightmain=$(window).height()-$head-30;
  	$("#ins_rightmain").css("min-height",$rightmain+"px");
    
});	






