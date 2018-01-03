//$.ajax("http:/localhost:51244/api/MyApi/Create").then(function (posts) {
//    console.log(posts);
//});


//$.ajax({
//    type: "POST",
//    url: "/LoggedIn",
//    data: $('#postForm').serialize(),
//    datatype: "html",
//    success: function (data) {
//        $('#result').html(data);
//        Console.log("hej");
//    }
//});


//var form = $("#postForm").find("form").serialize();
//$.ajax({
//    type: 'POST', 
//    url: "/api/MyApi/Create",
//    data: form,
//    dataType: 'json',
//    success: function (data) {
//        alert("Funkar"); 
//    }, 
//        error: function (data) {
//            alert("Funkar inte");
//    }
//});


//$(document).ready(function () {
//    $("#ApiIdBtn").click(function () {
//        var message = $("#Message").val();
//        $.ajax({
//            url: Response.Write.Url.Action("Create", "MyApiController"),
//            type: "Post",
//            data: JSON.stringify([message]), 
//            contentType: 'application/json; charset=utf-8',
//            success: function (data) { },
//            error: function () { alert('error'); }
//        });
//    });
//});  


//$ajax({
//    type: "POST",
//    url: Response.Write.Url.Action("Create", "MyApiController"),
//    data: $this.serialize(),
//    dataType: dataType,
//    success: function (result) {
//        alert("Post envoyé.");
//    },
//    error: function (result) {
//        alert("Echec lors de l'envoi du post.");
//    }   
//});

//$("#searchForm").submit(function (event) {
//    event.preventDefault();

//    var $form = $(this),
//        term = $form.find("textArea[name='message']").val(),
//        url = $form.attr("action");

//    var posting = $.post(url, { message: term });

//    posting.done(function (data) {
//        var content = $(data).find("#content");
//        $("#result").empty().append(content);
//    });
//});