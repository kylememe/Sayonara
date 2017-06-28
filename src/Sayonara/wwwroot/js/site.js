// Write your Javascript code.
$(function () {

    $("div.hoverhighlight,tr.hoverhighlight").hover(
      function () {
          $(this).addClass("highlighted");
      }, function () {
          $(this).removeClass("highlighted");
      }
    );    
});