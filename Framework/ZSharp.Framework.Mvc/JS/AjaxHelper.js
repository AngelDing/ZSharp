//假设我们定义返回的 JSON 是这样的格式
//{
//    "code": "int, 0 表示成功，其它值表示出错",
//    "message": "string, 附加的消息，可选",
//    "data": "object，附加的数据，可选
//};


//然后为项目公共类 app 定义一个 ajax 方法
//app.ajax = function(button, url, data) {
//    if (button) {
//        button.prop("disabled", true);
//    }

//    var deferred = $.Deferred();
//    $.ajax(url, {
//        type: "post",
//        dataType: "json",
//        data: data
//    }).done(function(json) [
//        if (json.code !== 0) {
//            showError(json.message || "操作发生错误");
//        deferred.reject();
//    } else {
//            deferred.resolve(json);
//}
//}).fail(function() {
//    showError("服务器错误，请稍后再试");
//    deferred.reject();
//}).always(function() {
//    if (button) {
//        button.prop("disabled", false);
//    }
//});
//return deferred.promise();
//};

//// 调用
//app.ajax("do/example", getFormData()).done(function(json) {
//    // 正常处理 json.data 就好
//});


//TODO:採用ES6 Promise進行改進
https://segmentfault.com/a/1190000003691961