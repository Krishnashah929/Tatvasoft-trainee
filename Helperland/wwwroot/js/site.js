// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
/*<!--Script for side bar-->*/
$(document).ready(function () {
    $('#dismiss, .overlay').on('click', function () {
        // hide sidebar
        $('#sidebar').removeClass('active');
        // hide overlay
        $('.overlay').removeClass('active');
        /*$('.model').removeClass('active');*/
    });
    $('#sidebarCollapse').on('click', function () {
        // open sidebar
        $('#sidebar').addClass('active');
        // fade in the overlay
        $('.overlay').addClass('active');
        $('.collapse.in').toggleClass('in');
        $('a[aria-expanded=true]').attr('aria-expanded', 'false');
    });
});
 
/*<!--cookie 's js'--> */
function myFunction() {
    var x = document.getElementById("privacy-policy-home");
    x.style.display = "none";
}

function preventBack() { window.history.forward(); }
setTimeout("preventBack()", 0);
window.onunload = function () { null };

//function logoutclick() {
//    debugger;
//    $.ajax({
//        url: '/Home/OnLogOut',
//        type: 'POST',
//        success: function (result) {
//            if (result.value == "true") {
//                preventBack();
//            }
//            else {
//                alert("Invalid");
//            }
//        }
//    });
//}

 