//Table Script
$(window).click(function () {
    $('.JStableOuter table').scroll(function (e) {

        $('.JStableOuter thead').css("top", -$(".JStableOuter tbody").scrollTop());
        $('.JStableOuter thead tr th').css("top", $(".JStableOuter table").scrollTop());

    });
});

//Admin Menu
function myFunction() {
    var x = document.getElementById("myDIV");
    if (x.style.display === "block") {
        x.style.display = "none";
    } else {
        x.style.display = "block";
    }
}

//Breadcrumb Hide for Home Page
$(document).ready(function () {
    var pathname = window.location.pathname;
    if (pathname != '/') {
        var x = document.getElementById("homebreadcrumb");
        x.style.display = "block";
    }
});


//$(document).ready(function () {
    
//    $(location).attr('href');

//    var pathname = window.location.pathname.toLowerCase();
//    var parts = pathname.split('/'); //Split for last part
//    var lastSegment = parts.pop() || parts.pop();  

//    var title = jQuery(this).attr('title').split(' ')[0].toLowerCase();

//    if (title == lastSegment) {
//        jQuery('.menu-section ul li').addClass('opened');
//        jQuery('.menu-section ul li ul li a').addClass('active');
        
//    }
   
//});