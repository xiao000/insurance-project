/**
 * Created by Administrator on 2017/12/11.
 */

$(function() {
    // product——add——保额交互
    var num = $(".l_insuredAmount_box").length;
    
    $(document).on('click', '.plusBtn', function(){
        console.log(num);
        $(".l_insuredAmount_box").find(".glyphicon").removeClass("glyphicon-plus plusBtn");
        $(".l_insuredAmount_box").find(".glyphicon").addClass("glyphicon-minus minusBtn");
        
        // var $html = $(".l_insuredAmount_box").html();
        num++;
        $(this).parents(".term").prepend('<div class="l_insuredAmount_box insuredAmount_no">'+
                                            '<div class="title">保额</div>'+
                                            '<div class="inputBox">'+
                                                '<input class="BxAmountFrom" data-val="true" data-val-number="字段 保额从 必须是一个数字。" data-val-regex="保额从必须为正整数" data-val-regex-pattern="^[1-9]\d*$" data-val-required="保额从还未添哦" id="['+num+'].BxAmountFrom" name="['+num+'].BxAmountFrom" type="text" value="" />'+
                                                '<span class="field-validation-valid" data-valmsg-for="['+num+'].BxAmountFrom" data-valmsg-replace="true"></span>'+
                                                ' 万到 '+
                                                '<input class="BxAmountTo" data-val="true" data-val-number="字段 保额到 必须是一个数字。" data-val-regex="保额到必须为正整数" data-val-regex-pattern="^[1-9]\d*$" data-val-required="保额到还未添哦" id="['+num+'].BxAmountTo" name="['+num+'].BxAmountTo" type="text" value="" />万'+
                                                '<span class="field-validation-valid" data-valmsg-for="['+num+'].BxAmountTo" data-valmsg-replace="true"></span>'+
                                            '</div>'+
                                            '<div class="title">费率</div>'+
                                            '<div class="inputBox_r">'+
                                                '<input class="BxPriRate" data-val="true" data-val-number="字段 费率 必须是一个数字。" data-val-regex="费率必须为1-4位小数" data-val-regex-pattern="^[0-9]+(.[0-9]{1,4})?$" data-val-required="费率还未添哦" id="['+num+'].BxPriRate" name="['+num+'].BxPriRate" type="text" value="" />'+
                                                '<span class="field-validation-valid" data-valmsg-for="['+num+'].BxPriRate" data-valmsg-replace="true"></span>'+
                                                '<span class="glyphicon glyphicon-plus plusBtn"></span>'+
                                                '<div class="title len">图片是否要上传</div>'+
                                                '<input class="NeedImg" type="checkbox" name="['+num+'].NeedImg" value="true"><span>是</span>'+
                                            '</div>'+
                                        '</div>');
    });
    $(document).on('click', '.minusBtn', function(){
        $(this).parents(".l_insuredAmount_box").remove();
    });
    // $(document).on('click', '.plusBtn', function(){
    //     var len = $(".insuredAmount_no").length;
    //     $(".l_insuredAmount_box").eq(len).addClass("insuredAmount_no");
    //     $(".l_insuredAmount_box").eq(len).find("input").attr("data-val","true");
    // });
    // $(document).on('click', '.minusBtn', function(){
    //     $(this).parents(".l_insuredAmount_box").removeClass("insuredAmount_no");
    //     $(this).parents(".l_insuredAmount_box").find("input").val("");
    //     $(this).parents(".l_insuredAmount_box").find("input").attr("class","");
    //     $(this).parents(".l_insuredAmount_box").find("input").attr("data-val","false");
    // });

});


