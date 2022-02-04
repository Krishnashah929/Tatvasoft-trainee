/*<!--Script for side bar-->*/
$(document).ready(function () {
    $('#dismiss, .overlay').on('click', function () {
      // hide sidebar
      $('#sidebar').removeClass('active');
      // hide overlay
      $('.overlay').removeClass('active');
      $('.model').removeClass('active');
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
/* <!--Script for side bar-->*/

/*<!--cookie 's js'--> */
  function myFunction() {
    var x = document.getElementById("privacy-policy-home");
    x.style.display = "none";
  }
/*<!--cookie 's js'--> */

  
window.onscroll = function () { scrollFunction() };
function scrollFunction() {
    if (document.body.scrollTop > 80 || document.documentElement.scrollTop > 80) {
        document.getElementById("navbar").style.backgroundColor = "rgba(19,19,19,0.8)";
    }
}
 


